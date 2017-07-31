using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEnemy : Enemy
{
    public float chargeTime = 1.0f;

    public AudioClip[] chargingSounds;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FireWeapon()
    {
        this.StartCoroutine(BeamWeaponHandler());
    }

    IEnumerator BeamWeaponHandler()
    {
        this.lockMovement = true;

        this.weaponTimeTillNextAllowed = this.weaponCooldown;

        yield return null;

        this.animator.SetBool("ChargingBeam", true);
        this.animator.SetBool("Firing", true);

        if (this.chargingSounds.Length > 0)
        {
            AudioManager.Play(this.chargingSounds[UnityEngine.Random.Range(0, this.chargingSounds.Length)]);
        }

        yield return new WaitForSeconds(this.chargeTime);

        GameObject beamGO = GameObject.Instantiate(
            this.weaponDischargePrefab,
            GetNextWorldDischargeSpawnPoint(),
            this.transform.rotation,
            this.transform);

        if (this.weaponFireSounds.Length > 0)
        {
            AudioManager.Play(this.weaponFireSounds[UnityEngine.Random.Range(0, this.weaponFireSounds.Length)]);
        }

        Beam beam = beamGO.GetComponent<Beam>();

        yield return new WaitForSeconds(beam.TotalTime);

        this.animator.SetBool("ChargingBeam", false);
        this.animator.SetBool("Firing", false);

        this.lockMovement = false;
    }
}
