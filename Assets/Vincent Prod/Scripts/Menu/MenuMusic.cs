using UnityEngine;

namespace Vincent_Prod.Scripts.Menu
{
    public class MenuMusic : MonoBehaviour
    {
        public AudioClip audioClip;
        private AudioSource _audioSource;
 
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _audioSource = gameObject.AddComponent<AudioSource>();
            //SoundSource.playOnAwake = false;
            //SoundSource.rolloffMode = AudioRolloffMode.Logarithmic;
            //SoundSource.loop = true;
        }
 
        public void Start()
        {
            _audioSource.clip = audioClip;
            _audioSource.Play(); 
        }
    }
}
