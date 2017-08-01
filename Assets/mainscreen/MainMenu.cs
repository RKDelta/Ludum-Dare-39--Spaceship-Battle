using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HideUI))]
public class MainMenu : MonoBehaviour
{
    private bool MainMenuOn = false;

    private HideUI hideUI;

    public HideUI[] otherUIElements;

    void Start()
    {
        this.MainMenuOn = false;

        this.hideUI = this.GetComponent<HideUI>();
    }

    private bool hasHidden = true;

    void Update()
    {
        if (this.MainMenuOn && this.hasHidden == true)
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
        else if (this.MainMenuOn == false && this.hasHidden == false)
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

    public void ActivateMainMenu()
    {
        this.MainMenuOn = true;
    }

    public void DeactivateMainMenu()
    {
        this.MainMenuOn = false;

        this.hideUI.Hide();
    }

    public void Resume()
    {
        this.DeactivateMainMenu();
        GameController.Instance.isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}