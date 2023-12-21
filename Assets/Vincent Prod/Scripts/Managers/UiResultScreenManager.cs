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
           if ( PlayerDataHandler.Instance.playerOneKills != -1) playersKills.Add(PlayerDataHandler.Instance.playerOneKills);
           Debug.Log(playersKills.Count);
           if ( PlayerDataHandler.Instance.playerTwoKills != -1) playersKills.Add(PlayerDataHandler.Instance.playerTwoKills);
           Debug.Log(playersKills.Count);
           if ( PlayerDataHandler.Instance.playerThreeKills != -1) playersKills.Add(PlayerDataHandler.Instance.playerThreeKills);
           Debug.Log(playersKills.Count);
           if ( PlayerDataHandler.Instance.playerFourKills != -1) playersKills.Add(PlayerDataHandler.Instance.playerFourKills);
           Debug.Log(playersKills.Count);
           Debug.Log((playersKills));
           playersKills = playersKills.OrderByDescending(i => i).ToList();
           Debug.Log((playersKills));
           
           foreach (int killValue in playersKills) {
               if (killValue < 0) break;
               GameObject resultPanel = Instantiate(resultPanelPrefab, this.transform);
               UiResultPanel resultPanelScript = resultPanel.GetComponent<UiResultPanel>();
               if (killValue == PlayerDataHandler.Instance.playerOneKills) {
                   Debug.Log(killValue);
                   ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerOneCharacter,
                       PlayerDataHandler.Instance.playerOneKills, PlayerDataHandler.Instance.playerOneDeaths, 
                       Color.blue, PlayerDataHandler.Instance.playerOnePortrait);
                   resultPanelScript.killsNumber.text = PlayerDataHandler.Instance.playerOneKills.ToString();
               }
               else if (killValue == PlayerDataHandler.Instance.playerTwoKills) {
                   Debug.Log(killValue);
                   ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerTwoCharacter, 
                       PlayerDataHandler.Instance.playerTwoKills, PlayerDataHandler.Instance.playerTwoDeaths, 
                       Color.red, PlayerDataHandler.Instance.playerTwoPortrait);
                   resultPanelScript.killsNumber.text = PlayerDataHandler.Instance.playerTwoKills.ToString();
               }
               else if (killValue == PlayerDataHandler.Instance.playerThreeKills) {
                   Debug.Log(killValue);
                   ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerThreeCharacter, 
                       PlayerDataHandler.Instance.playerThreeCharacter, PlayerDataHandler.Instance.playerThreeDeaths, 
                       Color.green, PlayerDataHandler.Instance.playerThreePortrait);
                   resultPanelScript.killsNumber.text = PlayerDataHandler.Instance.playerThreeKills.ToString();
               }
               else if (killValue == PlayerDataHandler.Instance.playerFourKills) {
                   Debug.Log(killValue);
                   ApplyScorePanel(resultPanelScript, killValue, PlayerDataHandler.Instance.playerFourCharacter, 
                       PlayerDataHandler.Instance.playerFourKills, PlayerDataHandler.Instance.playerFourDeaths,
                       Color.yellow, PlayerDataHandler.Instance.playerFourPortrait);
                   resultPanelScript.killsNumber.text = PlayerDataHandler.Instance.playerFourKills.ToString();
               }
               RankNumber += 1;
           }
        }

        private void ApplyScorePanel(UiResultPanel resultPanelScript, int killValue, int playerIndex, int playerKills, int playerDeath, Color playerColor, Sprite portrait)
        {
            resultPanelScript.rankNumber.text = RankNumber.ToString();
            resultPanelScript.portraitImage.sprite = portrait;
            resultPanelScript.killsNumber.text = playerKills.ToString();
            resultPanelScript.deathNumber.text = playerDeath.ToString();
            foreach (Image panel in resultPanelScript.panelImages) { 
                panel.color = playerColor;
            }
        }
        
        public void ReturnToMenu() {
            SceneManager.LoadScene("Menu");
            DestroyImmediate(PlayerDataHandler.Instance.gameObject);
        }
    }
}
