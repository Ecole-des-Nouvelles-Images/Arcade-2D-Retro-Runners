using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Vincent_Prod.Scripts.RandomEvents {
    public class LightEvent : MonoBehaviour
    {
        public static bool lightsOff;
        public static void LightsOff() {
            if (lightsOff) return;
            Light2D[] allLights = FindObjectsOfType<Light2D>();
            foreach (Light2D light2D in allLights) if (light2D.gameObject.CompareTag("GlobalLight")) light2D.GetComponent<Animator>().SetBool("LightFlash", true);
            foreach (Light2D light2D in allLights) if (light2D.gameObject.CompareTag("GlobalLight")) light2D.GetComponent<Animator>().SetBool("LightsUp", false);
            foreach (Light2D light2D in allLights) if (light2D.gameObject.CompareTag("BonusLight")) light2D.enabled = false;
            lightsOff = true;
        }
        public static void LightsOn() {
            Light2D[] allLights = FindObjectsOfType<Light2D>();
            foreach (Light2D light2D in allLights) if (light2D.gameObject.CompareTag("GlobalLight")) light2D.GetComponent<Animator>().SetBool("LightsUp", true);
            foreach (Light2D light2D in allLights) if (light2D.gameObject.CompareTag("GlobalLight")) light2D.GetComponent<Animator>().SetBool("LightFlash", false);
            foreach (Light2D light2D in allLights) if (light2D.gameObject.CompareTag("BonusLight")) light2D.enabled = true;
            lightsOff = false;
        }
    }
}
