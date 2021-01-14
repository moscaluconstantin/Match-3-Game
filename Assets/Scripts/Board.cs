using Assets.Scripts;
using System.Collections;
using UnityEngine;

public enum GameState
{
     move,
     wait
}
public class Board : MonoBehaviour
{
     public GameState currentState;
     public int width;
     public int height;
     public int offSet;
     public FindMatches findMatches;
     public GameObject tilePrefab;
     public GameObject[] dotPrefabs;
     public GameObject[,] allDots;
     private BackgroundTile[,] allTiles;
     private GameManager gameManager;
     private SoundManager soundManager;
     private RandomWithExclusion random;
     private float rowDecreaseWaitTime = .4f;
     private float boardRefillWaitTime = .5f;
     private float betweenMovesWaitTime = .5f;


     private void Start()
     {
          random = new RandomWithExclusion(dotPrefabs.Length);
          allTiles = new BackgroundTile[width, height];
          allDots = new GameObject[width, height];
          gameManager = FindObjectOfType<GameManager>();
          soundManager = FindObjectOfType<SoundManager>();
          findMatches = FindObjectOfType<FindMatches>();
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
               }
          }
          GenerateDots();
     }
     public void GenerateDots()
     {
          currentState = GameState.move;
          CleanBoard();
          for (int i = 0; i < width; i++)
          {
               for (int j = 0; j < height; j++)
               {
                    string name = "( " + i + ", " + j + " )";
                    Vector2 tempPosition = new Vector2(i, j + offSet);

                    int dotToUse = 0;

                    for (int x = 0; x < dotPrefabs.Length; x++)
                    {
                         dotToUse = random.Next();
                         if (dotToUse == -1)
                         {
                              dotToUse = 0;
                              break;
                         }
                         if (!MatchesAt(i, j, dotPrefabs[dotToUse]))
                         {
                              break;
                         }
                    }


                    GameObject dot = Instantiate(dotPrefabs[dotToUse], tempPosition, Quaternion.identity);
                    Dot script = dot.GetComponent<Dot>();
                    script.row = j;
                    script.column = i;
                    dot.transform.parent = this.transform;
                    dot.name = name;

                    allDots[i, j] = dot;
                    random.Reload();
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
          //row matches
          if (column > 1 && allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
          {
               return true;
          }
          //column matches
          if (row > 1 && allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
          {
               return true;
          }
          #region + matches
          //if (column > 0 && row > 0)
          //{
          //     //left T match
          //     if (allDots[column - 1, row].tag == piece.tag && allDots[column, row - 1].tag == piece.tag)
          //     {
          //          return true;
          //     }
          //     //right T match
          //     if (allDots[column - 1, row].tag == piece.tag && allDots[column - 1, row - 1].tag == piece.tag)
          //     {
          //          return true;
          //     }
          //     //J match
          //     if (allDots[column, row - 1].tag == piece.tag && allDots[column - 1, row - 1].tag == piece.tag)
          //     {
          //          return true;
          //     }
          //}
          ////L match
          //if (column > 0 && row < width - 1 && allDots[column - 1, row].tag == piece.tag &&
          //     allDots[column - 1, row + 1].tag == piece.tag)
          //{
          //     return true;
          //}
          #endregion
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
                    }
                    else if (nullCount > 0)
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
                         Vector2 tempPosition = new Vector2(i, j + offSet);
                         int dotToUse = Random.Range(0, dotPrefabs.Length);
                         GameObject dot = Instantiate(dotPrefabs[dotToUse], tempPosition, Quaternion.identity);
                         dot.transform.parent = this.transform;

                         allDots[i, j] = dot;

                         Dot script = dot.GetComponent<Dot>();
                         script.row = j;
                         script.column = i;
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
          yield return new WaitForSeconds(betweenMovesWaitTime);
          currentState = GameState.move;
     }
}
