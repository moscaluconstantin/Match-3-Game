using System.Collections;
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

     private Board board;
     //private MatchManager matchManager;
     private FindMatches findMatches;
     private GameObject otherDot;
     private Vector2 firstTouchPosition;
     private Vector2 finalTouchPosition;
     private Vector2 tempPosition;
     private float swipeAccuracy = 1f;
     private float moveAccuracy = .1f;
     private float moveSpeed = .6f;
     private float betweenMovesWaitTime = .5f;
     public float swipeAngle = 0f;
     //public float swipeRezist = 1f;

     private void Start()
     {
          board = FindObjectOfType<Board>();
          //matchManager = FindObjectOfType<MatchManager>();
          findMatches = FindObjectOfType<FindMatches>();
          //targetX = (int)transform.position.x;
          //targetY = (int)transform.position.y;
          //column = targetX;
          //row = targetY;
          //previousColumn = column;
          //previousRow = row;
     }
     private void Update()
     {
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
               findMatches.FindAllMatches();
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
               findMatches.FindAllMatches();
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
                    yield return new WaitForSeconds(betweenMovesWaitTime);
                    board.currentState = GameState.move;
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
          if (board.currentState == GameState.move)
          {
               firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          }
     }
     private void OnMouseUp()
     {
          if (board.currentState == GameState.move)
          {
               finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               CalculateAngle();
          }
     }
     private void CalculateAngle()
     {
          Vector2 swipeVector = finalTouchPosition - firstTouchPosition;
          if (Vector2.Distance(firstTouchPosition, finalTouchPosition) >= swipeAccuracy)
          {
               swipeAngle = Mathf.Atan2(swipeVector.y, swipeVector.x) * 180 / Mathf.PI;
               MovePices();
               board.currentState = GameState.wait;
          }
     }
     private void MovePices()
     {
          if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
          {
               //Right swipe
               otherDot = board.allDots[column + 1, row];
               SetPreviousPosition();
               otherDot.GetComponent<Dot>().column -= 1;
               column += 1;
          }
          else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
          {
               //Up swipe
               otherDot = board.allDots[column, row + 1];
               SetPreviousPosition();
               otherDot.GetComponent<Dot>().row -= 1;
               row += 1;
          }
          else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
          {
               //Left swipe
               otherDot = board.allDots[column - 1, row];
               SetPreviousPosition();
               otherDot.GetComponent<Dot>().column += 1;
               column -= 1;
          }
          else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
          {
               //Down swipe
               otherDot = board.allDots[column, row - 1];
               SetPreviousPosition();
               otherDot.GetComponent<Dot>().row += 1;
               row -= 1;
          }
          StartCoroutine(CheckMoveCo());
     }
     private void SetPreviousPosition()
     {
          previousColumn = column;
          previousRow = row;
     }
}
