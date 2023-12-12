using UnityEngine;
using Vincent_Prod.Scripts.RandomEvents;

namespace Vincent_Prod.Scripts.Managers
{
    public class RandomEventManager : MonoBehaviour {
        [ContextMenu("Turn Lights")]
        public void TurnLights() {
            if (LightEvent.lightsOff)LightEvent.LightsOn(); 
            else LightEvent.LightsOff(); 
        }
    }
}
