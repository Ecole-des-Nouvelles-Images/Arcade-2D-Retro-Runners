using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vincent_Prod.Scripts.Menu
{
    public class MenuHelperFunctions : MonoBehaviour
    {
        public static bool[] playersReady = new bool[4]{ false, false, false, false };
        public static bool[] playersJoined = new bool[4] { false, false, false, false };

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
            int countdownDuration = 10;

            int remainingTime = countdownDuration;
            for(int i = 0; i < countdownDuration; i++)
            {
                GameObject.Find("CountdownTimer").GetComponent<Text>().text = "Start\n" + remainingTime.ToString();
                yield return new WaitForSeconds(1);
                remainingTime--;
            }

            LoadScene("Arena");
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
                LoadScene("Menu");
            }
        }
    }
}
