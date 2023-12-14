using UnityEngine;

public class SpriteSwapperPaladin : MonoBehaviour {
    public GameObject frontShield;
    public GameObject sideShield;

    public void EnableFrontShield() {
        frontShield.SetActive(true);
        sideShield.SetActive(false);
    }
    public void EnableSideShield() {
        frontShield.SetActive(false);
        sideShield.SetActive(true);
    }
}
