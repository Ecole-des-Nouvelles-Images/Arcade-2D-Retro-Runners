using UnityEngine;
using UnityEngine.Audio;

namespace Vincent_Prod.Scripts.Menu
{
    public class MenuMusic : MonoBehaviour
    {
        public AudioClip audioClip;
        private AudioSource _audioSource;
        public AudioMixerGroup _master;  
 
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.GetComponent<AudioSource>().outputAudioMixerGroup = _master;
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
