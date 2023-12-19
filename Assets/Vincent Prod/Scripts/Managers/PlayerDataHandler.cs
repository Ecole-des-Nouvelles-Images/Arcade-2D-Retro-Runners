using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vincent_Prod.Scripts.Menu;

namespace Vincent_Prod.Scripts.Managers
{
    public class PlayerDataHandler : MonoBehaviourSingletonPersistent<PlayerDataHandler>
    {
        public List<GameObject> characterPrefabs = new List<GameObject>();
        public int playerOneCharacter;
        public int playerTwoCharacter;
        public int playerThreeCharacter;
        public int playerFourCharacter;
        
        public int playerOneKills;
        public int playerOneDeaths;
        public int playerTwoKills;
        public int playerTwoDeaths;
        public int playerThreeKills;
        public int playerThreeDeaths;
        public int playerFourKills;
        public int playerFourDeaths;
        
        private int _currentCharacterIndex;
        
        public void PlayerOneKnight() {
            playerOneCharacter = 0;
        }
        public void PlayerOneArcher() {
            playerOneCharacter = 1;
        }
        public void PlayerOnePaladin() {
            playerOneCharacter = 2;
        }
        public void PlayerOneMage() {
            playerOneCharacter = 3;
        }
        
        public void PlayerTwoKnight() {
            playerTwoCharacter = 0;
        }
        public void PlayerTwoArcher() {
            playerTwoCharacter = 1;
        }
        public void PlayerTwoPaladin() {
            playerTwoCharacter = 2;
        }
        public void PlayerTwoMage() {
            playerTwoCharacter = 3;
        }
        
        public void PlayerThreeKnight() {
            playerThreeCharacter = 0;
        }
        public void PlayerThreeArcher() {
            playerThreeCharacter = 1;
        }
        public void PlayerThreePaladin() {
            playerThreeCharacter = 2;
        }
        public void PlayerThreeMage() {
            playerThreeCharacter = 3;
        }
        
        public void PlayerFourKnight() {
            playerFourCharacter = 0;
        }
        public void PlayerFourArcher() {
            playerFourCharacter = 1;
        }
        public void PlayerFourPaladin() {
            playerFourCharacter = 2;
        }
        public void PlayerFourMage() {
            playerFourCharacter = 3;
        }
        
        public IEnumerator SpawnPlayers()
        {
            for(int i = 0; i < 4; i++) {
                if (i == 0) _currentCharacterIndex = playerOneCharacter;
                else if (i == 1) _currentCharacterIndex = playerTwoCharacter;
                else if (i == 2) _currentCharacterIndex = playerThreeCharacter;
                else if (i == 3) _currentCharacterIndex = playerFourCharacter;
                
                GameObject player = Instantiate(characterPrefabs[_currentCharacterIndex]);
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
