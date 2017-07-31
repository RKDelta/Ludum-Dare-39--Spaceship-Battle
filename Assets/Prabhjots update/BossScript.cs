using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("BossScript is deprecated. Please use Boss instead.", true)]
public class BossScript: MonoBehaviour
{
    public GameObject boss_bee;
    public float speed = 2;
    public GameObject player;
    private Rigidbody2D player_rb;
    private Rigidbody2D boss_rb;
    private Vector3 move;

    void Start()
    {
        this.boss_rb = GetComponent<Rigidbody2D>();
        this.player_rb = this.player.GetComponent<Rigidbody2D>();

    }
    
    void Update()
    {
        this.move = new Vector3(-1, 0, 0);

        if (this.boss_rb.transform.position.x > 0)
        {
            this.boss_rb.transform.position += this.move * this.speed * Time.deltaTime;
        }
    }
}
