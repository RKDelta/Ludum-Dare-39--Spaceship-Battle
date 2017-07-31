using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseUnit
{
    [SerializeField] private float maxPower = 1000;

    private float _power;

    public GameObject laserShotPrefab;
    public float laserCooldown = 0.15f;
    private float laserTimeTillNextAllowed;

    public GameObject shields;

    public Animator animator;
    public Animator shieldAnimator;

    public SpawnPoint[] laserSpawnPoints;
    private int laserSpawnIndex = 0;

    public AudioClip[] laserSounds;
    public AudioClip[] laserCantFireSounds;

    private bool justDamaged;

    public float Power
    {
        get
        {
            return this._power;
        }
        protected set
        {
            this._power = Mathf.Clamp(value, 0, this.maxPower);

            if (this._power <= 0)
            {
                PlayerDeath();
            }
        }
    }

    public float PowerProportional
    {
        get
        {
            return this.Power / this.maxPower;
        }
    }

    protected override void Start()
    {
        base.Start();

        this.Power = this.maxPower;

        this.StartCoroutine(this.ShieldDamage());
    }

    protected override void Update ()
    {
        this.laserTimeTillNextAllowed -= Time.deltaTime;

        this.MoveRelative(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (this.animator != null)
            {
                this.animator.SetBool("Moving", true);
            }
        }
        else
        {
            if (this.animator != null)
            {
                this.animator.SetBool("Moving", false);
            }
        }


        Debug.DrawRay(this.transform.position, this.transform.up * 100, Color.blue);
        Debug.DrawRay(this.transform.position, (CameraRig.GetWorldMousePosition() - (Vector2)this.transform.position), Color.red);

        float angle = -Vector3.SignedAngle(this.transform.up, (CameraRig.GetWorldMousePosition() - (Vector2)this.transform.position), Vector3.back);

        if (Mathf.Abs(angle) > 1)
        {
            this.Rotate(Mathf.Clamp(angle, -1, 1));
        }

        if (Input.GetMouseButton(0) && this.laserTimeTillNextAllowed <= 0)
        {
            Vector2 worldLaserSpawnPoint = 
                this.transform.position + this.transform.rotation * this.laserSpawnPoints[this.laserSpawnIndex].Rotation * this.laserSpawnPoints[this.laserSpawnIndex].position;

            Quaternion laserDirection = 
                Quaternion.RotateTowards(
                    this.transform.rotation,
                    Quaternion.Euler(0, 0, -Vector3.SignedAngle(Vector3.up, (CameraRig.GetWorldMousePosition() - worldLaserSpawnPoint), Vector3.back)),
                    30.0f);

            GameObject shotGO = GameObject.Instantiate(
                this.laserShotPrefab, 
                worldLaserSpawnPoint,
                laserDirection);

            WeaponDischarge discharge = shotGO.GetComponent<WeaponDischarge>();
            discharge.parentVelocity = this.rb.velocity;

            this.laserSpawnIndex++;
            this.laserSpawnIndex %= this.laserSpawnPoints.Length;

            this.laserTimeTillNextAllowed = this.laserCooldown;

            AudioManager.Play(this.laserSounds[UnityEngine.Random.Range(0, this.laserSounds.Length)]);
        }
	}

    public override void DoDamage(float damageAmount)
    {
        this.Power -= damageAmount;

        this.justDamaged = true;
    }

    IEnumerator ShieldDamage()
    {
        while (true)
        {
            if (this.justDamaged)
            {
                this.shieldAnimator.SetBool("TakingDamage", true);

                this.justDamaged = false;

                yield return new WaitForSeconds(0.15f);

                this.shieldAnimator.SetBool("TakingDamage", false);

                continue;
            }

            yield return null;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        UraniumData uranium = collider.GetComponent<UraniumData>();

        if (uranium != null)
        {
            this.Power += uranium.powerAmount;

            Destroy(collider.gameObject);
        }
    }

    public void PlayerDeath()
    {
        Debug.Log("The player died.");

        this.shields.SetActive(false);
    }

    public void OnDrawGizmosSelected()
    {
        for (int i = 0; i < this.laserSpawnPoints.Length; i++)
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawRay((Vector2)this.transform.position + this.laserSpawnPoints[i], Vector2.up * 0.25f);

            this.laserSpawnPoints[i].DrawGizmo(this.transform);
        }
    }
}
