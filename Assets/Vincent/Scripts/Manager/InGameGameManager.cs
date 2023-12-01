using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Vincent.Scripts.Manager
{
    public class InGameGameManager : MonoBehaviour
    {
        public PlayerInputManager playerInputManager;
        void Start()
        {
            Destroy(FindObjectOfType<PlayerPanelManager>());

            /*foreach (PlayerCharacterSelection playerCharacterSelected in FindObjectOfType<MenuManager>().playerCharacterSelection)
            {
                GameObject selectedCharacterPrefab = playerCharacterSelected.GetSelectedCharacterPrefab();
                GameObject playerCharacter = Instantiate(selectedCharacterPrefab, new Vector3(0,5,0), quaternion.identity);
                FindObjectOfType<MenuManager>().InstantiatedCharacters.Add(playerCharacter);
                Debug.Log(playerCharacter);
            }*/
            MenuManager menuManager = FindObjectOfType<MenuManager>();

            for (int i = 0; i < menuManager.InstantiatedCharacters.Count; i++)
            {
                GameObject playerCharacter = menuManager.InstantiatedCharacters[i];

                // Assigne la manette au joueur
                PlayerInput playerInput = playerCharacter.GetComponent<PlayerInput>();
                if (playerInput != null && i < menuManager.assignedGamepads.Count)
                {
                    // Obtient la manette
                    Gamepad gamepad = menuManager.assignedGamepads[i];

                    // Obtient l'utilisateur InputUser associé au PlayerInput
                    InputUser playerInputUser = playerInput.user;

                    // Associe la manette à l'utilisateur
                    InputUser.PerformPairingWithDevice(gamepad, playerInputUser);
                }
            }
        }
    }
}
