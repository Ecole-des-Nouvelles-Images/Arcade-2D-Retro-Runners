using UnityEngine;
using UnityEngine.InputSystem;

namespace Vincent.Scripts.Manager
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private InputAction southButtonAction;

        private void OnEnable()
        {
            // Configure l'action pour le bouton South
            southButtonAction = new InputAction(binding: "<Gamepad>/buttonSouth");
            southButtonAction.Enable();

            // Ajoute un callback pour détecter le bouton pressé
            southButtonAction.started += ctx => OnSouthButtonPressed();
        }

        private void OnDisable()
        {
            // Désactive l'action et retire le callback
            southButtonAction.Disable();
            southButtonAction.started -= ctx => OnSouthButtonPressed();
        }

        private void OnSouthButtonPressed()
        {
            // Code à exécuter lorsque le bouton South est pressé
            // Appelle la méthode pour activer le panneau
            ActivatePlayerPanel(0); // À remplacer par l'index approprié
        }

        private void ActivatePlayerPanel(int playerIndex)
        {
            // Met en œuvre la logique pour activer le panneau du joueur correspondant
            // Cela peut impliquer l'envoi d'un événement ou l'appel d'une méthode dans PlayerPanelManager
            PlayerPanelManager.Instance.ActivatePlayerPanel(playerIndex);
        }

    }
}