using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DirectionalMovementSpeed
{
    public float forward;
    public float left;
    public float back;
    public float right;

    public float rotational;

    public static readonly DirectionalMovementSpeed normal = new DirectionalMovementSpeed()
    {
        forward = 10,
        left = 2,
        back = 4,
        right = 2,

        rotational = 1.5f
    };

    public static Vector3 operator *(Vector3 movement, DirectionalMovementSpeed speed)
    {
        if (movement.y > 0)
        {
            movement.y *= speed.forward;
        }
        else if (movement.y < 0)
        {
            movement.y *= speed.back;
        }

        if (movement.x > 0)
        {
            movement.x *= speed.right;
        }
        else if (movement.x < 0)
        {
            movement.x *= speed.left;
        }

        return movement;
    }
}

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseUnit : MonoBehaviour
{
    public abstract void DoDamage(float damageAmount);

    protected Rigidbody2D rb;

    public DirectionalMovementSpeed movementSpeed = DirectionalMovementSpeed.normal;

    protected virtual void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();

        if (this.rb == null)
        {
            Debug.LogError("Unit does not have Rigidbody!");
        }

        this.rb.gravityScale = 0;
    }

    protected void Move(Vector2 direction)
    {


        this.rb.AddRelativeForce(direction * movementSpeed);
    }

    protected void Rotate(float torque)
    {
        this.rb.AddTorque(torque * movementSpeed.rotational);
    }
}
