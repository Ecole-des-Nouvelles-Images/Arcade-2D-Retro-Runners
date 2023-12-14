using System.Collections;
using UnityEngine;
using Vincent_Prod.Scripts.Menu;
using Vincent.Scripts.Knight;

namespace Vincent_Prod.Scripts.Managers
{
    public class GameModeHandler : MonoBehaviour {
        void Start()
        {
            PlayerDataHandler.Instance.StartCoroutine("SpawnPlayers");
        }

        
    }
}
