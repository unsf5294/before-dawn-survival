using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackCooldown = 2.0f;

    private float lastAttackTime = -2.0f;

    private bool hasCollided = false;

    private void Update()
    {
        if (!hasCollided)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Tracking position
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y + 1, player.position.z);

        // Get direction
        Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
        
        // Move
        transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
        
        // Face the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the collided
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true;
            
            // Push monster away


            if (Time.time - lastAttackTime >= attackCooldown)
            {
                // Get the player health script and deal damage
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth)
                {
                    playerHealth.TakeDamage(damage);
                    lastAttackTime = Time.time; // Update the last attack time
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollided = false;
        }
    }
}
