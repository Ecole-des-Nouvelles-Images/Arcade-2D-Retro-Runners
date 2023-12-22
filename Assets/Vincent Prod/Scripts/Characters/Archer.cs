using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vincent_Prod.Scripts.Arenas.Gravity_Arena;
using Vincent_Prod.Scripts.Arenas.Wind_Arena;
using Vincent_Prod.Scripts.Managers;

namespace Vincent_Prod.Scripts.Characters
{
    public class Archer : PlayerController
    {
        //Dash
        private bool _canDash = true;
        private bool _isDashing;
        public float dashPower = 12;
        private float _dashingTime = 0.2f;
        private float _dashingCooldown = 1f;
        private TrailRenderer _trail;

        //Attack
        public GameObject arrowPrefab;
        private float _arrowDirection;

        //UI
        public Sprite portrait;

        //Visuel
        private SpriteRenderer _spriteRenderer;
        public Animator animator;
        
        //Manager
        private PlayerManager _playerManager;
        public int listID;

        private void Awake() {
            _baseSpeed = speed;
            RespawnVFX.SetActive(false);
            kills = 0;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _jumpCount = 2;
            _trail = GetComponent<TrailRenderer>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerManager = FindObjectOfType<PlayerManager>();
            health = 150;
            deaths = 0;
            _canAttack = true;
            _rigidbody2D.velocity = Vector2.zero;
            respawnPoint = GameObject.FindWithTag("Respawn");
        }
        private void Start() {
            _playerManager.Players.Add(this.gameObject);
            listID = _playerManager.Players.Count;
            light2D.color = _playerManager.Players.Count switch {
                1 => color1,
                2 => color2,
                3 => color3,
                4 => color4,
                _ => _spriteRenderer.color
            };
            if (GravityManager.GravityArena) {
                transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            }
            upPointer.GetComponent<SpriteRenderer>().color = light2D.color;
            leftPointer.GetComponent<SpriteRenderer>().color = light2D.color;
            rightPointer.GetComponent<SpriteRenderer>().color = light2D.color;
            if (listID == 1) { PlayerDataHandler.Instance.playerOnePortrait = portrait; }
            else if (listID == 2) { PlayerDataHandler.Instance.playerTwoPortrait = portrait; }
            else if (listID == 3) { PlayerDataHandler.Instance.playerThreePortrait = portrait; }
            else if (listID == 4) { PlayerDataHandler.Instance.playerFourPortrait = portrait; }
        }
        private void Update() {
            switch (iceArena) {
                case false:
                    transform.Translate(new Vector3(movementInput.x, 0, 0) * speed * Time.deltaTime) ;
                    break;
                case true:
                    _rigidbody2D.AddForce(new Vector2(movementInput.x,0)* rbSpeed * Time.deltaTime);
                    break;
            }
            if (movementInput.x != 0) { animator.SetBool("Walk", true); }
            else { animator.SetBool("Walk", false); }
            if (GravityManager.GravityUp) { animator.SetFloat("VeloY", _rigidbody2D.velocity.x); }
            else { animator.SetFloat("VeloY", _rigidbody2D.velocity.y); }
            if (GravityManager.GravityArena) { 
                transform.localScale = movementInput.x switch {
                    < 0 => new Vector3(-1.25f, 1.25f, 1.25f),
                    > 0 => new Vector3(1.25f, 1.25f, 1.25f),
                    _ => transform.localScale
                };
            }
            else {
                transform.localScale = movementInput.x switch {
                    < 0 => new Vector3(-1, 1, 1),
                    > 0 => new Vector3(1, 1, 1),
                    _ => transform.localScale
                };
            }
            if (health <= 0) {
                deaths += 1;
                _trail.emitting = false;
                Respawn();
            }
            if (FindObjectOfType<WindManager>()) {
                upPointer.transform.parent = FindObjectOfType<WindManager>().transform;
                upPointer.transform.localPosition = new Vector3(transform.position.x, 7.65f,0);
            }
            else {
                upPointer.transform.position = new Vector3(transform.position.x, 7.65f,0);
            }
            if (FindObjectOfType<WindManager>()) {
                leftPointer.transform.parent = FindObjectOfType<WindManager>().transform;
                leftPointer.transform.localPosition = new Vector3(-14.25f, transform.position.y, 0);
                leftPointer.transform.rotation = Quaternion.Euler(0,0,90);
            }
            else {
                leftPointer.transform.position = new Vector3(-14.25f, transform.position.y, 0);
                leftPointer.transform.rotation = Quaternion.Euler(0,0,90);
            }
            if (FindObjectOfType<WindManager>()) {
                rightPointer.transform.parent = FindObjectOfType<WindManager>().transform;
                rightPointer.transform.localPosition = new Vector3(14.25f, transform.position.y, 0);
                rightPointer.transform.rotation = Quaternion.Euler(0,0,-90);
            }
            else {
                rightPointer.transform.position = new Vector3(14.25f, transform.position.y, 0);
                rightPointer.transform.rotation = Quaternion.Euler(0,0,-90);
            }
            if (_respawning) transform.position = respawnPoint.transform.position;
            
            if (listID == 1) {
                PlayerDataHandler.Instance.playerOneKills = kills;
                PlayerDataHandler.Instance.playerOneDeaths = deaths;
            }
            else if (listID == 2) {
                PlayerDataHandler.Instance.playerTwoKills = kills;
                PlayerDataHandler.Instance.playerTwoDeaths = deaths;
            }
            else if (listID == 3) {
                PlayerDataHandler.Instance.playerThreeKills = kills;
                PlayerDataHandler.Instance.playerThreeDeaths = deaths;
            }
            else if (listID == 4) {
                PlayerDataHandler.Instance.playerFourKills = kills;
                PlayerDataHandler.Instance.playerFourDeaths = deaths;
            }
        }

