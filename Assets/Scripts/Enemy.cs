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

    public GameObject laserShotPrefab;
    public float laserCooldown = 0.75f;
    private float laserTimeTillNextAllowed;

    public BaseUnit target;

    public float targetDistance = 3;
    private float sqrMinDistance;

    public float targetDistanceRange = 0.75f;
    private float sqrTargetDistanceRange;

    public float fireDistance = 5;
    private float sqrFireDistance;

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

    protected override void Start()
    {
        base.Start();

        this.Health = this.maxHealth;

        this.sqrMinDistance = this.targetDistance * this.targetDistance;
        this.sqrTargetDistanceRange = this.targetDistanceRange * this.targetDistanceRange;
        this.sqrFireDistance = this.fireDistance * this.fireDistance;
    }

    void Update ()
    {
        float sqrDistToTarget = (this.target.transform.position - this.transform.position).sqrMagnitude;

        if (Mathf.Abs(sqrDistToTarget - this.targetDistance) < this.sqrTargetDistanceRange)
        {
            this.Move(Vector2.left);
        }

        Debug.DrawRay(this.transform.position, this.transform.up, Color.green);
        Debug.DrawRay(this.transform.position, (this.target.transform.position - this.transform.position));

        float angle = -Vector3.SignedAngle(this.transform.up, (this.target.transform.position - this.transform.position), Vector3.back);

        this.laserTimeTillNextAllowed -= Time.deltaTime;

        if (Mathf.Abs(angle) > 1)
        {
            this.Rotate(Mathf.Clamp(angle, -1, 1));
        }

        if (sqrDistToTarget > this.targetDistance)
        {
            this.Move(Vector2.up);
        }
        else
        {
            this.Move(Vector2.down);
        }

        if ((sqrDistToTarget < this.sqrFireDistance) && (this.laserTimeTillNextAllowed <= 0))
        {
            // Fire!
            GameObject.Instantiate(this.laserShotPrefab, this.transform.position, this.transform.rotation);
            this.laserTimeTillNextAllowed = this.laserCooldown;
        }
    }
}
