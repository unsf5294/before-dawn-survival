using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // No negative HP
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("You took damage : " + damage + "HP left : " + currentHealth);

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
