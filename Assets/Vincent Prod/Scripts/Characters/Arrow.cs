using System;
using UnityEngine;

namespace Vincent_Prod.Scripts.Characters
{
    public class Arrow : MonoBehaviour {
        public GameObject parentPlayer;
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
