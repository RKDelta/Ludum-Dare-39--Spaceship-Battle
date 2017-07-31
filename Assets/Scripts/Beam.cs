using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public GameObject beam;
    public GameObject endCap;

    public float maxLength = 20.0f;

    public float timeTillMaxLength = 2.0f;

    float growthRate;

    public float timeAtMaxLength = 1.0f;

    public float TotalTime
    {
        get
        {
            return this.timeAtMaxLength + this.timeTillMaxLength;
        }
    }

    public float damagesPS = 50.0f;

    float timeSinceCreation;

    public LayerMask canHit;

    public float force = 20.0f;

    void Start()
    {
        this.timeSinceCreation = 0;

        this.growthRate = this.maxLength / this.timeTillMaxLength;

        Vector3 pos = this.transform.position;
        pos.z = 2;
        this.transform.position = pos;
    }

    void Update()
    {
        this.timeSinceCreation += Time.deltaTime;

        if (this.timeSinceCreation > this.TotalTime)
        {
            Destroy(this.gameObject);
            return;
        }

        float currentLength = Mathf.Clamp(this.growthRate * this.timeSinceCreation, 0, this.maxLength);

        RaycastHit2D result = Physics2D.Raycast(this.transform.position, this.transform.up, currentLength, this.canHit);

        if (result.collider != null)
        {
            BaseUnit unit = result.collider.GetComponent<BaseUnit>();

            if (unit != null)
            {
                unit.DoDamage(this.damagesPS * Time.deltaTime);
            }

            Rigidbody2D rb = result.collider.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForceAtPosition(this.transform.up * this.force * Time.deltaTime, this.transform.position);
            }

            this.SetLength(result.distance);
        }
        else
        {
            this.SetLength(currentLength);
        }
    }

    void SetLength(float length)
    {
        this.beam.transform.localScale =
            new Vector3(
                1,
                length,
                1);

        this.beam.transform.localPosition =
            new Vector3(
                0,
                length * 0.5f,
                0);

        this.endCap.transform.localPosition =
            new Vector3(
                0,
                length,
                0);
    } 
}
