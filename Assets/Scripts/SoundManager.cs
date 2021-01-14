using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
     [Header("Permissions")]
     public bool useBackgroundSound = true;
     public bool useDestroySound = true;
     public bool useSwapSound = true;

     [Header("Sounds")]
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
          if (useDestroySound)
          {
               destroySound.Play();
          }
     }
     public void PlaySwapSound()
     {
          if (useSwapSound)
          {
               swapSound.Play();
          }
     }
     public void PlayBackgroundSound()
     {
          if (!backgroundSound.isPlaying && useBackgroundSound)
          {
               backgroundSound.Play();
          }
     }
}
