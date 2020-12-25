using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
     public Text scoreText;
     public Text timerText;
     public GameObject gameOverUI;
     public Text finalScoreText;
     private float timerValue;
     private float scoreValue;

     private void Start()
     {
          StartGame();
     }
     private void Update()
     {
          if (timerValue > Time.deltaTime)
          {
               timerValue -= Time.deltaTime;
          }
          else
          {
               timerValue = 0f;
               EndGame();
          }
          UpdateTimer();
     }

     public void AddToScore(int value)
     {
          if (timerValue > 0)
          {
               scoreValue += value;
          }
          UpdateScore();
     }
     private void UpdateScore()
     {
          scoreText.text = "Score: " + scoreValue;
     }
     private void UpdateTimer()
     {
          timerText.text = timerValue.ToString();
     }
     private void EndGame()
     {
          gameOverUI.SetActive(true);
          finalScoreText.text = scoreValue.ToString();
     }
     public void StartGame()
     {
          timerValue = 60f;
          scoreValue = 0;
          UpdateScore();
          UpdateTimer();
          gameOverUI.SetActive(false);
     }
}
