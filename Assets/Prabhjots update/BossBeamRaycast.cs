using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("BossBeamRaycast is deprecated, please use Beam instead.", true)]
public class BossBeamRaycast : MonoBehaviour
{
    public float damage = 500f;
    public LayerMask canHit;
    public Player player;

    void Start()
    {

    }


    void Update()
    {
        RaycastHit2D hit_info;
        
        hit_info = Physics2D.Raycast(this.transform.position, this.transform.up, 10000, this.canHit);
        if (hit_info.collider != null)
        {
            Player player = hit_info.collider.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log(hit_info.collider.name);
                player.DoDamage(this.damage * Time.deltaTime);
                Debug.Log("did damage");
            }
        }
    }
}