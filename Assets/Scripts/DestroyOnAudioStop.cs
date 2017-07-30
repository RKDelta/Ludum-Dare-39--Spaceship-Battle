using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAudioStop : MonoBehaviour
{
    AudioSource source;

    void Start()
    {
        this.source = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (this.source.isPlaying == false)
        {
            Destroy(this.gameObject);
        }
    }
}
