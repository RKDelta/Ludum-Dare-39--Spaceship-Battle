using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct SpawnPoint
{
    public Vector2 position;

    public Vector3 eulerRotation;

    public Quaternion Rotation
    {
        get
        {
            return Quaternion.Euler(this.eulerRotation);
        }
    }

    public void DrawGizmo(Transform parent)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay((Vector2)parent.position + this.position, parent.rotation * this.Rotation * Vector2.up * 0.25f);
    }
}
