using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
     public class RandomWithExclusion
     {
          private int NomberOfElements;
          private List<int> Elements;
          public RandomWithExclusion(int nomberOfElements)
          {
               NomberOfElements = nomberOfElements;
               Elements = new List<int>();
          }
          public void Reload()
          {
               Elements.Clear();
               for (int i = 0; i < NomberOfElements; i++)
               {
                    Elements.Add(i);
               }
          }
          public int Next()
          {
               int temp = -1;
               if (Elements.Count > 0)
               {
                    int tempIndx = Random.Range(0, Elements.Count());
                    temp = Elements[tempIndx];
                    Elements.RemoveAt(tempIndx);
               }
               
               return temp;
          }
     }
}
