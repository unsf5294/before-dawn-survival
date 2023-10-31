using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private UnityEvent onDeath;
    [SerializeField] private ParticleSystem healReceiveEffect;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioSource audioSource;
    private bool playingSound = false;
    private int currentHealth;

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public void setHealth(int health)
    {
        currentHealth = health;
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
        currentHealth = Mathf.Clamp(currentHealth, -1, maxHealth);
        Debug.Log("Lost health: " + damage + ". Current HP: " + currentHealth);

        if (damage >= 5 && playingSound == false)
        {
            StartCoroutine(playSound());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator playSound()
    {
        playingSound = true;
        audioSource.PlayOneShot(hurtSound);
        yield return new WaitForSeconds(0.1f);
        playingSound = false;
    }

    public void AddHealth(int healthAmount)
    {
        currentHealth += healthAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        var particles = Instantiate(this.healReceiveEffect);
        particles.transform.position = transform.position;
        Debug.Log("Gained health: " + healthAmount + ". Current HP: " + currentHealth);
    }

    // for effect after killing monster
    public void AddHealthWithDelay(int healthAmount, float delay)
    {
        StartCoroutine(DelayedAddHealth(healthAmount, delay));
    }

    private IEnumerator DelayedAddHealth(int healthAmount, float delay)
    {
        yield return new WaitForSeconds(delay);
        AddHealth(healthAmount);
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        this.onDeath.Invoke();
        Destroy(this.gameObject);
    }
}
