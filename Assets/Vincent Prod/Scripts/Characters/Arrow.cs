using System;
using UnityEngine;

namespace Vincent_Prod.Scripts.Characters {
    public class Arrow : MonoBehaviour {
        public GameObject parentPlayer;
        public Vector3 parentScale;
        

        private void Start()
        {
            parentScale = parentPlayer.transform.localScale;
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
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}
