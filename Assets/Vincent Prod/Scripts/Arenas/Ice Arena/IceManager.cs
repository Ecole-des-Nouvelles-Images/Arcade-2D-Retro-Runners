using System;
using System.Collections.Generic;
using UnityEngine;
using Vincent_Prod.Scripts.Characters;
using Vincent_Prod.Scripts.Managers;

namespace Vincent_Prod.Scripts.Arenas.Ice_Arena
{
    public class IceManager : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private List<PlayerController> Players = new List<PlayerController>();
        [ContextMenu("Change Ice")]
        public void ChangeIce() {
            Players.Clear();
            Players.Add(FindObjectOfType<PlayerController>());
            foreach (var player in Players) {
                player.iceArena = !player.iceArena;
            }
        }

        private void Start() {
            _playerManager = FindObjectOfType<PlayerManager>();
        }

        private void FixedUpdate() {
            foreach (GameObject player in _playerManager.Players) {
                if (player.GetComponent<Knight>() && player.GetComponent<Knight>().iceArena != true) player.GetComponent<Knight>().iceArena = true;
                if (player.GetComponent<Archer>() && player.GetComponent<Archer>().iceArena != true) player.GetComponent<Archer>().iceArena = true;
                if (player.GetComponent<Paladin>() && player.GetComponent<Paladin>().iceArena != true) player.GetComponent<Paladin>().iceArena = true;
                if (player.GetComponent<Mage>() && player.GetComponent<Mage>().iceArena != true) player.GetComponent<Mage>().iceArena = true;
            }
        }
    }
}
