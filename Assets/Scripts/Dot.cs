﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
     [Header("Board Variables")]
     public int column;
     public int row;
     public int previousColumn;
     public int previousRow;
     public int targetX;
     public int targetY;
     public bool isMatched = false;
     public int scorePrice = 1;

     //private GameManager gameManager;
     private Board board;
     private GameObject otherDot;
     private Vector2 firstTouchPosition;
     private Vector2 finalTouchPosition;
     private Vector2 tempPosition;
     private float swipeAccuracy = 1f;
     private float moveAccuracy = .1f;
     private float moveSpeed = .6f;
     public float swipeAngle = 0f;
     public float swipeRezist = 1f;

     private void Start()
     {
          //gameManager = FindObjectOfType<GameManager>();
          board = FindObjectOfType<Board>();
          targetX = (int)transform.position.x;
          targetY = (int)transform.position.y;
          column = targetX;
          row = targetY;
          previousColumn = column;
          previousRow = row;
     }
     private void Update()
     {
          FindMatches();
          if (isMatched)
          {
               SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
               mySprite.color = new Color(0f, 0f, 0f, .2f);
          }
          targetX = column;
          targetY = row;
          if (Mathf.Abs(targetX - transform.position.x) > moveAccuracy)
          {
               //Move Towards the target
               tempPosition = new Vector2(targetX, transform.position.y);
               transform.position = Vector2.Lerp(transform.position, tempPosition, moveSpeed);
               if (board.allDots[column, row] != this.gameObject)
               {
                    board.allDots[column, row] = this.gameObject;
               }
          }
          else
          {
               //Directly set the position
               tempPosition = new Vector2(targetX, transform.position.y);
               transform.position = tempPosition;
          }
          if (Mathf.Abs(targetY - transform.position.y) > moveAccuracy)
          {
               //Move Towards the target
               tempPosition = new Vector2(transform.position.x, targetY);
               transform.position = Vector2.Lerp(transform.position, tempPosition, moveSpeed);
               if (board.allDots[column, row] != this.gameObject)
               {
                    board.allDots[column, row] = this.gameObject;
               }
          }
          else
          {
               //Directly set the position
               tempPosition = new Vector2(transform.position.x, targetY);
               transform.position = tempPosition;
          }
     }

     public IEnumerator CheckMoveCo()
     {
          yield return new WaitForSeconds(.5f);
          if (otherDot != null)
          {
               Dot otherDot_Dot = otherDot.GetComponent<Dot>();
               if (!isMatched && !otherDot_Dot.isMatched)
               {
                    otherDot_Dot.row = row;
                    otherDot_Dot.column = column;
                    row = previousRow;
                    column = previousColumn;
               }
               else
               {
                    board.DestroyMatches();
               }
               otherDot = null;
          }
     }
     private void OnMouseDown()
     {
          firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
     }
     private void OnMouseUp()
     {
          finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          CalculateAngle();
     }
     private void CalculateAngle()
     {
          Vector2 swipeVector = finalTouchPosition - firstTouchPosition;
          if (Vector2.Distance(firstTouchPosition, finalTouchPosition) >= swipeAccuracy)
          {
               swipeAngle = Mathf.Atan2(swipeVector.y, swipeVector.x) * 180 / Mathf.PI;
               MovePices();
          }
     }
     private void MovePices()
     {
          if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
          {
               //Right swipe
               otherDot = board.allDots[column + 1, row];
               otherDot.GetComponent<Dot>().column -= 1;
               column += 1;
          }
          else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
          {
               //Up swipe
               otherDot = board.allDots[column, row + 1];
               otherDot.GetComponent<Dot>().row -= 1;
               row += 1;
          }
          else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
          {
               //Left swipe
               otherDot = board.allDots[column - 1, row];
               otherDot.GetComponent<Dot>().column += 1;
               column -= 1;
          }
          else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
          {
               //Down swipe
               otherDot = board.allDots[column, row - 1];
               otherDot.GetComponent<Dot>().row += 1;
               row -= 1;
          }
          StartCoroutine(CheckMoveCo());
     }
     private void FindMatches()
     {
          if (column > 0 && column < board.width - 1)
          {
               GameObject leftDot1 = board.allDots[column - 1, row];
               GameObject rightDot1 = board.allDots[column + 1, row];
               if (leftDot1 == null || rightDot1 == null)
               {
                    return;
               }
               if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
               {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
               }
          }
          if (row > 0 && row < board.height - 1)
          {
               GameObject downDot1 = board.allDots[column, row - 1];
               GameObject upDot1 = board.allDots[column, row + 1];
               if (downDot1 == null || upDot1 == null)
               {
                    return;
               }
               if (downDot1.tag == this.gameObject.tag && upDot1.tag == this.gameObject.tag)
               {
                    downDot1.GetComponent<Dot>().isMatched = true;
                    upDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
               }
          }
     }
}
