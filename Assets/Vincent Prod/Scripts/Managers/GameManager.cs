using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vincent_Prod.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public float timeRemaining = 180;
        public bool timerIsRunning = false;
        public TextMeshProUGUI timeText;

        private void Start()
        {
            timerIsRunning = true;
        }

        private void Update()
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out !");
                timerIsRunning = false;
                SceneManager.LoadScene("VictoryScreen");
            }
        }
        
        void DisplayTime(float timeToDisplay)
        {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
        
    }
}
