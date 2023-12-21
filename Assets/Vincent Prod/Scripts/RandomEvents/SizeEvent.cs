using System;
using System.Drawing;
using UnityEngine;
using Vincent_Prod.Scripts.Arenas.Gravity_Arena;
using Vincent_Prod.Scripts.Managers;
using Random = UnityEngine.Random;

namespace Vincent_Prod.Scripts.RandomEvents
{
    public class SizeEvent : MonoBehaviour
    {
        public static bool sizeChanged;
        private Vector3 _baseSize;
        private PlayerManager _playerManager;

        private void Start()
        {
            _playerManager = FindObjectOfType<PlayerManager>();
            if (GravityManager.GravityArena) _baseSize = new Vector3(1.25f, 1.25f, 1.25f);
            else _baseSize = new Vector3(1, 1, 1);
        }

        
        public void ChangeSize(SizeEvent sizeEvent) {
            if (sizeChanged) sizeEvent.ChangeDefaultSize();
            else {
                int upOrDown = Random.Range(0, 6);
                if (upOrDown == 0 || upOrDown == 3 || upOrDown == 4) { sizeEvent.ChangeSizeUp(); }
                else if (upOrDown == 1 || upOrDown == 3 || upOrDown == 5) { sizeEvent.ChangeSizeDown(); }
            }
        }

        private void ChangeDefaultSize() {
            foreach (GameObject player in _playerManager.Players) { player.transform.parent.localScale = _baseSize; }
            sizeChanged = false;
        }
        private void ChangeSizeUp() {
            foreach (GameObject player in _playerManager.Players) {
                player.transform.parent.localScale = _baseSize * 1.5f;
                player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);
            }
            sizeChanged = true;
        }

        private void ChangeSizeDown() {
            foreach (GameObject player in _playerManager.Players) {
                player.transform.parent.localScale = _baseSize * 0.5f; 
                player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);
            }
            
            sizeChanged = true;
        }
    }
}
