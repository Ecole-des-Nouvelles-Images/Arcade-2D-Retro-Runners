using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
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
        public ParticleSystem _jumpParticle;
        
        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _jumpCount = 2;
            _trail = GetComponent<TrailRenderer>();
        }

        private void Update() {
            transform.Translate(new Vector3(_movementInput.x, 0, 0) * speed * Time.deltaTime) ;
        }

        public void OnMove(InputAction.CallbackContext ctx) => _movementInput = ctx.ReadValue<Vector2>();
            // Left Joystick
        public void OnJump(InputAction.CallbackContext ctx) {
            if (ctx.performed) Jump();
        }
        // A button / X button
        public void OnDash(InputAction.CallbackContext ctx) {
            if (ctx.performed && _canDash) {
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
        // Right trigger (RT, R2)

        private void Jump() {
            if (_jumpCount > 1) return;
            Vector2 jumpVec = new Vector2(0, jumpForce);
            _rigidbody2D.AddForce(jumpVec * Time.deltaTime, ForceMode2D.Impulse);
            _jumpCount += 1;
            _jumpParticle.Play();
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Ground") || !_groundCollider) return;
            _isGrounded = true;
            _jumpCount = 0;
        }
        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("Ground") || !_groundCollider) return;
            _isGrounded = false;
        }
        
        //Coroutine Dash
        private IEnumerator Dash() {
            _canDash = false;
            _isDashing = true;
            float originalGravity = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.velocity = new Vector2(transform.localScale.x * dashPower, _rigidbody2D.velocity.y);
            _trail.emitting = true;
            yield return new WaitForSeconds(_dashingTime);
            _rigidbody2D.gravityScale = originalGravity;
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            _trail.emitting = false;
            _isDashing = false;
            yield return new WaitForSeconds(_dashingCooldown);
            _canDash = true;
        }
    }
    
    
}
