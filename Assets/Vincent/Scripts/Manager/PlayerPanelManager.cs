using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vincent.Scripts.Manager
{
    public class PlayerPanelManager : MonoBehaviour
    {
        public GameObject[] playerPanels;
        public static PlayerPanelManager Instance { get; private set; }

        private void Awake() {
            DisableAllPlayerPanels();
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }
        private void Start() {
            InvokeRepeating(nameof(CheckConnectedGamepads), 1f, 1f);
        }
        private void OnDestroy() {
            CancelInvoke(nameof(CheckConnectedGamepads));
        }
        private void CheckConnectedGamepads() {
            for (int i = 0; i < Gamepad.all.Count; i++) {
                var gamepad = Gamepad.all[i];
                if (gamepad != null) {
                    ActivatePlayerPanel(i);
                }
            }
        }
        public void ActivatePlayerPanel(int playerIndex) {
            if (playerIndex < playerPanels.Length) {
                playerPanels[playerIndex].SetActive(true);
            }
        }
        private void DisableAllPlayerPanels() {
            foreach (var panel in playerPanels) {
                panel.SetActive(false);
            }
        }
    }
}
    
