using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ExplosionController : MonoBehaviour
{
    // public bool shouldPlay;
    VideoPlayer video;
    // private bool finishedPlaying;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        // shouldPlay = false;
        // finishedPlaying = false;
        video.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        // shouldPlay = false;
        // finishedPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("shouldPlay" + shouldPlay);
        // Debug.Log("finishedPlaying" + finishedPlaying);
        // if (shouldPlay)
        // {
        //     if ((float)video.frame < video.frameCount - 3)
        //     {
        //         finishedPlaying = false;
        //     }
        //     else
        //     {
        //         finishedPlaying = true;
        //         shouldPlay = false;
        //         Debug.Log("finisheddddddd");
        //     }
        // }

        // if (shouldPlay && !finishedPlaying)
        // {
        //     video.Play();
        //     return;
        // }

        // if (shouldPlay)
        // {
        //     finishedPlaying = false;
        // }
        // else
        // {
        //     video.Stop();
        //     finishedPlaying = true;
        // }
    }

    // public void ShowExplosion()
    // {
    //     shouldPlay = true;
    // }
}
