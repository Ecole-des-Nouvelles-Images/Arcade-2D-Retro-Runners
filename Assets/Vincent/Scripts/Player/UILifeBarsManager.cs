using System.Collections.Generic;
using UnityEngine;
using Vincent.Scripts.Player;
using PlayerManager = Vincent_Prod.Scripts.PlayerManager;

public class UILifeBarsManager : MonoBehaviour
    {
        public List<UILifeBars> LifeBars = new List<UILifeBars>();
        public PlayerManager playerManager;
        public GameObject LifeBarPrefab;
        private int _lastPlayerCount;

        private void Update()
        {
            if (_lastPlayerCount < playerManager.Players.Count)
            {
                Instantiate(LifeBarPrefab, this.transform);
            }
            _lastPlayerCount = playerManager.Players.Count;
        }
    }

