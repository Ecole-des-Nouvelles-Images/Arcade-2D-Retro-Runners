using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Vincent.Scripts.Player;

namespace Vincent.Scripts.Knight
{
    public class KnightController : PlayerController
    {
        public PlayerInput playerInput;
        public float speed;
        public float jumpForce;
        private Vector2 _movementInput;
        private Rigidbody2D _rigidbody2D;
        public BoxCollider2D _groundCollider;
        private bool _isGrounded;
        private int _jumpCount;

        private bool _canDash = true;
        private bool _isDashing;
        public float dashPower = 12;
        private float _dashingTime = 0.2f;
        private float _dashingCooldown = 1f;
        private TrailRenderer _trail;

        //private SpriteRenderer _spriteRenderer;
        public ParticleSystem _jumpParticle;
        //private PlayerManager _playerManager;

        public GameObject attackBox;
        public GameObject upAttackBox;
        public GameObject downAttackBox;
        private float _attackTime = 0.4f;
        private float _attackCooldown = 0.4f;
        private bool _canAttack;
        private bool _damageTake;
        private float _iFrame = 0.2f;
        public int health;
        public int Dies;
        public int listID;
        public Sprite portrait;
        
        public GameObject upPointer;
        public GameObject leftPointer;
        public GameObject rightPointer;
        
        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _jumpCount = 2;
            _trail = GetComponent<TrailRenderer>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerManager = FindObjectOfType<PlayerManager>();
            attackBox.SetActive(false);
            upAttackBox.SetActive(false);
            downAttackBox.SetActive(false);
            health = 150;
            Dies = 0;
            _canAttack = true;
            
            upPointer.GetComponent<SpriteRenderer>().color = _spriteRenderer.color;
            leftPointer.GetComponent<SpriteRenderer>().color = _spriteRenderer.color;
            rightPointer.GetComponent<SpriteRenderer>().color = _spriteRenderer.color;
            upPointer.SetActive(false);
            leftPointer.SetActive(false);
            rightPointer.SetActive(false);
            _rigidbody2D.velocity = Vector2.zero;
        }

        private void Start()
        {
            _playerManager.Players.Add(this.gameObject);
            listID = _playerManager.Players.Count;
            _spriteRenderer.color = _playerManager.Players.Count switch {
                1 => color1,
                2 => color2,
                3 => color3,
                4 => color4,
                _ => _spriteRenderer.color
            };
        }

        private void Update() {
            //transform.Translate(new Vector3(_movementInput.x, 0, 0) * speed * Time.deltaTime) ;
            _rigidbody2D.AddForce(new Vector3(_movementInput.x, 0, 0) * speed * Time.deltaTime, ForceMode2D.Impulse);
            transform.localScale = _movementInput.x switch {
                < 0 => new Vector3(-1, 1, 1),
                > 0 => new Vector3(1, 1, 1),
                _ => transform.localScale
            };
            if (health <= 0) {
                Dies += 1;
                Respawn();
            }
            upPointer.transform.position = new Vector3(transform.position.x, 13.8f,0);
            leftPointer.transform.position = new Vector3(-14.25f, transform.position.y, 0);
            leftPointer.transform.rotation = Quaternion.Euler(0,0,90);
            rightPointer.transform.position = new Vector3(14.25f, transform.position.y, 0);
            rightPointer.transform.rotation = Quaternion.Euler(0,0,-90);
        }
        
        

        public void OnMove(InputAction.CallbackContext ctx)
        {
            if (playerInput)
            {
                _movementInput = ctx.ReadValue<Vector2>();
            }   
        }

