using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WeaponDischarge : MonoBehaviour
{
    public float speed = 10;
    public float damageAmount = 10;

    public float range = 100;
    private float sqrRange;

    public GameObject hitEffect;

    private Vector2 startPos;

    private Vector2 movementPerTimeStep;
    private float activationDistance;

    public LayerMask canHit;

    public float force = 1f;
    
    public void Start()
    {
        this.movementPerTimeStep = this.transform.rotation * Vector3.up * this.speed * Time.fixedDeltaTime;

        this.activationDistance = this.movementPerTimeStep.magnitude * 2f;

        this.sqrRange = this.range * this.range;
    }

    public void FixedUpdate()
    {
        this.transform.Translate(this.movementPerTimeStep, Space.World);

        RaycastHit2D result = Physics2D.Raycast(this.transform.position, this.transform.forward, this.activationDistance, this.canHit);

        if (result.collider != null)
        {
            BaseUnit unit = result.collider.GetComponent<BaseUnit>();

            if (unit != null)
            {
                unit.DoDamage(this.damageAmount);
            }

            Rigidbody2D rb = result.collider.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForceAtPosition(this.transform.up * this.force, this.transform.position);
            }

            Destroy(this.gameObject);

            return;
        }

        if (Vector3.SqrMagnitude((Vector2)this.transform.position - this.startPos) >= sqrRange)
        {
            Destroy(this.gameObject);
        }
    }
}