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

    private bool hasHidden = true;

	void Start ()
    {
        this.deathScreenOn = false;

        this.hideUI = this.GetComponent<HideUI>();

        this.hideUI.Hide();
    }
	
	void Update ()
    {
        if (this.deathScreenOn && this.hasHidden == true)
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
        else if (this.deathScreenOn == false && this.hasHidden == false)
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
