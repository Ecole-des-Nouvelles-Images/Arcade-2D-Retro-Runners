using UnityEngine;

namespace Vincent_Prod.Scripts.Arenas.Wind_Arena
{
    public class WindManager : MonoBehaviour
    {
        public ParticleSystem _rightWindParticles;
        public ParticleSystem _leftWindParticles;
    
        public void WindRight() {
            _rightWindParticles.Play();
            _leftWindParticles.Stop();
        }
        public void WindLeft() {
            _rightWindParticles.Stop();
            _leftWindParticles.Play();
        }
        public void StopAll() {
            _rightWindParticles.Stop();
            _leftWindParticles.Stop();
        }
    }
}
