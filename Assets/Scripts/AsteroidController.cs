using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AsteroidController : MonoBehaviour
{
    public event Action<GameObject> OnDeath;

    void Start()
    {
        GameController.Instance.ObjectMoveRelativeToPlayer(this.gameObject, ref this.OnDeath);
    }

    void FixedUpdate()
    {
    }

    private void OnDestroy()
    {
        if (this.OnDeath != null)
        {
            this.OnDeath(this.gameObject);
        }
    }
}