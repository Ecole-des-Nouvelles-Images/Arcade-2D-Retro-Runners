using System;
using UnityEngine;

namespace Vincent_Prod.Scripts.Arenas.Falling_Arena
{
    public class FallingPlanks : MonoBehaviour
    {
        [SerializeField] private int _playersStepsOnMe;
        private BoxCollider2D _playerDetector;

        private void Start() {
            _playersStepsOnMe = 0;
            _playerDetector = GetComponentInChildren<BoxCollider2D>();
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player") && _playerDetector) return;
            _playersStepsOnMe += 1;
            //Debug.Log("Players stepped on me : " + _playersStepsOnMe);
        }

        private void FixedUpdate() {
            if (_playersStepsOnMe == 30) GetComponent<Animator>().SetTrigger("Falling");
            
        }
    }
}
