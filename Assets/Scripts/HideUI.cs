using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class HideUI : MonoBehaviour
{
    public float baseAlpha = 1;

    public bool hidden;

    public float alphaMultiplier;

    CanvasGroup group;

    private void Awake()
    {
        this.hidden = false;

        this.group = this.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (this.hidden)
        {
            this.alphaMultiplier = Mathf.Clamp01(this.alphaMultiplier - Time.deltaTime * 8);

            this.group.blocksRaycasts = false;
            this.group.interactable = false;
        }
        else
        {
            this.alphaMultiplier = Mathf.Clamp01(this.alphaMultiplier + Time.deltaTime * 8);

            this.group.blocksRaycasts = true;
            this.group.interactable = true;
        }

        this.group.alpha = this.alphaMultiplier * this.baseAlpha;
    }

    public void Show()
    {
        this.hidden = false;
    }

    public void Hide()
    {
        this.hidden = true;
    }
}
