using System.Collections;
using UnityEngine;
using Vincent_Prod.Scripts.Menu;
using Vincent.Scripts.Knight;

namespace Vincent_Prod.Scripts.Managers
{
    public class GameModeHandler : MonoBehaviour
    {
        public GameObject knightPrefab;
        public GameObject archerPrefab;
        public GameObject paladinPrefab;
        public GameObject magePrefab;
        
        void Start()
        {
            StartCoroutine("SpawnPlayers");
        }

        IEnumerator SpawnPlayers()
        {
            for(int i = 0; i < 4; i++)
            {
                GameObject player = Instantiate(knightPrefab);
                player.transform.position = GameObject.Find("SpawnPoint" + (i + 1)).transform.position;
                player.transform.rotation = GameObject.Find("SpawnPoint" + (i + 1)).transform.rotation;

                if(MenuHelperFunctions.playersReady[i] == false)
                {
                    foreach(Transform trans in player.transform)
                    {
                        Destroy(trans.parent.gameObject);
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
