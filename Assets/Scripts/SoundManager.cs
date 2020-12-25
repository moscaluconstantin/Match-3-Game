using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
     public AudioSource destroySound;
     public AudioSource swapSound;
     public AudioSource backgroundSound;
     private bool isPlaying;

     private void Start()
     {
          PlayBackgroundSound();
     }
     private void Update()
     {
          PlayBackgroundSound();
     }

     public void PlayDestroySound()
     {
          destroySound.Play();
     }
     public void PlaySwapSound()
     {
          swapSound.Play();
     }
     public void PlayBackgroundSound()
     {
          if (!backgroundSound.isPlaying)
          {
               backgroundSound.Play();
          }
     }
}
