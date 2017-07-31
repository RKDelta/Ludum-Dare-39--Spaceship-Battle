using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    public GameObject starParticlesPrefab;

    public CameraRig cameraRig;

    private GameObject[,] starParticles;
    
    public float starsWidth = 26;

    void Start()
    {
        if (this.cameraRig == null)
        {
            this.cameraRig = Camera.main.GetComponentInParent<CameraRig>();
        }

        this.starParticles = new GameObject[2, 2];

        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                this.starParticles[x, y] = Instantiate(this.starParticlesPrefab, this.transform);
            }
        }
    }

    void Update()
    {
        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                Vector2 newPos =
                    new Vector2(
                        (Mathf.Floor(this.cameraRig.transform.position.x / this.starsWidth) + x) * this.starsWidth,
                        (Mathf.Floor(this.cameraRig.transform.position.y / this.starsWidth) + y) * this.starsWidth);

                if (newPos != (Vector2)this.starParticles[x, y].transform.position)
                {
                    this.starParticles[x, y].transform.position = newPos;
                }
            }
        }
    }
}
