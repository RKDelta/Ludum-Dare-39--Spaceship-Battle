using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarController : MonoBehaviour
{
    public GameObject bar;

    public GameObject skull;
    public GameObject skullRed;
    public GameObject skullScream;

    [HideInInspector] public Boss boss;

    void Start()
    {
        this.skullScream.gameObject.SetActive(false);
        this.skullRed.gameObject.SetActive(false);
        this.skull.gameObject.SetActive(false);
        this.skull.gameObject.SetActive(true);
    }

    void Update()
    {
        if (this.boss.HealthProprtional > 0.25 && this.boss.HealthProprtional < 0.5)
        {
            this.skullRed.gameObject.SetActive(true);
            this.skull.gameObject.SetActive(false);
        }
        else if (this.boss.HealthProprtional < 0.25)
        {
            this.skullScream.gameObject.SetActive(true);
            this.skullRed.gameObject.SetActive(false);
        }

        this.bar.transform.localScale = new Vector2(this.boss.HealthProprtional, 1);
    }
}
