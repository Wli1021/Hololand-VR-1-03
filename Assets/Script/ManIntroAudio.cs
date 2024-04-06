using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManIntroAudio : MonoBehaviour
{
    public AudioSource manVoice;
    public float delay = 5f;
    void Start()
    {
        //Plays Man Voice after 5 seconds
        manVoice.PlayDelayed(delay);
    }
 

}
