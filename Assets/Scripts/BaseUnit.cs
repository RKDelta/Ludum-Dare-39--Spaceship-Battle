using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseUnit : MonoBehaviour
{
    public abstract void DoDamage(float damageAmount);

    protected Rigidbody2D rb;

    public float movementSpeed = 10;
    public float rotationSpeed = 0.5f;

    public void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();

        if (this.rb == null)
        {
            Debug.LogError("Unit does not have Rigidbody!");
        }

        this.rb.gravityScale = 0;
    }

    public void Move(Vector2 direction)
    {
        this.rb.AddRelativeForce(direction * movementSpeed);
    }

    public void Rotate(float torque)
    {
        this.rb.AddTorque(torque * rotationSpeed);
    }
}