        // Left Joystick -> Keyboard A and D
        public void OnJump(InputAction.CallbackContext ctx) {
            if (ctx.performed && playerInput) Jump();
        }
        // A button / X button -> Keyboard Space
        public void OnDash(InputAction.CallbackContext ctx) {
            if (ctx.performed && _canDash && playerInput) {
                switch (_movementInput.x)
                {
                    case > 0 when dashPower < 0:
                    case < 0 when dashPower > 0:
                        dashPower = -dashPower;
                        break;
                }
                StartCoroutine(Dash());
            }
        }
        // Right trigger (RT, R2) -> Keyboard E
        public void OnAttack(InputAction.CallbackContext ctx) {
            if (ctx.performed && _canAttack && _movementInput.y > 0.5f && playerInput) {
                StartCoroutine(AttackUp());
            }
            else if (ctx.performed && _canAttack && _movementInput.y < -0.5 && !_isGrounded && playerInput) {
                StartCoroutine(AttackDown());
            }
            else if (ctx.performed && _canAttack && playerInput) {
                StartCoroutine(Attack());
            }
        }

        private void Jump() {
            if (_jumpCount > 1) return;
            //jumpForce = 650;
            //Vector2 jumpVec = new Vector2(0, jumpForce * Time.deltaTime);
            Vector2 jumpVec = new Vector2(0, jumpForce);
            _rigidbody2D.AddForce(transform.up * jumpVec, ForceMode2D.Impulse);
            _jumpCount += 1;
            _jumpParticle.Play();
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("UpOutZone")) {
                upPointer.SetActive(true);
            }
            if (other.CompareTag("LeftOutZone")) {
                leftPointer.SetActive(true);
            }
            if (other.CompareTag("RightOutZone")) {
                rightPointer.SetActive(true);
            }
            if (!other.CompareTag("Ground") || !_groundCollider) return;
            _isGrounded = true;
            _jumpCount = 0;
        }
        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("UpOutZone")) {
                upPointer.SetActive(false);
            }
            if (other.CompareTag("LeftOutZone")) {
                leftPointer.SetActive(false);
            }
            if (other.CompareTag("RightOutZone")) {
                rightPointer.SetActive(false);
            }
            if (!other.CompareTag("Ground") || !_groundCollider) return;
            _isGrounded = false;
        }
        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Attack") && !_damageTake || other.CompareTag("Arrow") && !_damageTake) {
                StartCoroutine(TakeDamage());
            }
            if (other.CompareTag("DeathZone")) {
                health = 0;
            }
        }

        private void Respawn() {
            transform.position = new Vector3(0, 15, 0);
            health = 150;
        }
        
        //Coroutine Dash
        private IEnumerator Dash() {
            _canDash = false;
            _isDashing = true;
            float originalGravity = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.velocity = new Vector2(dashPower, _rigidbody2D.velocity.y);
            _trail.emitting = true;
            yield return new WaitForSeconds(_dashingTime);
            _rigidbody2D.gravityScale = originalGravity;
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            _trail.emitting = false;
            _isDashing = false;
            yield return new WaitForSeconds(_dashingCooldown);
            _canDash = true;
        }
        //Coroutine Attack
        private IEnumerator Attack() {
            _canAttack = false;
            attackBox.SetActive(true);
            yield return new WaitForSeconds(_attackTime);
            attackBox.SetActive(false);
            yield return new WaitForSeconds(_attackCooldown);
            _canAttack = true;
        }
        private IEnumerator AttackUp() {
            _canAttack = false;
            upAttackBox.SetActive(true);
            yield return new WaitForSeconds(_attackTime);
            upAttackBox.SetActive(false);
            yield return new WaitForSeconds(_attackCooldown);
            _canAttack = true;
        }
        
        private IEnumerator AttackDown() {
            _canAttack = false;
            downAttackBox.SetActive(true);
            yield return new WaitForSeconds(_attackTime);
            downAttackBox.SetActive(false);
            yield return new WaitForSeconds(_attackCooldown);
            _canAttack = true;
        }

        //Couroutine Dégats
        private IEnumerator TakeDamage() {
            _damageTake = true;
            health -= 10;
            yield return new WaitForSeconds(_iFrame);
            _damageTake = false;
        }
        
    }
    
    
}
