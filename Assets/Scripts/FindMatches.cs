using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
     public List<GameObject> currentMatches;
     private Board board;

     private void Start()
     {
          currentMatches = new List<GameObject>();
          board = FindObjectOfType<Board>();
     }

     public void FindAllMatches()
     {
          StartCoroutine(FindAllMatchesCo());
     }
     private IEnumerator FindAllMatchesCo()
     {
          yield return new WaitForSeconds(.2f);
          for (int i = 0; i < board.width; i++)
          {
               for (int j = 0; j < board.height; j++)
               {
                    GameObject currentDot = board.allDots[i, j];

                    TwoLeft(i, j, currentDot, true);
                    TwoBelow(i, j, currentDot, true);
                    //TwoBelowAndLeft(i, j, currentDot, true);
                    //TwoLeftAndAbove(i, j, currentDot, true);
                    //OneLeftOneBelow(i, j, currentDot, true);
                    //TwoLeftAndBelow(i, j, currentDot, true);
               }
          }
     }
     private bool TwoLeft(int column, int row, GameObject ex, bool markMatch)
     {
          if (column > 1)
          {
               return ThreeMatching(board.allDots[column - 1, row],
                    board.allDots[column - 2, row], ex, markMatch);
          }
          return false;
     }
     public bool TwoBelow(int column, int row, GameObject ex, bool markMatch)
     {
          if (row > 1)
          {
               return ThreeMatching(board.allDots[column, row - 1],
                    board.allDots[column, row - 2], ex, markMatch);
          }
          return false;
     }
     //private bool OneLeftOneBelow(int column, int row, GameObject ex, bool markMatch)
     //{
     //     if (column > 0 && row > 0)
     //     {
     //          return ThreeMatching(board.allDots[column - 1, row],
     //               board.allDots[column, row - 1], ex, markMatch);
     //     }
     //     return false;
     //}
     //private bool TwoLeftAndBelow(int column, int row, GameObject ex, bool markMatch)
     //{
     //     if (column > 0 && row > 0)
     //     {
     //          return ThreeMatching(board.allDots[column - 1, row],
     //               board.allDots[column - 1, row - 1], ex, markMatch);
     //     }
     //     return false;
     //}
     //private bool TwoLeftAndAbove(int column, int row, GameObject ex, bool markMatch)
     //{
     //     if (column > 0 && row < board.width - 1)
     //     {
     //          return ThreeMatching(board.allDots[column - 1, row],
     //               board.allDots[column - 1, row + 1], ex, markMatch);
     //     }
     //     return false;
     //}
     //private bool TwoBelowAndLeft(int column, int row, GameObject ex, bool markMatch)
     //{
     //     if (column > 0 && row > 0)
     //     {
     //          return ThreeMatching(board.allDots[column, row - 1], 
     //               board.allDots[column - 1, row - 1], ex, markMatch);
     //     }
     //     return false;
     //}
     private bool ThreeMatching(GameObject Ex1, GameObject Ex2, GameObject Ex, bool markMatch)
     {
          if (Ex1 == null || Ex2 == null || Ex == null)
          {
               return false;
          }

          Dot Ex1Script = Ex1.GetComponent<Dot>();
          Dot Ex2Script = Ex2.GetComponent<Dot>();

          //if (Ex1.tag != Ex.tag || Ex2.tag != Ex.tag || Ex1Script.isMatched || Ex2Script.isMatched)
          if (Ex1.tag != Ex.tag || Ex2.tag != Ex.tag)
          {
               return false;
          }

          if (markMatch)
          {
               Ex1Script.isMatched = true;
               Ex2Script.isMatched = true;
               Ex.GetComponent<Dot>().isMatched = true;
          }

          return true;
     }
}
