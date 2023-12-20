using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Vincent_Prod.Scripts.RandomEvents {
    public class LightEvent : MonoBehaviour
    {
        public static bool lightsOff;
        public static void LightsOff() {
            Light2D[] allLights = FindObjectsOfType<Light2D>();
            foreach (Light2D light2D in allLights) if (light2D.gameObject.CompareTag("GlobalLight")) light2D.intensity = 0.05f;
            lightsOff = true;
        }
        public static void LightsOn() {
            Light2D[] allLights = FindObjectsOfType<Light2D>();
            foreach (Light2D light2D in allLights) if (light2D.gameObject.CompareTag("GlobalLight")) light2D.intensity = 0.7f;
            lightsOff = false;
        }
    }
}
