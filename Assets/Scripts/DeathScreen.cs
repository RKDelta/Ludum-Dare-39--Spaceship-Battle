using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(HideUI))]
public class DeathScreen : MonoBehaviour
{
    public Text scoreText;
    private bool deathScreenOn = false;
    private float transition;

    private HideUI hideUI;

    public HideUI[] otherUIElements;

	void Start ()
    {
        this.deathScreenOn = false;

        this.hideUI = this.GetComponent<HideUI>();

        this.hideUI.Hide();
    }
	
	void Update ()
    {
        if (this.deathScreenOn)
        {
            this.hideUI.Show();

            if (this.otherUIElements != null)
            {
                foreach (HideUI other in this.otherUIElements)
                {
                    other.Hide();
                }
            }
        }
        else
        {
            this.hideUI.Hide();

            if (this.otherUIElements != null)
            {
                foreach (HideUI other in this.otherUIElements)
                {
                    other.Show();
                }
            }
        }
    }

    public void ActivateDeathScreen(float score)
    {
        this.deathScreenOn = true;
        this.scoreText.text = ((int)score).ToString(); 
    }

    public void DeactivateDeathScreen()
    {
        this.deathScreenOn = false;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
