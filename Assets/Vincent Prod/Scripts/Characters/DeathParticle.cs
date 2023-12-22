using UnityEngine;

namespace Vincent_Prod.Scripts.Characters
{
    public class DeathParticle : MonoBehaviour
    {
        private float _timer;

    
        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= 2.5f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
