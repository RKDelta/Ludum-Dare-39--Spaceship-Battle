using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss : BaseUnit
{
    private bool generate_beams = false;
    public GameObject beam;
    public GameObject arms;

    [SerializeField] private float maxHealth = 1000;

    public float Health { get; protected set; }

    public event Action<GameObject> OnDeath;

    public float HealthProprtional
    {
        get
        {
            return this.Health / this.maxHealth;
        }
    }

    protected override void Start()
    {
        GameController.Instance.ObjectMoveRelativeToPlayer(this.gameObject, ref this.OnDeath);

        base.Start();

        this.Health = this.maxHealth;
    }
    
    protected override void Update()
    {
        base.Update();

        if (this.transform.position.x > 0)
        {
            this.transform.position += this.transform.right * -Time.deltaTime;
        }
        else
        {
            if (this.generate_beams == false)
            {
                Quaternion angle1 = Quaternion.Euler(0, 0, 180);
                Quaternion angle2 = Quaternion.Euler(0, 0, 90);
                Quaternion angle3 = Quaternion.Euler(0, 0, -90);
                Quaternion angle4 = Quaternion.Euler(0, 0, 0);


                Instantiate(this.beam, Vector3.zero, angle1, this.arms.transform).transform.localPosition = Vector3.zero;
                Instantiate(this.beam, Vector3.zero, angle2, this.arms.transform).transform.localPosition = Vector3.zero;
                Instantiate(this.beam, Vector3.zero, angle3, this.arms.transform).transform.localPosition = Vector3.zero;
                Instantiate(this.beam, Vector3.zero, angle4, this.arms.transform).transform.localPosition = Vector3.zero;

                this.generate_beams = true;
            }
        }
    }

    public override void DoDamage(float damageAmount)
    {
        this.Health -= damageAmount;

        if (this.Health <= 0)
        {
            Destroy(this);
            return;
        }
    }

    public void OnDestroy()
    {
        if (this.OnDeath != null)
        {
            this.OnDeath(this.gameObject);
        }
    }
}
