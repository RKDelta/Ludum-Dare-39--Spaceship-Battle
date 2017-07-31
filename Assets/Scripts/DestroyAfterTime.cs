using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float animationLength;

    private float time;

    void Start()
    {
    }
    
    void Update()
    {
        this.time += Time.deltaTime;

        if (this.time >= this.animationLength)
        {
            Destroy(this.gameObject);
        }
    }
}