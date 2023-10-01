using UnityEngine;

public class MonsterHealth : MonoBehaviour
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

        // No negative
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Monster took damage : " + damage + "HP left : " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Monster has died!");
        Destroy(this.gameObject);
    }
}
