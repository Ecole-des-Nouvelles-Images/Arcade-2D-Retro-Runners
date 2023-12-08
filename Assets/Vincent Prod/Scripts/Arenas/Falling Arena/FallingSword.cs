using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vincent_Prod.Scripts.Arenas.Falling_Arena
{
    public class FallingSword : MonoBehaviour
    {
        [SerializeField]private int _fallingSecond;
        private float _timer;
        public bool isFalling;
        
        private void Awake() {
            isFalling = false;
            _fallingSecond = 0;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        private void Start() {
            _fallingSecond = Random.Range(30, 181);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _fallingSecond) {
                GetComponentInChildren<Animator>().SetTrigger("Fall");
            }
        }

        private void Falling()
        {
            GetComponent<Rigidbody2D>().gravityScale = 0.45f;
            GetComponent<BoxCollider2D>().enabled = false;
            isFalling = true;
        }
        
        private void FixedUpdate() {
            if (isFalling) {
                Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                if (velocity.x < 0) {
                    angle += 180f;
                }
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Enter In Player : " + other.gameObject);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("DeathZone"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
