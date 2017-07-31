using System;
using UnityEngine;

public class Torpedo : BaseUnit
{
    public Transform target;

    public float activationDistance = 2.0f;
    public float explosionRadius = 3.0f;
    public float damage = 0.5f;

    public LayerMask canHit;

    public float thrusterActivationTime = 1;

    float timeSinceCreation = 0;

    bool thrustersActive = false;

    float force;

    public Animator animator;

    public float maxTime;

    protected override void Start()
    {
        base.Start();

        this.animator.SetBool("Moving", false);

        this.rb = this.GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        this.timeSinceCreation += Time.deltaTime;

        if (this.timeSinceCreation > this.thrusterActivationTime)
        {
            this.thrustersActive = true;

            if (Physics2D.Raycast(this.transform.position, this.transform.up, this.activationDistance, this.canHit).collider != null)
            {
                this.Explode();
            }

            if (this.timeSinceCreation > this.maxTime)
            {
                this.Explode();
            }
        }
    }

    public void FixedUpdate()
    {
        if (this.thrustersActive)
        {
            this.animator.SetBool("Moving", true);

            this.MoveRelative(Vector3.up);

            float angle = -Vector3.SignedAngle(this.transform.up, ((Vector2)this.target.position - (Vector2)this.transform.position), Vector3.back);
            
            this.Rotate(Mathf.Clamp(angle, -1, 1));
        }
    }

    public void Explode()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(this.transform.position, this.explosionRadius, this.canHit);

        foreach (Collider2D collider in results)
        {
            BaseUnit unit = collider.GetComponent<BaseUnit>();

            if (unit != null && unit != this)
            {
                unit.DoDamage(this.damage);
            }
        }

        results = Physics2D.OverlapCircleAll(this.transform.position, this.explosionRadius, this.canHit);

        foreach (Collider2D collider in results)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForceAtPosition(this.transform.up * this.force, this.transform.position, ForceMode2D.Impulse);
            }
        }

        Destroy(this.gameObject);
        return;
    }

    public override void DoDamage(float damageAmount)
    {
        this.Explode();
    }
}
