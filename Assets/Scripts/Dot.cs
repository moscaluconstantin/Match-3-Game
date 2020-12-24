﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
     public int column;
     public int row;
     public int targetX;
     public int targetY;
     public bool isMatched = false;
     private Board board;
     private GameObject otherDot;
     private Vector2 firstTouchPosition;
     private Vector2 finalTouchPosition;
     private Vector2 tempPosition;
     public float swipeAngle = 0f;
     private float moveAccuracy = .1f;
     private float moveSpeed = .4f;

     private void Start()
     {
          board = FindObjectOfType<Board>();
          targetX = (int)transform.position.x;
          targetY = (int)transform.position.y;
          column = targetX;
          row = targetY;
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
          }
          else
          {
               //Directly set the position
               tempPosition = new Vector2(targetX, transform.position.y);
               transform.position = tempPosition;
               board.allDots[column, row] = this.gameObject;
          }
          if (Mathf.Abs(targetY - transform.position.y) > moveAccuracy)
          {
               //Move Towards the target
               tempPosition = new Vector2(transform.position.x, targetY);
               transform.position = Vector2.Lerp(transform.position, tempPosition, moveSpeed);
          }
          else
          {
               //Directly set the position
               tempPosition = new Vector2(transform.position.x, targetY);
               transform.position = tempPosition;
               board.allDots[column, row] = this.gameObject;
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
          swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
               finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
          //Debug.Log(swipeAngle);
          MovePices();
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
     }
     private void FindMatches()
     {
          if (column > 0 && column < board.width - 1)
          {
               GameObject leftDot1 = board.allDots[column - 1, row];
               GameObject rightDot1 = board.allDots[column + 1, row];
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
               if (downDot1.tag == this.gameObject.tag && upDot1.tag == this.gameObject.tag)
               {
                    downDot1.GetComponent<Dot>().isMatched = true;
                    upDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
               }
          }
     }
}
