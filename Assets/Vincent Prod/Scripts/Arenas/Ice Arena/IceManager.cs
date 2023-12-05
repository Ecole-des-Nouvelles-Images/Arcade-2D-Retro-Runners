using System.Collections.Generic;
using UnityEngine;
using Vincent_Prod.Scripts.Characters;

namespace Vincent_Prod.Scripts.Arenas.Ice_Arena
{
    public class IceManager : MonoBehaviour
    {
        private List<PlayerController> Players = new List<PlayerController>();
        [ContextMenu("Change Ice")]
        public void ChangeIce()
        {
            Players.Add(FindObjectOfType<PlayerController>());
            foreach (var player in Players)
            {
                player.iceArena = !player.iceArena;
            }
        }
    }
}
