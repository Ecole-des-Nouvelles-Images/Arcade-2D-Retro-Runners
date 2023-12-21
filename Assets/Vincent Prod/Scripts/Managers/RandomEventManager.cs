using System;
using UnityEngine;
using Vincent_Prod.Scripts.RandomEvents;
using Random = UnityEngine.Random;

namespace Vincent_Prod.Scripts.Managers
{
    public class RandomEventManager : MonoBehaviour
    {
        private float _timeRemaining;
        private bool _lightChoosed;
        private bool _sizeChoosed;
        private bool _eventChoosed;
        private bool _startTimer;
        private float _Timer;
        public SizeEvent sizeEvent;
        
        private void Update()
        {
            _timeRemaining = gameObject.GetComponent<GameManager>().timeRemaining;
            switch (_timeRemaining) {
                case >= 119.85f and <= 120.15f:
                case >= 59.85f and <= 60.15f:
                    if (!_eventChoosed) Invoke("ChooseEvent",0f) ;
                    _eventChoosed = true;
                    _startTimer = true;
                    _Timer = 0f;
                    break;
            }
            switch (_timeRemaining) {
                case >= 89.85f and <= 90.15f:
                case >= 29.85f and <= 30.15f:
                    if (!_eventChoosed) Invoke("RestaureDefault",0f);
                    _eventChoosed = true;
                    _startTimer = true;
                    _Timer = 0f;
                    break;
            }
            if (_startTimer) { _Timer += Time.deltaTime; }
            if (_Timer >= 10) { _startTimer = false; _eventChoosed = false; }
        }

        private void ChooseEvent() {
            int eventValue = Random.Range(0, 6);
            if (eventValue == 0 || eventValue == 3 || eventValue == 4) { TurnLights(); _lightChoosed = true; }
            else if ((eventValue == 1 || eventValue == 3 || eventValue == 5)) { ChangeSize(); _sizeChoosed = true; }
        }

        private void RestaureDefault() {
            if (_lightChoosed) { TurnLights(); _lightChoosed = false; }
            else if (_sizeChoosed) { ChangeSize(); _sizeChoosed = false; }
        }

        [ContextMenu("Turn Lights")]
        public void TurnLights() {
            if (LightEvent.lightsOff)LightEvent.LightsOn(); 
            else LightEvent.LightsOff(); 
        }

        [ContextMenu("Change Size")]
        public void ChangeSize() {
            sizeEvent.ChangeSize(sizeEvent);
        }
    }
}
