using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    private void Start()
    {
        currentHealth = maxHealth; 
        StartCoroutine(DecreaseHealthOverTime());
    }

    private IEnumerator DecreaseHealthOverTime()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(3f); // Wait for 3 seconds
            TakeDamage(1); // Decrease by one point
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Lost 1 health point. Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        Destroy(this.gameObject);
    }
}
