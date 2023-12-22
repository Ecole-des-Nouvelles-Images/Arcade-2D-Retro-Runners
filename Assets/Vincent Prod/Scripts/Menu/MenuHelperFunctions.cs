using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vincent_Prod.Scripts.Managers;

namespace Vincent_Prod.Scripts.Menu
{
    public class MenuHelperFunctions : MonoBehaviour
    {
        public static bool[] playersReady = new bool[4]{ false, false, false, false };
        public static bool[] playersJoined = new bool[4] { false, false, false, false };

        public List<Scene> Arenas = new List<Scene>();

        public UnityEvent OnOpenMenu;
        


        //lobby variables
        public int playerIndex = 1;
        public GameObject playerPanel;

        private void Start()
        {
            OnOpenMenu.Invoke();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void PlayerJoined()
        {
            GameObject.Find("CountdownTimer").GetComponent<Text>().text = "";
            foreach (MenuHelperFunctions menuHelper in GameObject.FindObjectsOfType<MenuHelperFunctions>())
            {
                menuHelper.StopCoroutine("LobbyCountdown");
            }
            playersJoined[playerIndex - 1] = true;
        }

        public void PlayerReady()
        {
            playersReady[playerIndex - 1] = true;

            bool allPlayersReady = true;
            int readyCount = 0;
            for(int i = 0; i < playersJoined.Length; i++)
            {
                if(playersJoined[i] == true)
                {
                    if (playersReady[i] == false)
                    {
                        allPlayersReady = false;
                    }
                    else
                    {
                        readyCount++;
                    }
                }
            }

            if(allPlayersReady == true && readyCount > 1)
            {
                StartCoroutine("LobbyCountdown");
            }
        }


        IEnumerator LobbyCountdown()
        {
            int countdownDuration = 5;

            int remainingTime = countdownDuration;
            for(int i = 0; i < countdownDuration; i++)
            {
                GameObject.Find("CountdownTimer").GetComponent<Text>().text = "Start\n" + remainingTime.ToString();
                yield return new WaitForSeconds(1);
                remainingTime--;
            }

            int _randomArenaChoice = Random.Range(0, 3);
            if (_randomArenaChoice == 0) LoadScene("Gravity Arena");
            else if (_randomArenaChoice == 2) LoadScene("Falling Arena");
            else if (_randomArenaChoice == 1) LoadScene("Ice Arena");
            Destroy(FindObjectOfType<MenuMusic>().gameObject);
            //else if (_randomArenaChoice == 3) LoadScene("Wind Arena");
        }

        public void OnCancel()
        {
            if (playersReady[playerIndex - 1] == true)
            {
                playerPanel.transform.Find("Get Ready Panel").transform.Find("ReadyButton").gameObject.SetActive(true);
                playerPanel.GetComponent<Animator>().Play("RotatePlayerModel");
                playersReady[playerIndex - 1] = false;
                playerPanel.transform.Find("EventSystemPlayer").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(null);
                playerPanel.transform.Find("EventSystemPlayer").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(playerPanel.transform.Find("Get Ready Panel").transform.Find("ReadyButton").gameObject);

                GameObject.Find("CountdownTimer").GetComponent<Text>().text = "";
                foreach (MenuHelperFunctions menuHelper in GameObject.FindObjectsOfType<MenuHelperFunctions>())
                {
                    menuHelper.StopCoroutine("LobbyCountdown");
                }
                Debug.Log("Cancel Ready");
            }
            else if (playersJoined[playerIndex -1] == true)
            {
                playerPanel.transform.Find("Get Ready Panel").gameObject.SetActive(false);
                playerPanel.transform.Find("Join Panel").gameObject.SetActive(true);
                playerPanel.transform.Find("EventSystemPlayer").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(null);

                playerPanel.transform.Find("EventSystemPlayer").GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(playerPanel.transform.Find("Join Panel").transform.Find("JoinButton").gameObject);

                playerPanel.GetComponent<Animator>().Play("RotatePlayerModel");
                playersJoined[playerIndex - 1] = false;
                Debug.Log("Cancel Join" + playerIndex);
            }
            else if(playerIndex == 1)
            {
                Destroy(FindObjectOfType<MenuMusic>().gameObject);
                Destroy(FindObjectOfType<PlayerDataHandler>().gameObject);
                LoadScene("Menu");
            }
        }
    }
}