        private void FixedUpdate() {
            upPointer.SetActive(false);
            leftPointer.SetActive(false);
            rightPointer.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("BigAttack") && !_damageTake || other.CompareTag("FallingSword")) {
                Transform attackParent = other.transform.parent.parent;
                Vector2 expulsionDirection = (transform.position - attackParent.position).normalized;
                _rigidbody2D.AddForce(expulsionDirection * _bigAttackExplusionForce, ForceMode2D.Impulse);
                StartCoroutine(TakeBigDamage());
                _lastPlayerHitMe = other.GetComponentInParent<PlayerController>();
            }
            if (!other.CompareTag("Ground") || !groundCollider) return;
            animator.SetBool("Jump", false);
            animator.SetBool("Grounded", true);
            _isGrounded = true;
            _jumpCount = 0;
        }
        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("UpOutZone")) upPointer.SetActive(false);
            if (other.CompareTag("LeftOutZone")) leftPointer.SetActive(false);
            if (other.CompareTag("RightOutZone")) rightPointer.SetActive(false);
            if (!other.CompareTag("Ground") || !groundCollider) return;
            animator.SetBool("Grounded", false);
            _isGrounded = false;
        }
        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Attack") && !_damageTake || other.CompareTag("Arrow") && !_damageTake) {
                if (other.CompareTag("Arrow")) {
                    if (other.GetComponent<Arrow>().parentPlayer == this.gameObject) return;
                    Transform attackParent = other.transform;
                    Vector2 expulsionDirection = (transform.position - attackParent.position).normalized;
                    _rigidbody2D.AddForce(expulsionDirection * _attackExplusionForce, ForceMode2D.Impulse);
                    StartCoroutine(TakeDamage());
                    _lastPlayerHitMe = other.GetComponent<Arrow>().parentPlayer.GetComponent<Archer>();
                }
                else {
                    Transform attackParent = other.transform.parent.parent;
                    Vector2 expulsionDirection = (transform.position - attackParent.position).normalized;
                    _rigidbody2D.AddForce(expulsionDirection * _attackExplusionForce, ForceMode2D.Impulse);
                    StartCoroutine(TakeDamage());
                    _lastPlayerHitMe = other.GetComponentInParent<PlayerController>();
                }
            }
            if (other.CompareTag("Spell") && !_damageTake) {
                StartCoroutine(TakeSpellDamage());
                _lastPlayerHitMe = other.GetComponentInParent<Spell>().parentPlayer.GetComponent<Mage>();
            }
            if (other.CompareTag("UpOutZone")) upPointer.SetActive(true);
            if (other.CompareTag("LeftOutZone")) leftPointer.SetActive(true);
            if (other.CompareTag("RightOutZone")) rightPointer.SetActive(true);
            if (other.CompareTag("DeathZone")) health = 0;
            if (other.CompareTag("Ground")) _jumpCount = 0;
        }
        
        public void OnMove(InputAction.CallbackContext ctx) {
            if (playerInput) {
                movementInput = ctx.ReadValue<Vector2>();
                if (GravityManager.GravityUp) { movementInput.x = -movementInput.x; }
            }
        }
        public void OnJump(InputAction.CallbackContext ctx) {
            if (ctx.performed && playerInput) Jump();
        }
        public void OnDash(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && _canDash && playerInput) {
                switch (movementInput.x) {
                    case > 0 when dashPower < 0:
                    case < 0 when dashPower > 0:
                        dashPower = -dashPower;
                        break;
                }
                StartCoroutine(Dash());
            }
        }
        public void OnAttack(InputAction.CallbackContext ctx) {
            if (ctx.performed && _canAttack && playerInput) {
                _arrowDirection = movementInput.y;
                StartCoroutine(Attack());
            }
        }
        
        private void Respawn() {
            GameObject deathPart = Instantiate(DeathParticle, transform.position, quaternion.identity);
            deathPart.transform.parent = null;
            _rigidbody2D.velocity = Vector2.zero;
            transform.position = respawnPoint.transform.position;
            health = 150;
            if(_lastPlayerHitMe)_lastPlayerHitMe.kills += 1;
            StartCoroutine(RespawnStun());
            _lastPlayerHitMe = null;
        }
        //Couroutine Respawn
        private IEnumerator RespawnStun() {
            RespawnVFX.SetActive(true);
            _respawning = true;
            _damageTake = true;
            yield return new WaitForSeconds(_respawnTime);
            RespawnVFX.SetActive(false);
            _rigidbody2D.velocity = Vector2.zero;
            _respawning = false;
            _damageTake = false;
        }
        private void Jump() {
            if (_jumpCount > 1) return;
            animator.SetBool("Jump", true);
            Vector2 jumpVec = new Vector2(0, jumpPower);
            Vector2 jumpVecHoriz = new Vector2(jumpPower, 0);
            if (GravityManager.GravityLeft || GravityManager.GravityRight) _rigidbody2D.AddForce(transform.up * jumpVecHoriz, ForceMode2D.Impulse);
            else _rigidbody2D.AddForce(transform.up * jumpVec, ForceMode2D.Impulse);
            _jumpCount += 1;
        }
        
        //Couroutine Dash
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
            GameObject arrow = Instantiate(arrowPrefab, transform.position, quaternion.identity);
            Rigidbody2D arrowRigidbody2D = arrow.GetComponent<Rigidbody2D>();
            arrow.GetComponent<Arrow>().parentPlayer = this.gameObject;
            Transform arrowTransform = arrow.GetComponent<Transform>();
            if (transform.localScale.x < 0) arrowTransform.localScale = transform.localScale;
            float directionX = Mathf.Sign(transform.localScale.x);
            if (!GravityManager.GravityLeft && !GravityManager.GravityRight && !GravityManager.GravityUp) {
                if (_arrowDirection != 0 || _arrowDirection < -0.3f && _arrowDirection > 0) {
                    arrowRigidbody2D.velocity = new Vector2((directionX * 25f) / 2, _arrowDirection * 20f);
                    if (_arrowDirection > 0) {
                        arrowRigidbody2D.gravityScale = 1.15f;
                        animator.SetBool("AttackUp", true);
                    }
                    if (_arrowDirection < -0.3f) {
                        arrowRigidbody2D.gravityScale = 0.005f;
                        animator.SetBool("AttackDown", true);
                    }
                }
                else {
                    arrowRigidbody2D.velocity = new Vector2(directionX * 25f, 0);
                    animator.SetBool("Attack", true);
                }
            }
            else if (GravityManager.GravityUp)
            {
                if (_arrowDirection != 0 || _arrowDirection < -0.3f && _arrowDirection > 0) {
                    arrowRigidbody2D.velocity = new Vector2((-directionX * 25f) / 2, -_arrowDirection * 20f);
                    if (_arrowDirection > 0) {
                        arrowRigidbody2D.gravityScale = 1.15f;
                        animator.SetBool("AttackUp", true);
                    }
                    if (_arrowDirection < -0.3f) {
                        arrowRigidbody2D.gravityScale = 0.005f;
                        animator.SetBool("AttackDown", true);
                    }
                }
                else {
                    arrowRigidbody2D.velocity = new Vector2(-directionX * 25f, 0);
                    animator.SetBool("Attack", true);
                }
            }
            else if (GravityManager.GravityRight) {
                if (_arrowDirection != 0 || _arrowDirection < -0.3f && _arrowDirection > 0) {
                    arrowRigidbody2D.velocity = new Vector2(-_arrowDirection * 20f,(directionX * 25f)/ 2);
                    if (_arrowDirection > 0) {
                        arrowRigidbody2D.gravityScale = 1.25f;
                        animator.SetBool("AttackUp", true);
                    }
                    if (_arrowDirection < -0.3f) {
                        arrowRigidbody2D.gravityScale = 0.005f;
                        animator.SetBool("AttackDown", true);
                    }
                }
                else {
                    arrowRigidbody2D.velocity = new Vector2(0, (directionX * 25f));
                    animator.SetBool("Attack", true);
                }
            }
            else if (GravityManager.GravityLeft) {
                if (_arrowDirection != 0 || _arrowDirection < -0.3f && _arrowDirection > 0) {
                    arrowRigidbody2D.velocity = new Vector2(_arrowDirection * 20f,(directionX * 25f)/ 2);
                    if (_arrowDirection > 0) {
                        arrowRigidbody2D.gravityScale = 1.25f;
                        animator.SetBool("AttackUp", true);
                    }
                    if (_arrowDirection < -0.3f) {
                        arrowRigidbody2D.gravityScale = 0.005f;
                        animator.SetBool("AttackDown", true);
                    }
                }
                else {
                    arrowRigidbody2D.velocity = new Vector2(0, (-directionX * 25f));
                    animator.SetBool("Attack", true);
                }
            }
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
            animator.SetBool("AttackUp", false);
            animator.SetBool("AttackDown", false);
            animator.SetBool("Attack", false);
        }
        
        //Couroutine Damage
        private IEnumerator TakeDamage() {
            ImpactVFX.Play();
            _damageTake = true;
            health -= 10;
            animator.SetBool("Damage", true);
            speed = speed / 3;
            yield return new WaitForSeconds(_iFrame);
            speed = _baseSpeed;
            animator.SetBool("Damage", false);
            _damageTake = false;
        }
        private IEnumerator TakeSpellDamage()
        {
            ImpactVFX.Play();
            _damageTake = true;
            health -= 5;
            animator.SetBool("Damage", true);
            speed = speed / 3;
            yield return new WaitForSeconds(_iFrame);
            speed = _baseSpeed;
            animator.SetBool("Damage", false);
            _damageTake = false;
        }
        private IEnumerator TakeBigDamage()
        {
            ImpactVFX.Play();
            _damageTake = true;
            health -= 20;
            animator.SetBool("Damage", true);
            speed = speed / 3;
            yield return new WaitForSeconds(_iFrame);
            speed = _baseSpeed;
            animator.SetBool("Damage", false);
            _damageTake = false;
        }
        
        public void KillDone()
        {
            kills += 1;
        }
    }
}
