using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    private bool victoryScreenOn = false;
    private float transition;

    private HideUI hideUI;

    public HideUI[] otherUIElements;

    private bool hasHidden = true;

    void Start()
    {
        this.victoryScreenOn = false;

        this.hideUI = this.GetComponent<HideUI>();

        this.hideUI.Hide();
    }

    void Update()
    {
        if (this.victoryScreenOn && this.hasHidden == true)
        {
            this.hideUI.Show();

            if (this.otherUIElements != null)
            {
                foreach (HideUI other in this.otherUIElements)
                {
                    other.Hide();
                }
            }

            this.hasHidden = false;
        }
        else if (this.victoryScreenOn == false && this.hasHidden == false)
        {
            this.hideUI.Hide();

            if (this.otherUIElements != null)
            {
                foreach (HideUI other in this.otherUIElements)
                {
                    other.Show();
                }
            }

            this.hasHidden = true;
        }
    }

    public void ActivateVictoryScreen()
    {
        this.victoryScreenOn = true;
    }

    public void DeactivateVictoryScreen()
    {
        this.victoryScreenOn = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
