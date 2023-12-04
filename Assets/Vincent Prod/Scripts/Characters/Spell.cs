using UnityEngine;

namespace Vincent_Prod.Scripts.Characters
{
    public class Spell : MonoBehaviour
    {
        private float Timer;
        void Update() {
            Timer += Time.deltaTime;
            if (Timer >= 5) {
                Destroy(this.gameObject);
            }
        }
    }
}
