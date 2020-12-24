using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
     public GameObject[] dots;

     private void Start()
     {
          Initialize();
     }

     private void Initialize()
     {
          int dotToUse = Random.Range(0, dots.Length);
          GameObject dot = Instantiate(dots[dotToUse], transform.position, Quaternion.identity);
          dot.transform.parent = this.transform;
          dot.name = this.gameObject.name;
     }
}
