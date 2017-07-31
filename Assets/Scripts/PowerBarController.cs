using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarController : MonoBehaviour
{
    public Image powerBarLiquid;
    public Player player;

    public Image powerBarCrack;

    public Text powerPercent;

    private void Start()
    {
        this.powerBarCrack.gameObject.SetActive(false);
    }

    void Update ()
    {
        float powerProp = this.player.PowerProportional;

        this.powerBarLiquid.fillAmount = powerProp;

        if (powerProp < 0.25)
        {
            this.powerBarCrack.gameObject.SetActive(true);
        }

        this.powerPercent.text = string.Format("{0}%", Mathf.RoundToInt(this.player.PowerProportional * 100));
    }
}
