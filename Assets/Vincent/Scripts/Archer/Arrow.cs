using UnityEngine;

namespace Vincent.Scripts.Archer {
    public class Arrow : MonoBehaviour {
        public GameObject parentPlayer;
        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Player") || other.CompareTag("DeathZone")) {
                if (other.gameObject != parentPlayer) {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
