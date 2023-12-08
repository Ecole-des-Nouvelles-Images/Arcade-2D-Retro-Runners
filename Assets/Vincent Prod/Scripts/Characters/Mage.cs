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
        
        //Attack
        public GameObject spellPrefab;
        
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
            health = 140;
            deaths = 0;
            _canAttack = true;
            guardBox.SetActive(false);
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
                    transform.Translate(new Vector3(movementInput.x, 0, 0) * speed * Time.deltaTime) ;
                    break;
                case true:
                    _rigidbody2D.AddForce(new Vector2(movementInput.x,0)* rbSpeed * Time.deltaTime);
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
        }

        private void FixedUpdate() {
            upPointer.SetActive(false);
            leftPointer.SetActive(false);
            rightPointer.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("BigAttack") && !_damageTake || other.CompareTag("FallingSword")) {
                StartCoroutine(TakeBigDamage());
            }
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
                if (other.GetComponent<Spell>().parentPlayer == this.gameObject) return;
                StartCoroutine(TakeSpellDamage());
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
            StartCoroutine(RespawnStun());
        }
        //Couroutine Respawn
        private IEnumerator RespawnStun() {
            _respawning = true;
            _damageTake = true;
            yield return new WaitForSeconds(_respawnTime);
            _rigidbody2D.velocity = Vector2.zero;
            _respawning = false;
            _damageTake = false;
        }
        
        private void Jump() {
            if (_jumpCount > 1) return;
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
            yield return new WaitForSeconds(_guardTime);
            guardBox.SetActive(false);
            _isGuarding= false;
            yield return new WaitForSeconds(_guardCooldown);
            _canGuard = true;
        }
        
        //Coroutine Attack
        private IEnumerator Attack() {
            _canAttack = false;
            GameObject lastSpell = Instantiate(spellPrefab, transform.position, quaternion.identity);
            lastSpell.GetComponent<Spell>().parentPlayer = this.gameObject;
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
            Debug.Log("taking damade");
            _damageTake = true;
            health -= 2;
            yield return new WaitForSeconds(_iFrame);
            _damageTake = false;
        }
        private IEnumerator TakeBigDamage()
        {
            _damageTake = true;
            health -= 20;
            yield return new WaitForSeconds(_iFrame);
            _damageTake = false;
        }
    }
}
