using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Vincent.Scripts.Archer;
using Vincent.Scripts.Knight;
using Vincent.Scripts.Player;

namespace Vincent.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {
        public PlayerCharacterSelection playerCharacterSelection;
        
        private void Start()
        {
            // Obtient le prefab du personnage sélectionné par le joueur
            GameObject selectedCharacterPrefab = playerCharacterSelection.GetSelectedCharacterPrefab();

            // Instancie le personnage dans la scène
            GameObject playerCharacter = Instantiate(selectedCharacterPrefab, Vector3.zero, Quaternion.identity);

            // Assigne le contrôle du joueur au personnage
            
            if (playerCharacter.GetComponent<KnightController>())
            {
                KnightController knightController = playerCharacter.GetComponent<KnightController>();
                knightController.playerInput = GetComponent<PlayerInput>();
            }
            if (playerCharacter.GetComponent<ArcherController>())
            {
                ArcherController archerController = playerCharacter.GetComponent<ArcherController>();
                archerController.playerInput = GetComponent<PlayerInput>();
            }
        }
    }
}