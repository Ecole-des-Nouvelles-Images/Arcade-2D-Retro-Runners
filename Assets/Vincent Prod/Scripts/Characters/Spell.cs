using System;
using UnityEngine;
using Vincent_Prod.Scripts.Arenas.Ice_Arena;

namespace Vincent_Prod.Scripts.Characters
{
    public class Spell : MonoBehaviour
    {
        private float Timer;
        private Rigidbody2D _rigidbody2D;
        public GameObject parentPlayer;
        
        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        private void Update() {
            Timer += Time.deltaTime;
            if (Timer >= 5) {
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ground")) {
                _rigidbody2D.mass = 0f;
                _rigidbody2D.gravityScale = 0f;
                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                transform.parent = other.transform;
            }
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Ground")) {
                if(other.GetComponentInParent<Ground>())transform.rotation = other.GetComponentInParent<Ground>().gameObject.transform.rotation;
                if (other.CompareTag("Ground")) {
                    _rigidbody2D.mass = 0f;
                    _rigidbody2D.gravityScale = 0f;
                    _rigidbody2D.velocity = Vector2.zero;
                    _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                    transform.parent = other.transform;
                }
            }
        }
    }
}
