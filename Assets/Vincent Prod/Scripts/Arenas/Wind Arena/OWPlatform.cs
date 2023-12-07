using UnityEngine;
using Vincent_Prod.Scripts.Characters;

namespace Vincent_Prod.Scripts.Arenas.Wind_Arena
{
    public class OWPlatform : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;
            if (other.GetComponent<Knight>()) other.GetComponent<Rigidbody2D>().velocity = other.GetComponent<Rigidbody2D>().velocity / 5;
            if (other.GetComponent<Archer>()) other.GetComponent<Rigidbody2D>().velocity = other.GetComponent<Rigidbody2D>().velocity / 5;
            if (other.GetComponent<Paladin>()) other.GetComponent<Rigidbody2D>().velocity = other.GetComponent<Rigidbody2D>().velocity / 5;
            if (other.GetComponent<Mage>()) other.GetComponent<Rigidbody2D>().velocity = other.GetComponent<Rigidbody2D>().velocity / 5;
        }
    }
}
