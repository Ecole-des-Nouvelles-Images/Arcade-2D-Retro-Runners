using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vincent_Prod.Scripts.Arenas.Gravity_Arena;
using Vincent_Prod.Scripts.Arenas.Wind_Arena;
using Vincent_Prod.Scripts.Managers;

namespace Vincent_Prod.Scripts.Characters
{
    public class Mage : PlayerController
    {
        //Guard
        private bool _canGuard = true;
        private bool _isGuarding;
        private float _guardTime = 5f;
        private float _guardCooldown = 8f;
        public GameObject guardBox;
        
        //Movement
        private bool _isFloating;
        public ParticleSystem hoverParticles;
        
        //Attack
        public GameObject spellPrefab;
        
        //UI
        public Sprite portrait;

        //Visuel
        private SpriteRenderer _spriteRenderer;
        public List<ParticleSystem> shieldParticles = new List<ParticleSystem>();
        public Animator shieldAnimator;
        public Animator animator;
        
        //Manager
        private PlayerManager _playerManager;
        public int listID;

        private void Awake()
        {
            RespawnVFX.SetActive(false);
            kills = 0;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = 1;
            _jumpCount = 2;
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _playerManager = FindObjectOfType<PlayerManager>();
            health = 140;
            deaths = 0;
            _canAttack = true;
            _isFloating = false;
            hoverParticles.Stop();
            guardBox.SetActive(false);
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
            upPointer.SetActive(false);
            leftPointer.SetActive(false);
            rightPointer.SetActive(false);
            _rigidbody2D.gravityScale = 1;
            _isFloating = false;
            hoverParticles.Stop();
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
            if(movementInput.x != 0) animator.SetBool("Walk", true);
            else animator.SetBool("Walk", false);
            animator.SetFloat("VeloY", _rigidbody2D.velocity.y);
            if (health <= 0) {
                deaths += 1;
                _rigidbody2D.gravityScale = 1;
                _isFloating = false;
                hoverParticles.Stop();
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
            animator.SetBool("Hover", false);
            _isGrounded = true;
            _jumpCount = 0;
            _isFloating = false;
            hoverParticles.Stop();
            _rigidbody2D.gravityScale = 1;
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
                if (other.GetComponent<Spell>().parentPlayer == this.gameObject) return;
                StartCoroutine(TakeSpellDamage());
                _lastPlayerHitMe = other.GetComponentInParent<PlayerController>();
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
        public void OnGuard(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && _canGuard && playerInput) {
                StartCoroutine(Guard());
            }
        }
        public void OnAttack(InputAction.CallbackContext ctx) {
            if (ctx.performed && _canAttack && playerInput) {
                StartCoroutine(Attack());
            }
        }
        
        private void Respawn()
        {
            _rigidbody2D.velocity = Vector2.zero;
            transform.position = respawnPoint.transform.position;
            health = 140;
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
            switch (_jumpCount)
            {
                case > 1 when !_isFloating:
                    _rigidbody2D.velocity = Vector2.zero;
                    _rigidbody2D.gravityScale = 0.15f;
                    _isFloating = true;
                    hoverParticles.Play();
                    animator.SetBool("Hover", true);
                    break;
                case > 1 when _isFloating:
                    _rigidbody2D.velocity = Vector2.zero;
                    _rigidbody2D.gravityScale = 1;
                    _isFloating = false;
                    hoverParticles.Stop();
                    animator.SetBool("Hover", false);
                    break;
            }
            if (_jumpCount > 1) return;
            animator.SetBool("Jump", true);
            Vector2 jumpVec = new Vector2(0, jumpPower);
            Vector2 jumpVecHoriz = new Vector2(jumpPower, 0);
            if (GravityManager.GravityLeft || GravityManager.GravityRight) _rigidbody2D.AddForce(transform.up * jumpVecHoriz, ForceMode2D.Impulse);
            else _rigidbody2D.AddForce(transform.up * jumpVec, ForceMode2D.Impulse);
            _jumpCount += 1;
        }
        
        //Couroutine Dash
        private IEnumerator Guard() {
            _canGuard = false;
            _isGuarding = true;
            guardBox.SetActive(true);
            animator.SetTrigger("Shield");
            shieldAnimator.SetTrigger("Visible");
            foreach (var particle in shieldParticles) { particle.Play(); }
            yield return new WaitForSeconds(_guardTime);
            shieldAnimator.SetTrigger("Invisible");
            foreach (var particle in shieldParticles) { particle.Stop(); }
            yield return new WaitForSeconds(0.35f);
            guardBox.SetActive(false);
            _isGuarding= false;
            yield return new WaitForSeconds(_guardCooldown);
            _canGuard = true;
        }
        
        //Coroutine Attack
        private IEnumerator Attack() {
            _canAttack = false;
            animator.SetTrigger("Spell");
            GameObject lastSpell = Instantiate(spellPrefab, transform.position, quaternion.identity);
            lastSpell.GetComponent<Spell>().parentPlayer = this.gameObject;
            yield return new WaitForSeconds(attackCooldown);
            _canAttack = true;
        }
        
        //Couroutine Damage
        private IEnumerator TakeDamage() {
            ImpactVFX.Play();
            animator.SetBool("Damage", true); 
            _damageTake = true;
            if (_isGuarding) health -= 5; 
            else health -= 10;
            _rigidbody2D.gravityScale = 1;
            hoverParticles.Stop();
            _isFloating = false;
            yield return new WaitForSeconds(_iFrame);
            animator.SetBool("Damage", false); 
            _damageTake = false;
            
        }
        private IEnumerator TakeSpellDamage()
        { 
            ImpactVFX.Play();
            animator.SetBool("Damage", true); 
            _damageTake = true;
            if (_isGuarding) health -= 1; 
            else health -= 5;
            yield return new WaitForSeconds(_iFrame);
            animator.SetBool("Damage", false); 
            _damageTake = false;
        }
        private IEnumerator TakeBigDamage()
        {
            ImpactVFX.Play();
            animator.SetBool("Damage", true); 
            _damageTake = true;
            if (_isGuarding) health -= 10; 
            else health -= 20;
            yield return new WaitForSeconds(_iFrame);
            animator.SetBool("Damage", false); 
            _damageTake = false;
        }
        
        public void KillDone()
        {
            kills += 1;
        }
    }
}
