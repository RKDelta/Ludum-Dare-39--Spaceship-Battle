using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleToScreenResolution : MonoBehaviour
{
    public int baseHeight;
    public int baseWidth;

    RectTransform rt;

    //public Canvas canvas;

    private void Start()
    {
        this.rt = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        Debug.Log(Screen.height + "," + this.baseHeight * 3);

        if (Screen.height >= this.baseHeight * 3)
        {
            this.rt.sizeDelta = new Vector2(3 * this.baseWidth, 3 * this.baseHeight);
        }
        else if (Screen.height >= this.baseHeight * 2)
        {
            this.rt.sizeDelta = new Vector2(2 * this.baseWidth, 2 * this.baseHeight);
        }
        else
        {
            this.rt.sizeDelta = new Vector2(this.baseWidth, this.baseHeight);
        }
    }
}
