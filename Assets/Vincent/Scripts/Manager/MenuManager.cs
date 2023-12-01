using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace Vincent.Scripts.Manager
{
    public class MenuManager : MonoBehaviour
    {
        public List<PlayerCharacterSelection> playerCharacterSelection = new List<PlayerCharacterSelection>();
        public List<GameObject> CharacterSelection = new List<GameObject>();
        public List<Vector3> SpawnPoints = new List<Vector3>();
        public List<GameObject> InstantiatedCharacters = new List<GameObject>();
        
        public List<Gamepad> assignedGamepads = new List<Gamepad>();
        
        public void StartGame() {
            
            AssignGamepadsToPlayers();
            
            foreach (var playerCharacterSelection in playerCharacterSelection) {
                DontDestroyOnLoad(playerCharacterSelection.gameObject); 
            }
            DontDestroyOnLoad(this.gameObject);
            foreach (GameObject canvas in CharacterSelection) {
                canvas.SetActive(false);
            }
            SceneManager.LoadScene("Vincent");
        }
        
        private void AssignGamepadsToPlayers()
        {
            assignedGamepads.Clear();

            for (int i = 0; i < Gamepad.all.Count; i++)
            {
                assignedGamepads.Add(Gamepad.all[i]);
            }
        }
    }
}