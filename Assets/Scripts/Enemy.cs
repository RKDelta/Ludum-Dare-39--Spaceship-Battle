using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseUnit
{
    [SerializeField] private float maxHealth = 50;

    public AudioClip[] weaponFireSounds;

    public float Health { get; protected set; }

    public float HealthProprtional
    {
        get
        {
            return this.Health / this.maxHealth;
        }
    }

    public GameObject[] uraniumPrefabs;

    public GameObject weaponDischargePrefab;
    public float weaponCooldown = 0.75f;
    protected float weaponTimeTillNextAllowed;

    public Animator animator;

    public BaseUnit target;

    public float targetDistance = 6;
    private float sqrMinDistance;

    public float targetDistanceRange = 0.75f;
    private float sqrTargetDistanceRange;

    public float fireDistance = 8;
    private float sqrFireDistance;

    public SpawnPoint[] dischargeSpawnPoints;
    protected int dischargeSpawnIndex = 0;

    public event Action<GameObject> OnDeath;

    protected bool lockMovement = false;

    public LayerMask environmentLayer;

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
        if (this.OnDeath != null)
        {
            this.OnDeath(this.gameObject);
        }

        GameObject.Instantiate(
            this.uraniumPrefabs[UnityEngine.Random.Range(0, this.uraniumPrefabs.Length)], 
            this.transform.position,
            Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));

        Destroy(this.gameObject);
    }

    protected override void Start()
    {
        base.Start();

        GameController.Instance.ObjectMoveRelativeToPlayer(this.gameObject, ref this.OnDeath);

        this.Health = this.maxHealth;

        this.sqrMinDistance = this.targetDistance * this.targetDistance;
        this.sqrTargetDistanceRange = this.targetDistanceRange * this.targetDistanceRange;
        this.sqrFireDistance = this.fireDistance * this.fireDistance;
    }

    protected override void Update ()
    {
        float sqrDistToTarget = (this.target.transform.position - this.transform.position).sqrMagnitude;

        Debug.DrawRay(this.transform.position, this.transform.up, Color.green);
        Debug.DrawRay(this.transform.position, (this.target.transform.position - this.transform.position));

        Debug.DrawRay(this.transform.position, this.transform.up * 5, Color.cyan);
        Debug.DrawRay(this.transform.position + (this.transform.rotation * new Vector2(-1.2f, 0.0f)), this.transform.up * 5, Color.cyan);
        Debug.DrawRay(this.transform.position + (this.transform.rotation * new Vector2(1.2f, 0.0f)), this.transform.up * 5, Color.cyan);

        RaycastHit2D centre =
            Physics2D.Raycast(this.transform.position, this.transform.up, this.targetDistance - this.targetDistanceRange, this.environmentLayer);
        RaycastHit2D left =
            Physics2D.Raycast(this.transform.position + (this.transform.rotation * new Vector2(-0.8f, 0.0f)), this.transform.up, this.targetDistance, this.environmentLayer);
        RaycastHit2D right =
            Physics2D.Raycast(this.transform.position + (this.transform.rotation * new Vector2(0.8f, 0.0f)), this.transform.up, this.targetDistance, this.environmentLayer);

        float angle = 0;

        if (centre.collider != null)
        {
            if (left.collider != null)
            {
                if (right.collider == null)
                {
                    //Debug.Log("Go Right");
                    angle = 1;
                }
                else if (right.distance > left.distance)
                {
                    //Debug.Log("Go Right");
                    angle = 1;
                }
                else
                {
                    //Debug.Log("Go Left");
                    angle = -1;
                }
            }
            else
            {
                //Debug.Log("Go Left");
                angle = -1;
            }
        }
        else if (left.collider != null)
        {
            if (right.collider == null)
            {
                //Debug.Log("Go Right");
                angle = 1;
            }
            else if (right.distance > left.distance)
            {
                //Debug.Log("Go Right");
                angle = 1;
            }
            else
            {
                //Debug.Log("Go Left");
                angle = -1;
            }
        }
        else if (right.collider != null)
        {
            //Debug.Log("Go Left");
            angle = -1;
        }
        else
        {
            angle = -Vector3.SignedAngle(this.transform.up, (this.target.transform.position - this.transform.position), Vector3.back);
        }

        if (this.lockMovement == false)
        {
            if (centre.collider != null)
            {
                this.MoveRelative(new Vector2(angle, 0));
            }
            else if (Mathf.Abs(sqrDistToTarget - this.targetDistance) < this.sqrTargetDistanceRange)
            {
                this.MoveRelative(Vector2.left);
            }

            if (Mathf.Abs(angle) > 1)
            {
                this.Rotate(Mathf.Clamp(angle, -1, 1));
            }

            if (centre.collider != null && centre.distance < 2)
            {
                this.MoveRelative(Vector3.down);
            }
            else if (sqrDistToTarget > (this.targetDistance + this.targetDistanceRange) * (this.targetDistance + this.targetDistanceRange))
            {
                this.MoveRelative(Vector2.up);

                if (this.animator != null)
                {
                    this.animator.SetBool("Moving", true);
                }
            }
            else if(sqrDistToTarget < (this.targetDistance - this.targetDistanceRange) * (this.targetDistance - this.targetDistanceRange))
            {
                this.MoveRelative(Vector2.down);

                if (this.animator != null)
                {
                    this.animator.SetBool("Moving", false);
                }
            }
        }
        else
        {
            if (this.animator != null)
            {
                this.animator.SetBool("Moving", false);
            }
        }

        this.weaponTimeTillNextAllowed -= Time.deltaTime;

        if ((sqrDistToTarget < this.sqrFireDistance) && (this.weaponTimeTillNextAllowed <= 0) && (angle <= 10))
        {
            this.FireWeapon();
        }
    }

    protected virtual void FireWeapon()
    {
        GameObject.Instantiate(
            this.weaponDischargePrefab, 
            this.GetNextWorldDischargeSpawnPoint(), 
            this.transform.rotation);
        
        this.weaponTimeTillNextAllowed = this.weaponCooldown;

        if (this.weaponFireSounds.Length > 0)
        {
            AudioManager.Play(this.weaponFireSounds[UnityEngine.Random.Range(0, this.weaponFireSounds.Length)]);
        }
    }

    protected Vector2 GetNextWorldDischargeSpawnPoint()
    {
        Vector2 point
            = this.transform.position + this.transform.rotation * this.dischargeSpawnPoints[this.dischargeSpawnIndex].Rotation * this.dischargeSpawnPoints[this.dischargeSpawnIndex].position;

        this.dischargeSpawnIndex++;
        this.dischargeSpawnIndex %= this.dischargeSpawnPoints.Length;

        return point;
    }

    public void OnDrawGizmosSelected()
    {
        for (int i = 0; i < this.dischargeSpawnPoints.Length; i++)
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawRay((Vector2)this.transform.position + this.laserSpawnPoints[i], Vector2.up * 0.25f);

            this.dischargeSpawnPoints[i].DrawGizmo(this.transform);
        }
    }
}