using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Vincent_Prod.Scripts.Managers;

namespace Vincent_Prod.Scripts.Characters
{
    public class Paladin : PlayerController
    {
        //Move
        private bool _canMove;
        
        //Guard
        private bool _canGuard = true;
        private bool _isGuarding;
        private float _guardTime = 0.75f;
        private float _guardCooldown = 1f;
        public GameObject guardBox;
        public GameObject upGuardBox;
        
        //Attack
        public GameObject attackBox;
        public GameObject upAttackBox;

        //UI
        public Sprite portrait;

        //Visuel
        private SpriteRenderer _spriteRenderer;
        
        //Manager
        private PlayerManager _playerManager;
        public int listID;

        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _jumpCount = 2;
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerManager = FindObjectOfType<PlayerManager>();
            guardBox.SetActive(false);
            upGuardBox.SetActive(false);
            attackBox.SetActive(false);
            upAttackBox.SetActive(false);
            health = 170;
            deaths = 0;
            _canAttack = true;
            _canMove = true;
            _rigidbody2D.velocity = Vector2.zero;
            respawnPoint = GameObject.FindWithTag("Respawn");
        }

        private void Start() {
            _playerManager.Players.Add(this.gameObject);
            listID = _playerManager.Players.Count;
            _spriteRenderer.color = _playerManager.Players.Count switch {
                1 => color1,
                2 => color2,
                3 => color3,
                4 => color4,
                _ => _spriteRenderer.color
            };
            upPointer.GetComponent<SpriteRenderer>().color = _spriteRenderer.color;
            leftPointer.GetComponent<SpriteRenderer>().color = _spriteRenderer.color;
            rightPointer.GetComponent<SpriteRenderer>().color = _spriteRenderer.color;
            upPointer.SetActive(false);
            leftPointer.SetActive(false);
            rightPointer.SetActive(false);
        }

        private void Update() {
            switch (iceArena) {
                case false:
                    if(_canMove) transform.Translate(new Vector3(movementInput.x, 0, 0) * speed * Time.deltaTime) ;
                    break;
                case true:
                    if(_canMove) _rigidbody2D.AddForce(new Vector2(movementInput.x,0)* rbSpeed * Time.deltaTime);
                    break;
            }
            transform.localScale = movementInput.x switch {
                < 0 => new Vector3(-1, 1, 1),
                > 0 => new Vector3(1, 1, 1),
                _ => transform.localScale
            };
            if (health <= 0) {
                deaths += 1;
                Respawn();
            }
            upPointer.transform.position = new Vector3(transform.position.x, 13.8f,0);
            leftPointer.transform.position = new Vector3(-14.25f, transform.position.y, 0);
            leftPointer.transform.rotation = Quaternion.Euler(0,0,90);
            rightPointer.transform.position = new Vector3(14.25f, transform.position.y, 0);
            rightPointer.transform.rotation = Quaternion.Euler(0,0,-90);
            if (_respawning) transform.position = respawnPoint.transform.position;
        }

        private void FixedUpdate() {
            upPointer.SetActive(false);
            leftPointer.SetActive(false);
            rightPointer.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Ground") || !groundCollider) return;
            _isGrounded = true;
            _jumpCount = 0;
        }
        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("UpOutZone")) upPointer.SetActive(false);
            if (other.CompareTag("LeftOutZone")) leftPointer.SetActive(false);
            if (other.CompareTag("RightOutZone")) rightPointer.SetActive(false);
            if (!other.CompareTag("Ground") || !groundCollider) return;
            _isGrounded = false;
        }
        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Attack") && !_damageTake || other.CompareTag("Arrow") && !_damageTake) {
                StartCoroutine(TakeDamage());
            }
            if (other.CompareTag("Spell") && !_damageTake) {
                StartCoroutine(TakeSpellDamage());
            }
            if (other.CompareTag("UpOutZone")) upPointer.SetActive(true);
            if (other.CompareTag("LeftOutZone")) leftPointer.SetActive(true);
            if (other.CompareTag("RightOutZone")) rightPointer.SetActive(true);
            if (other.CompareTag("DeathZone")) health = 0;
        }
        
        public void OnMove(InputAction.CallbackContext ctx) {
            if (playerInput) {
                movementInput = ctx.ReadValue<Vector2>();
            }
        }
        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && playerInput) Jump();
        }
        public void OnGuard(InputAction.CallbackContext ctx)
        {
            switch (ctx.performed)
            {
                case true when _canGuard && movementInput.y > 0.5f && playerInput:
                    StartCoroutine(GuardUp());
                    break;
                case true when _canGuard && playerInput:
                    StartCoroutine(Guard());
                    break;
            }
        }
        public void OnAttack(InputAction.CallbackContext ctx)
        {
            switch (ctx.performed)
            {
                case true when _canAttack && movementInput.y > 0.5f && playerInput:
                    StartCoroutine(AttackUp());
                    break;
                case true when _canAttack && playerInput:
                    StartCoroutine(Attack());
                    break;
            }
        }
        
        private void Respawn()
        {
            _rigidbody2D.velocity = Vector2.zero;
            transform.position = respawnPoint.transform.position;
            health = 170;
            StartCoroutine(RespawnStun());
        }
        //Couroutine Respawn
        private IEnumerator RespawnStun()
        {
            _respawning = true;
            _damageTake = true;
            yield return new WaitForSeconds(_respawnTime);
            _rigidbody2D.velocity = Vector2.zero;
            _respawning = false;
            _damageTake = false;
        }
        private void Jump() {
            if (_jumpCount > 1) return;
            //jumpForce = 650;
            //Vector2 jumpVec = new Vector2(0, jumpForce * Time.deltaTime);
            Vector2 jumpVec = new Vector2(0, jumpPower);
            _rigidbody2D.AddForce(transform.up * jumpVec, ForceMode2D.Impulse);
            _jumpCount += 1;
        }
        
        //Couroutines Guard
        private IEnumerator Guard() {
            _canGuard = false;
            _isGuarding = true;
            _canMove = false;
            guardBox.SetActive(true);
            yield return new WaitForSeconds(_guardTime);
            guardBox.SetActive(false);
            _isGuarding= false;
            _canMove = true;
            yield return new WaitForSeconds(_guardCooldown);
            _canGuard = true;
        }
        private IEnumerator GuardUp() {
            _canGuard = false;
            _isGuarding = true;
            _canMove = false;
            upGuardBox.SetActive(true);
            yield return new WaitForSeconds(_guardTime);
            upGuardBox.SetActive(false);
            _isGuarding= false;
            _canMove = true;
            yield return new WaitForSeconds(_guardCooldown);
            _canGuard = true;
        }
        
        //Couroutines Attack
        private IEnumerator Attack() {
            _canAttack = false;
            _canMove = false;
            attackBox.SetActive(true);
            yield return new WaitForSeconds(attackTime);
            _canMove = true;
            attackBox.SetActive(false);
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }
        private IEnumerator AttackUp() {
            _canAttack = false;
            _canMove = false;
            upAttackBox.SetActive(true);
            yield return new WaitForSeconds(attackTime);
            _canMove = true;
            upAttackBox.SetActive(false);
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }
        
        //Couroutine Damage
        private IEnumerator TakeDamage() {
            _damageTake = true;
            health -= 10;
            yield return new WaitForSeconds(_iFrame);
            _damageTake = false;
        }
        private IEnumerator TakeSpellDamage()
        {
            _damageTake = true;
            health -= 2;
            yield return new WaitForSeconds(_iFrame);
            _damageTake = false;
        }
    }
}
