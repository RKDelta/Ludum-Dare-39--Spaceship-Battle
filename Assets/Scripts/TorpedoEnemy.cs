using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class TorpedoEnemy : Enemy
{
    public float timeBetweenTorpedos = 0.25f;

    protected override void FireWeapon()
    {
        this.StartCoroutine(this.TorpedoFireHandler());        
    }

    IEnumerator TorpedoFireHandler()
    {
        this.weaponTimeTillNextAllowed = this.weaponCooldown;

        this.lockMovement = true;

        yield return null;

        for (int i = 0; i < this.dischargeSpawnPoints.Length; i++)
        {
            Vector3 point
                = this.transform.position + this.transform.rotation * this.dischargeSpawnPoints[i].position;

            GameObject torpedoGo =
                GameObject.Instantiate(
                    this.weaponDischargePrefab,
                    point,
                    this.transform.rotation /** this.dischargeSpawnPoints[i].Rotation*/);

            Rigidbody2D torpedoRb = torpedoGo.GetComponent<Rigidbody2D>();

            torpedoRb.AddForce(this.transform.rotation * this.dischargeSpawnPoints[i].Rotation * Vector2.up * 5, ForceMode2D.Impulse);

            Torpedo torpedo = torpedoGo.GetComponent<Torpedo>();

            torpedo.target = this.target.transform;

            if (this.weaponFireSounds.Length > 0)
            {
                AudioManager.Play(this.weaponFireSounds[UnityEngine.Random.Range(0, this.weaponFireSounds.Length)]);
            }

            yield return new WaitForSeconds(this.timeBetweenTorpedos);
        }

        yield return new WaitForSeconds(this.timeBetweenTorpedos);

        this.lockMovement = false;
    }
}
