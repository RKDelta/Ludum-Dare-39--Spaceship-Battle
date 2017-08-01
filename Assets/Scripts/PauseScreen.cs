using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HideUI))]
public class PauseScreen : MonoBehaviour
{
    private bool pauseScreenOn = false;

    private HideUI hideUI;

    public HideUI[] otherUIElements;

    public Animator animator;

    public Player player;

    void Start()
    {
        this.pauseScreenOn = false;

        this.hideUI = this.GetComponent<HideUI>();

        this.hideUI.Hide();
    }

    private bool hasHidden = true;

    void Update()
    {
        if (this.pauseScreenOn && this.hasHidden == true)
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
        else if(this.pauseScreenOn == false && this.hasHidden == false)
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
        
        switch (this.player.controlMode)
        {
            case ControlMode.MouseRotation:
                this.animator.SetBool("Absolute", false);
                this.animator.SetBool("KeyboardRotation", false);
                this.animator.SetBool("MouseRotation", true);
                break;
            case ControlMode.KeyboardRotation:
                this.animator.SetBool("Absolute", false);
                this.animator.SetBool("KeyboardRotation", true);
                this.animator.SetBool("MouseRotation", false);
                break;
            case ControlMode.Absolute:
                this.animator.SetBool("Absolute", true);
                this.animator.SetBool("KeyboardRotation", false);
                this.animator.SetBool("MouseRotation", false);
                break;
        }
    }

    public void ActivatePauseScreen()
    {
        this.pauseScreenOn = true;
    }

    public void DeactivatePauseScreen()
    {
        this.pauseScreenOn = false;
    }

    public void Resume()
    {
        this.DeactivatePauseScreen();
        GameController.Instance.isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeControls()
    {
        this.player.controlMode = (ControlMode)(((int)this.player.controlMode + 1) % 3);
    }
}