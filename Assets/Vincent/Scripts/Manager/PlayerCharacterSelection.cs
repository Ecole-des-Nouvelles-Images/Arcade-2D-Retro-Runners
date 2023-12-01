using UnityEngine;
using UnityEngine.InputSystem;

namespace Vincent.Scripts.Manager
{
    public class PlayerCharacterSelection : MonoBehaviour
    {
        public int selectedCharacterIndex;
        public GameObject[] characterPrefabs;
        public PlayerInput playerInput;

        private void Start()
        {
            selectedCharacterIndex = 4;
        }

        public void CharacterKnightSelected() {
            selectedCharacterIndex = 0;
            Debug.Log("Knight selected for : " + this.gameObject);
        }
        public void CharacterArcherSelected() {
            selectedCharacterIndex = 1;
            Debug.Log("Archers elected for : " + this.gameObject);
        }
        public void CharacterPaladinSelected() {
            selectedCharacterIndex = 2;
            Debug.Log("Paladin selected for : " + this.gameObject);
        }
        public void CharacterMageSelected() {
            selectedCharacterIndex = 3;
            Debug.Log("Mage selected for : " + this.gameObject);
        }
        
        public GameObject GetSelectedCharacterPrefab()
        {
            return characterPrefabs[selectedCharacterIndex];
        }
    }
}