using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
     public Text scoreText;
     public Text timerText;
     public GameObject gameOverUI;
     public Board board;
     public Text finalScoreText;
     private float timerValue;
     private float scoreValue;

     private void Start()
     {
          GameManagerSetUp();
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

     private void GameManagerSetUp()
     {
          timerValue = 60f;
          scoreValue = 0;
          UpdateScore();
          UpdateTimer();
          gameOverUI.SetActive(false);
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
          GameManagerSetUp();
          board.GenerateDots();
     }    
}
