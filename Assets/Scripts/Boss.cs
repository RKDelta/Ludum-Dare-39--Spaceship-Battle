using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss : BaseUnit
{
    enum BeamMovement
    {
        clockwise,
        anticlockwise,
        stop_clockwiseNext,
        stop_anticlockwiseNext
    }

    private BeamMovement beamMovement = BeamMovement.stop_clockwiseNext;

    public float wait = 3f;
    public float spinSpeed = 60f;
    float time;
    private bool is_going_left;

    private bool generatedBeams = false;
    public GameObject beam;
    public GameObject arms;

    [SerializeField] private float maxHealth = 1000;

    public float Health { get; protected set; }

    public float HealthProprtional
    {
        get
        {
            return this.Health / this.maxHealth;
        }
    }

    public LayerMask layersToClear;
    public float clearRadius;
    private bool cleared;

    public Action OnDestroyed;

    private bool inPosition;

    public GameObject shockwaveEffect;

    protected override void Start()
    {
        base.Start();

        this.Health = this.maxHealth;

        Debug.Log(this.Health);
    }

    protected override void Update()
    {
        if (GameController.Instance.isPaused == false)
        {
            base.Update();

            if (this.transform.localPosition.x > 0)
            {
                this.transform.localPosition += this.transform.right * -Time.deltaTime;
            }
            else
            {
                if (this.cleared == false)
                {
                    Collider2D[] results = Physics2D.OverlapCircleAll(this.transform.position, this.clearRadius, this.layersToClear);

                    foreach (Collider2D collider in results)
                    {
                        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

                        if (rb != null)
                        {
                            collider.enabled = false;

                            rb.drag = 0;

                            rb.AddForceAtPosition(
                                Quaternion.Euler(0, 0, Vector3.SignedAngle(collider.transform.position - this.transform.position, Vector3.up, Vector3.back)) * new Vector2(0, 10000),
                                this.transform.position,
                                ForceMode2D.Impulse);

                            GameController.Instance.RemoveObjectFromRelativeMovement(collider.gameObject);
                        }
                    }

                    GameObject.Instantiate(this.shockwaveEffect, this.transform.position, Quaternion.identity);

                    this.cleared = true;
                }

                this.time += Time.deltaTime;

                if (this.generatedBeams == false && this.time >= 0.75)
                {
                    Quaternion angle1 = Quaternion.Euler(0, 0, 180);
                    Quaternion angle2 = Quaternion.Euler(0, 0, 90);
                    Quaternion angle3 = Quaternion.Euler(0, 0, -90);
                    Quaternion angle4 = Quaternion.Euler(0, 0, 0);
                    
                    Instantiate(this.beam, Vector3.zero, angle1, this.arms.transform).transform.localPosition = Vector3.zero;
                    Instantiate(this.beam, Vector3.zero, angle2, this.arms.transform).transform.localPosition = Vector3.zero;
                    Instantiate(this.beam, Vector3.zero, angle3, this.arms.transform).transform.localPosition = Vector3.zero;
                    Instantiate(this.beam, Vector3.zero, angle4, this.arms.transform).transform.localPosition = Vector3.zero;

                    this.generatedBeams = true;

                    this.time = 4;
                }

                if (this.time >= 6 && this.generatedBeams)
                {
                    this.time -= 6;

                    switch (this.beamMovement)
                    {
                        case BeamMovement.clockwise:
                            this.beamMovement = BeamMovement.stop_anticlockwiseNext;
                            break;
                        case BeamMovement.anticlockwise:
                            this.beamMovement = BeamMovement.stop_clockwiseNext;
                            break;
                        case BeamMovement.stop_clockwiseNext:
                            this.beamMovement = BeamMovement.clockwise;
                            break;
                        case BeamMovement.stop_anticlockwiseNext:
                            this.beamMovement = BeamMovement.anticlockwise;
                            break;
                    }
                }

                switch (this.beamMovement)
                {
                    case BeamMovement.clockwise:
                        SpinAnticlockwise();
                        break;
                    case BeamMovement.anticlockwise:
                        SpinClockwise();
                        break;
                }
            }
        }
    }

    void SpinAnticlockwise()
    {
        this.arms.transform.Rotate(0, 0, this.spinSpeed * Time.deltaTime);
    }

    void SpinClockwise()
    {
        this.arms.transform.Rotate(0, 0, this.spinSpeed * -Time.deltaTime);
    }

    public override void DoDamage(float damageAmount)
    {
        if (this.generatedBeams == false)
        {
            return;
        }

        this.Health -= damageAmount;

        if (this.Health <= 0)
        {
            if (this.OnDestroyed != null)
            {
                this.OnDestroyed();
            }

            Destroy(this.gameObject);
            return;
        }
    }
}
