using System;
using UnityEngine;
using Vincent_Prod.Scripts.Arenas.Gravity_Arena;

namespace Vincent_Prod.Scripts.Characters {
    public class Arrow : MonoBehaviour {
        public GameObject parentPlayer;
        private Rigidbody2D _rigidbody2D;
        public SpriteRenderer arrowSprite;
        public Vector3 parentScale;
        

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            parentScale = parentPlayer.transform.localScale;
            if (GravityManager.GravityUp || GravityManager.GravityLeft) {
                arrowSprite.flipX = true;
            }
            else {
                arrowSprite.flipX = false;
            }
        }

        private void FixedUpdate() {
            Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            if (velocity.x < 0) {
                angle += 180f;
            }
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Player") || other.CompareTag("DeathZone") || other.CompareTag("Ground")) {
                if (other.gameObject != parentPlayer) {
                    Destroy(this.gameObject);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Shield")) {
                _rigidbody2D.velocity = Vector2.zero;
            }
            if (other.CompareTag("MagicShield"))
            {
                transform.localScale = new Vector3(-transform.localScale.x, -transform.localScale.y, 1);
                _rigidbody2D.velocity = new Vector2(-_rigidbody2D.velocity.x / 1.5f, -_rigidbody2D.velocity.y / 1.5f);
            }
        }
    }
}
