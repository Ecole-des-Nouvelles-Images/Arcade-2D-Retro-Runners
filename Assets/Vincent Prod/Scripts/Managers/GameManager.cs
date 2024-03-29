using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vincent_Prod.Scripts.Arenas.Gravity_Arena;

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
                Physics2D.gravity= new Vector2(0,-25);
                GravityManager.GravityArena = false;
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
