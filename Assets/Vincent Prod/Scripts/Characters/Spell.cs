using System;
using Unity.Mathematics;
using UnityEngine;
using Vincent_Prod.Scripts.Arenas.Gravity_Arena;
using Vincent_Prod.Scripts.Arenas.Ice_Arena;

namespace Vincent_Prod.Scripts.Characters
{
    public class Spell : MonoBehaviour
    {
        private float Timer;
        private Rigidbody2D _rigidbody2D;
        public GameObject parentPlayer;
        public GameObject particlesPrefab;
        private GameObject _myParticles;
        
        private void Awake() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start() {
            transform.rotation = parentPlayer.transform.rotation;
            _myParticles = Instantiate(particlesPrefab, transform.position, quaternion.identity);
            _myParticles.transform.parent = null;
        }

        private void Update() {
            Timer += Time.deltaTime;
            _myParticles.transform.position = transform.position;
            _myParticles.transform.rotation = transform.rotation;
            if (Timer >= 3) {
                foreach (var particle in _myParticles.GetComponent<SpellParticles>().particleSystems) {
                    particle.Stop();
                }
            }
            if (Timer >= 5) {
                Destroy(_myParticles);
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
