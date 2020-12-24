using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
     private Vector2 firstTouchPosition;
     private Vector2 finalTouchPosition;
     public float swipeAngle = 0f;

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
          Debug.Log(swipeAngle);
     }
}
