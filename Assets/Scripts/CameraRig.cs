using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] private Camera _spriteCamera;
    [SerializeField] private Camera _effectCamera;

    public Camera SpriteCamera { get; protected set; }
    public Camera EffectCamera { get; protected set; }

    public static CameraRig Instance { get; protected set; }

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are two CameraRigs in the scene. There should be only one CameraRig.");
        }

        this.SpriteCamera = this._spriteCamera;
        this.EffectCamera = this._effectCamera;

        Instance = this;
    }

    public static Vector2 GetWorldMousePosition()
    {

        Ray ray = Instance.SpriteCamera.ScreenPointToRay(Input.mousePosition);
        Plane z0Plane = new Plane(Vector3.back, Vector3.zero);

        float distance;

        if (z0Plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector2.zero;
    }
}