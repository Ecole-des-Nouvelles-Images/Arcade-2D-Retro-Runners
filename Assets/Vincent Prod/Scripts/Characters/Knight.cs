using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Vincent_Prod.Scripts.Arenas.Gravity_Arena;
using Vincent_Prod.Scripts.Arenas.Wind_Arena;
using Vincent_Prod.Scripts.Managers;

namespace Vincent_Prod.Scripts.Characters
{
    public class Knight : PlayerController
    {
        //Dash
        private bool _canDash = true;
        private bool _isDashing;
        public float dashPower = 12;
        private float _dashingTime = 0.2f;
        private float _dashingCooldown = 1f;
        private TrailRenderer _trail;

        //Attack
        public GameObject attackBox;
        public GameObject upAttackBox;
        public GameObject downAttackBox;
        private bool _downAttack;

        //UI
        public Sprite portrait;

        //Visuel
        private SpriteRenderer _spriteRenderer;
        public Animator animator;
        
        
        //Manager
        private PlayerManager _playerManager;
        public int listID;

        private void Awake()
        {
            _baseSpeed = speed;
            RespawnVFX.SetActive(false);
            kills = 0;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _jumpCount = 2;
            _trail = GetComponent<TrailRenderer>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerManager = FindObjectOfType<PlayerManager>();
            attackBox.SetActive(false);
            upAttackBox.SetActive(false);
            downAttackBox.SetActive(false);
            health = 150;
            deaths = 0;
            _canAttack = true;
            _downAttack = false;
            _rigidbody2D.velocity = Vector2.zero;
            attackTime = 0.25f;
            attackCooldown = 0.05f;
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
            upPointer.SetActive(false);
            leftPointer.SetActive(false);
            rightPointer.SetActive(false);
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
            if (GravityManager.GravityUp) { animator.SetFloat("VeloY", _rigidbody2D.velocity.x); }
            else { animator.SetFloat("VeloY", _rigidbody2D.velocity.y); }
            if (movementInput.x != 0) animator.SetBool("Walk", true);
            else animator.SetBool("Walk", false);

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
            animator.SetBool("Grounded", true);
            animator.SetBool("Jump", false);
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
            if (other.CompareTag("Player") && _downAttack && downAttackBox.GetComponent<BoxCollider2D>()) {
                if (GravityManager.GravityLeft || GravityManager.GravityRight) _rigidbody2D.AddForce(transform.up * new Vector2(jumpPower / 7,0), ForceMode2D.Impulse);
                else _rigidbody2D.AddForce(transform.up * new Vector2(0,jumpPower / 7), ForceMode2D.Impulse);
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
        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && playerInput) Jump();
        }
        public void OnDash(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && _canDash && playerInput)
            {
                switch (movementInput.x)
                {
                    case > 0 when dashPower < 0:
                    case < 0 when dashPower > 0:
                        dashPower = -dashPower;
                        break;
                }

                StartCoroutine(Dash());
            }
        }
        public void OnAttack(InputAction.CallbackContext ctx)
        {
            switch (ctx.performed)
            {
                case true when _canAttack && movementInput.y > 0.5f && playerInput:
                    StartCoroutine(AttackUp());
                    break;
                case true when _canAttack && movementInput.y < -0.5f && !_isGrounded && playerInput:
                    StartCoroutine(AttackDown());
                    break;
                case true when _canAttack && playerInput:
                    StartCoroutine(Attack());
                    break;
            }
        }
        
        
        private void Respawn()
        {
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
        private IEnumerator RespawnStun()
        {
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
            animator.SetBool("Jump", false);
            Vector2 jumpVec = new Vector2(0, jumpPower);
            Vector2 jumpVecHoriz = new Vector2(jumpPower, 0);
            animator.SetBool("Jump", true);
            if (GravityManager.GravityLeft || GravityManager.GravityRight) _rigidbody2D.AddForce(transform.up * jumpVecHoriz, ForceMode2D.Impulse);
            else _rigidbody2D.AddForce(transform.up * jumpVec, ForceMode2D.Impulse);
            _jumpCount += 1;
        }
        
        //Couroutine Dash
        private IEnumerator Dash() {
            _canDash = false;
            _isDashing = true;
            animator.SetBool("Dash", true);
            float originalGravity = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.velocity = new Vector2(dashPower, _rigidbody2D.velocity.y);
            _trail.emitting = true;
            yield return new WaitForSeconds(_dashingTime);
            animator.SetBool("Dash", false);
            _rigidbody2D.gravityScale = originalGravity;
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            _trail.emitting = false;
            _isDashing = false;
            yield return new WaitForSeconds(_dashingCooldown);
            _canDash = true;
        }
        
        //Couroutines Attack
        private IEnumerator Attack() {
            _canAttack = false;
            animator.SetBool("Attack", true);
            attackBox.SetActive(true);
            yield return new WaitForSeconds(attackTime);
            attackBox.SetActive(false);
            animator.SetBool("Attack", false);
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }
        private IEnumerator AttackUp() {
            _canAttack = false;
            animator.SetBool("AttackUp", true);
            upAttackBox.SetActive(true);
            yield return new WaitForSeconds(attackTime);
            upAttackBox.SetActive(false);
            animator.SetBool("AttackUp", false);
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }
        private IEnumerator AttackDown() {
            _canAttack = false;
            _downAttack = true;
            animator.SetBool("AttackDown", true);
            downAttackBox.SetActive(true);
            yield return new WaitForSeconds(attackTime);
            downAttackBox.SetActive(false);
            animator.SetBool("AttackDown", false);
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
            _downAttack = false;
        }
        
        //Couroutine Damage
        private IEnumerator TakeDamage() {
            ImpactVFX.Play();
            _damageTake = true;
            animator.SetBool("Damage",true);
            health -= 10;
            speed = speed / 3;
            yield return new WaitForSeconds(_iFrame);
            speed = _baseSpeed;
            _damageTake = false;
            animator.SetBool("Damage",false);
        }
        private IEnumerator TakeSpellDamage()
        {
            ImpactVFX.Play();
            _damageTake = true;
            animator.SetBool("Damage",true);
            health -= 5;
            speed = speed / 3;
            yield return new WaitForSeconds(_iFrame);
            speed = _baseSpeed;
            _damageTake = false;
            animator.SetBool("Damage",false);
        }
        private IEnumerator TakeBigDamage()
        {
            ImpactVFX.Play();
            _damageTake = true;
            animator.SetBool("Damage", true);
            health -= 20;
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
