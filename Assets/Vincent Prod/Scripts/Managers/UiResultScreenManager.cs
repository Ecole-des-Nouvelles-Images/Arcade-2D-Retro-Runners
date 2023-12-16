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
            RankNumber = 1;
           playersKills.Add(PlayerDataHandler.Instance.playerOneKills);
           playersKills.Add(PlayerDataHandler.Instance.playerTwoKills);
           playersKills.Add(PlayerDataHandler.Instance.playerThreeKills);
           playersKills.Add(PlayerDataHandler.Instance.playerFourKills);
           
           playersKills = playersKills.OrderByDescending(i => i).ToList();
           foreach (int killValue in playersKills) {
               if (killValue < 0) break;
               GameObject resultPanel = Instantiate(resultPanelPrefab, this.transform);
               UiResultPanel resultPanelScript = resultPanel.GetComponent<UiResultPanel>();
               if (killValue == PlayerDataHandler.Instance.playerOneKills) {
                   ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerOneCharacter,
                       PlayerDataHandler.Instance.playerOneKills, PlayerDataHandler.Instance.playerOneDeaths, Color.blue);
               }
               else if (killValue == PlayerDataHandler.Instance.playerTwoKills) {
                   ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerTwoCharacter, 
                       PlayerDataHandler.Instance.playerTwoKills, PlayerDataHandler.Instance.playerTwoDeaths, Color.red);
               }
               else if (killValue == PlayerDataHandler.Instance.playerThreeKills) {
                   ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerThreeCharacter, 
                       PlayerDataHandler.Instance.playerThreeCharacter, PlayerDataHandler.Instance.playerThreeDeaths, Color.green);
               }
               else if (killValue == PlayerDataHandler.Instance.playerFourKills) {
                   ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerFourKills, 
                       PlayerDataHandler.Instance.playerFourKills, PlayerDataHandler.Instance.playerFourDeaths, Color.yellow);
               }
               RankNumber += 1;
           }
        }

        public void ApplyScorePanel(UiResultPanel resultPanelScript, int killValue, int playerIndex, int playerKills, int playerDeath, Color playerColor)
        {
            resultPanelScript.rankNumber.text = new string(RankNumber.ToString());
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
