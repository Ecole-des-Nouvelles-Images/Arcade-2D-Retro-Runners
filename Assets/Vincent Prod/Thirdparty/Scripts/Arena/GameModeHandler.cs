using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vincent_Prod.Scripts.Menu;

public class GameModeHandler : MonoBehaviour
{
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnPlayers");
    }

    IEnumerator SpawnPlayers()
    {
        for(int i = 0; i < 4; i++)
        {
            GameObject player = Instantiate(playerPrefab) as GameObject;
            player.transform.position = GameObject.Find("Spawnpoint " + (i + 1).ToString()).transform.position;
            player.transform.rotation = GameObject.Find("Spawnpoint " + (i + 1).ToString()).transform.rotation;

            if(MenuHelperFunctions.playersReady[i] == false)
            {
                foreach(Transform transform in player.transform)
                {
                    Destroy(transform.gameObject);
                    player.GetComponent<PlayerHandler>().enabled = false;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
