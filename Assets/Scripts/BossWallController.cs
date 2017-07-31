using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWallController : MonoBehaviour
{
    public float width = 10;
    public float height = 10;

    public GameObject wallCornerPrefab;
    public GameObject wallBeamPrefab;

    void Start()
    {
        GameObject.Instantiate(
            this.wallCornerPrefab,
            this.transform.position + new Vector3(this.width / 2, this.height / 2, -3),
            Quaternion.Euler(0, 0, -45),
            this.transform);
        GameObject.Instantiate(
            this.wallCornerPrefab,
            this.transform.position + new Vector3(this.width / 2, -this.height / 2, -3),
            Quaternion.Euler(0, 0, -135),
            this.transform);
        GameObject.Instantiate(
            this.wallCornerPrefab,
            this.transform.position + new Vector3(-this.width / 2, this.height / 2, -3),
            Quaternion.Euler(0, 0, 45),
            this.transform);
        GameObject.Instantiate(
            this.wallCornerPrefab,
            this.transform.position + new Vector3(-this.width / 2, -this.height / 2, -3),
            Quaternion.Euler(0, 0, 135),
            this.transform);

        GameObject.Instantiate(
            this.wallBeamPrefab,
            this.transform.position + new Vector3(this.width / 2, 0, -2),
            Quaternion.Euler(0, 0, 90),
            this.transform)
            .transform.localScale = new Vector2(this.height, 1); ;
        GameObject.Instantiate(
            this.wallBeamPrefab,
            this.transform.position + new Vector3(-this.width / 2, 0, -2),
            Quaternion.Euler(0, 0, 270),
            this.transform)
            .transform.localScale = new Vector2(this.height, 1);
        GameObject.Instantiate(
            this.wallBeamPrefab,
            this.transform.position + new Vector3(0, this.height / 2, 0),
            Quaternion.Euler(0,0,0),
            this.transform)
            .transform.localScale = new Vector2(this.width, 1); ;
        GameObject.Instantiate(
            this.wallBeamPrefab,
            this.transform.position + new Vector3(0, -this.height / 2, 0),
            Quaternion.Euler(0, 0, 180),
            this.transform)
            .transform.localScale = new Vector2(this.width, 1); ;
    }
    
    void Update()
    {

    }
}
