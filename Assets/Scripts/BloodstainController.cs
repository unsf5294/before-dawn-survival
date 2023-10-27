using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodstainController : MonoBehaviour
{
    public PlayerHealth playerHealth; 
    public Image bloodstainImage; 

    private void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth reference is not set!");
            return;
        }

        if (bloodstainImage == null)
        {
            Debug.LogError("Bloodstain Image reference is not set!");
            return;
        }

        // Set the initial alpha of the bloodstain image
        SetBloodstainAlpha(10f / 255f);
    }

    private void Update()
    {
        UpdateBloodstainAlpha();
    }

    void UpdateBloodstainAlpha()
    {
        float healthPercentage = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;

        if (healthPercentage >= 0.75f)
        {
            SetBloodstainAlpha(10f / 255f);
        }
        else if (healthPercentage >= 0.50f)
        {
            SetBloodstainAlpha(25f / 255f);
        }
        else if (healthPercentage >= 0.25f)
        {
            SetBloodstainAlpha(50f / 255f);
        }
        else
        {
            SetBloodstainAlpha(100f / 255f);
        }
    }

    void SetBloodstainAlpha(float alpha)
    {
        Color currentColor = bloodstainImage.color;
        bloodstainImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }
}