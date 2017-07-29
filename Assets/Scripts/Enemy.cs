using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseUnit
{
    [SerializeField] private float maxHealth;

    public float Health { get; protected set; }

    public float HealthProprtional
    {
        get
        {
            return this.Health / this.maxHealth;
        }
    }

    public override void DoDamage(float damageAmount)
    {
        this.Health -= damageAmount;

        if (this.Health <= 0)
        {
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
	
	void Update ()
    {
		
	}
}
