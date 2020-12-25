using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
     //Independent development branch
     public int width;
     public int height;
     public GameObject tilePrefab;
     public GameObject[] dotPrefabs;
     public GameObject[,] allDots;
     private BackgroundTile[,] allTiles;
     private GameManager gameManager;
     private SoundManager soundManager;
     private float rowDecreaseWaitTime = .4f;
     private float boardRefillWaitTime = .5f;

     private void Start()
     {
          allTiles = new BackgroundTile[width, height];
          allDots = new GameObject[width, height];
          gameManager = FindObjectOfType<GameManager>();
          soundManager = FindObjectOfType<SoundManager>();
          SetUp();
     }

     public void SetUp()
     {
          for (int i = 0; i < width; i++)
          {
               for (int j = 0; j < height; j++)
               {
                    string name = "( " + i + ", " + j + " )";
                    Vector2 tempPosition = new Vector2(i, j);
                    GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.name = name;

                    //int dotToUse = Random.Range(0, dotPrefabs.Length);
                    //if (MatchesAt(i, j, dotPrefabs[dotToUse]))
                    //{
                    //     dotToUse++;
                    //     dotToUse = dotToUse % dotPrefabs.Length;
                    //}

                    //GameObject dot = Instantiate(dotPrefabs[dotToUse], tempPosition, Quaternion.identity);
                    //dot.transform.parent = this.transform;
                    //dot.name = name;

                    //allDots[i, j] = dot;
               }
          }
          GenerateDots();
     }
     public void GenerateDots()
     {
          CleanBoard();
          for (int i = 0; i < width; i++)
          {
               for (int j = 0; j < height; j++)
               {
                    string name = "( " + i + ", " + j + " )";
                    Vector2 tempPosition = new Vector2(i, j);
                    //GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                    //backgroundTile.transform.parent = this.transform;
                    //backgroundTile.name = name;

                    int dotToUse = Random.Range(0, dotPrefabs.Length);
                    if (MatchesAt(i, j, dotPrefabs[dotToUse]))
                    {
                         dotToUse++;
                         dotToUse = dotToUse % dotPrefabs.Length;
                    }

                    GameObject dot = Instantiate(dotPrefabs[dotToUse], tempPosition, Quaternion.identity);
                    dot.transform.parent = this.transform;
                    dot.name = name;

                    allDots[i, j] = dot;
               }
          }
     }
     private void CleanBoard()
     {
          for (int i = 0; i < width; i++)
          {
               for (int j = 0; j < height; j++)
               {
                    if (allDots[i, j] != null)
                    {
                         Destroy(allDots[i, j]);
                    }
               }
          }
     }
     private bool MatchesAt(int column, int row, GameObject piece)
     {
          if (column > 1 && allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
          {
               return true;
          }
          if (row > 1 && allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
          {
               return true;
          }

          return false;
     }

     private void DestroyMatchesAt(int column, int row)
     {
          Dot currentDot = allDots[column, row].GetComponent<Dot>();
          if (currentDot.isMatched)
          {
               soundManager.PlayDestroySound();
               gameManager.AddToScore(currentDot.scorePrice);
               Destroy(allDots[column, row]);
               allDots[column, row] = null;
          }
     }

     public void DestroyMatches()
     {
          for (int i = 0; i < width; i++)
          {
               for (int j = 0; j < height; j++)
               {
                    if (allDots[i, j] != null)
                    {
                         DestroyMatchesAt(i, j);
                    }
               }
          }
          StartCoroutine(DecreaseRowCo());
     }

     private IEnumerator DecreaseRowCo()
     {
          int nullCount = 0;
          for (int i = 0; i < width; i++)
          {
               for (int j = 0; j < height; j++)
               {
                    if (allDots[i, j] == null)
                    {
                         nullCount++;
                    }else if (nullCount > 0)
                    {
                         allDots[i, j].GetComponent<Dot>().row -= nullCount;
                         allDots[i, j] = null;
                    }
               }
               nullCount = 0;
          }
          yield return new WaitForSeconds(rowDecreaseWaitTime);
          StartCoroutine(FillBoardCo());
     }

     private void RefillBoard()
     {
          for (int i = 0; i < width; i++)
          {
               for (int j = 0; j < height; j++)
               {
                    if (allDots[i, j] == null)
                    {
                         Vector2 tempPosition = new Vector2(i, j);
                         int dotToUse = Random.Range(0, dotPrefabs.Length);
                         GameObject dot = Instantiate(dotPrefabs[dotToUse], tempPosition, Quaternion.identity);
                         dot.transform.parent = this.transform;
                         
                         allDots[i, j] = dot;
                    }
               }
          }
     }

     private bool MatchesOnBoard()
     {
          for (int i = 0; i < width; i++)
          {
               for (int j = 0; j < height; j++)
               {
                    if (allDots[i, j] != null)
                    {
                         if (allDots[i, j].GetComponent<Dot>().isMatched)
                         {
                              return true;
                         }
                    }
               }
          }
          return false;
     }

     private IEnumerator FillBoardCo()
     {
          RefillBoard();
          yield return new WaitForSeconds(boardRefillWaitTime);

          while (MatchesOnBoard())
          {
               yield return new WaitForSeconds(boardRefillWaitTime);
               DestroyMatches();
          }
     }
}
