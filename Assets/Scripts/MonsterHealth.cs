using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int playerHealing = 10;
    [SerializeField] private ParticleSystem healingEffect;

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
        StartCoroutine(PlaySound());

        if (currentHealth <= 0)
        {
           Die();
        }
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<AudioSource>().Play();
    }

    private void Die()
    {
        var particles = Instantiate(this.healingEffect);
        particles.transform.position = transform.position;
        
        // heal after short duration to match the healing particle
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.AddHealthWithDelay(playerHealing, 2.5f);
        Destroy(this.gameObject);
    }
}