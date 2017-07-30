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

    public Rigidbody2D target;

    public Vector2 velocity;

    public float mouseZoomSensitivity = 1f;

    public float minZoom = 2;
    public float maxZoom = 15;
    private float currentZoom = 5;

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

    public void FixedUpdate()
    {
        Vector2 targetVelocity = this.target.velocity;

        this.velocity = Vector2.Lerp(this.velocity, targetVelocity, 0.15f);

        this.transform.position += (Vector3)this.velocity * Time.fixedDeltaTime;
    }

    public void Update()
    {
        Vector2 targetPosition = this.target.transform.position;

        this.transform.position = Vector2.Lerp(this.transform.position, targetPosition, 0.75f * Time.deltaTime);

        Vector3 position = this.transform.position;
        position.z = -10;
        this.transform.position = position;

        this.currentZoom += -Input.GetAxis("Mouse ScrollWheel") * this.mouseZoomSensitivity * this.currentZoom;

        this.currentZoom = Mathf.Clamp(this.currentZoom, this.minZoom, this.maxZoom);

        this.EffectCamera.orthographicSize = this.currentZoom;
        this.SpriteCamera.orthographicSize = this.currentZoom;
    }
}