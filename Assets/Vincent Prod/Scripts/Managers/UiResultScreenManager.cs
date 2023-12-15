using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vincent_Prod.Scripts.Characters;

namespace Vincent_Prod.Scripts.Managers
{
    public class UiResultScreenManager : MonoBehaviour
    {
        public List<int> playersKills = new List<int>();
        public GameObject resultPanelPrefab;
        public int lastKillAmount;
        public int RankNumber;
        public List<Sprite> portraits = new List<Sprite>();
        
        private void Start()
        { 
            RankNumber = 0;
           playersKills.Add(PlayerDataHandler.Instance.playerOneKills);
           playersKills.Add(PlayerDataHandler.Instance.playerTwoKills);
           playersKills.Add(PlayerDataHandler.Instance.playerThreeKills);
           playersKills.Add(PlayerDataHandler.Instance.playerFourKills);
           
           playersKills = playersKills.OrderBy(i => i).ToList();
           foreach (int killValue in playersKills) {
               if (killValue < 0) break;
               GameObject resultPanel = Instantiate(resultPanelPrefab, this.transform);
               UiResultPanel resultPanelScript = resultPanel.GetComponent<UiResultPanel>();
               resultPanelScript.rankNumber.text = new string((RankNumber + 1).ToString());
               ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerOneCharacter, 
                   PlayerDataHandler.Instance.playerOneKills, PlayerDataHandler.Instance.playerOneDeaths, Color.blue);
               ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerTwoCharacter, 
                   PlayerDataHandler.Instance.playerTwoKills, PlayerDataHandler.Instance.playerTwoDeaths, Color.red);
               ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerThreeCharacter, 
                   PlayerDataHandler.Instance.playerThreeCharacter, PlayerDataHandler.Instance.playerThreeDeaths, Color.green);
               ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerFourKills, 
                   PlayerDataHandler.Instance.playerFourKills, PlayerDataHandler.Instance.playerFourDeaths, Color.yellow);
               
               
               
               /*if (killValue == PlayerDataHandler.Instance.playerOneKills) {
                   resultPanelScript.portraitImage.sprite = portraits[PlayerDataHandler.Instance.playerOneCharacter] ?? resultPanelScript.portraitImage.sprite;
                   resultPanelScript.killsNumber.text = new string(PlayerDataHandler.Instance.playerOneKills.ToString());
                   resultPanelScript.deathNumber.text = new string(PlayerDataHandler.Instance.playerOneDeaths.ToString());
                   foreach (Image panel in resultPanelScript.panelImages) {
                      panel.color = Color.blue;
                   }
               }
               else if (killValue == PlayerDataHandler.Instance.playerTwoKills) {
                   resultPanelScript.portraitImage.sprite = portraits[PlayerDataHandler.Instance.playerOneCharacter] ?? resultPanelScript.portraitImage.sprite;
                   resultPanelScript.killsNumber.text = new string(PlayerDataHandler.Instance.playerTwoKills.ToString());
                   resultPanelScript.deathNumber.text = new string(PlayerDataHandler.Instance.playerTwoDeaths.ToString());
                   foreach (Image panel in resultPanelScript.panelImages) {
                       panel.color = Color.red;
                   }
               }
               else if (killValue == PlayerDataHandler.Instance.playerThreeKills) {
                   resultPanelScript.portraitImage.sprite = portraits[PlayerDataHandler.Instance.playerOneCharacter] ?? resultPanelScript.portraitImage.sprite;
                   resultPanelScript.killsNumber.text = new string(PlayerDataHandler.Instance.playerThreeKills.ToString());
                   resultPanelScript.deathNumber.text = new string(PlayerDataHandler.Instance.playerThreeDeaths.ToString());
                   foreach (Image panel in resultPanelScript.panelImages) {
                       panel.color = Color.green;
                   }
               }
               else if (killValue == PlayerDataHandler.Instance.playerFourKills) {
                   resultPanelScript.portraitImage.sprite = portraits[PlayerDataHandler.Instance.playerOneCharacter] ?? resultPanelScript.portraitImage.sprite;
                   resultPanelScript.killsNumber.text = new string(PlayerDataHandler.Instance.playerFourKills.ToString());
                   resultPanelScript.deathNumber.text = new string(PlayerDataHandler.Instance.playerFourDeaths.ToString());
                   foreach (Image panel in resultPanelScript.panelImages) {
                       panel.color = Color.yellow;
                   }
               }*/
           }
        }

        public void ApplyScorePanel(UiResultPanel resultPanelScript, int killValue, int playerIndex, int playerKills, int playerDeath, Color playerColor) {
            if (killValue == playerKills) {
                resultPanelScript.portraitImage.sprite = portraits[playerIndex] ?? resultPanelScript.portraitImage.sprite;
                resultPanelScript.killsNumber.text = new string(playerKills.ToString());
                resultPanelScript.deathNumber.text = new string(playerDeath.ToString());
                foreach (Image panel in resultPanelScript.panelImages) {
                    panel.color = playerColor;
                }
            }
        }
        
        public void ReturnToMenu() {
            SceneManager.LoadScene("Menu");
            DestroyImmediate(PlayerDataHandler.Instance.gameObject);
        }
    }
}
