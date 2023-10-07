using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private float trackRange = 10.0f; // Range within which monster starts tracking the player

    private float lastAttackTime = -2.0f;
    private float lastDirectionChangeTime = 0f;
    private float directionChangeInterval = 2f;
    private Vector3 currentMoveDirection;

    private bool hasCollided = false;
    private bool isAttacking = false;

    private void Start()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= trackRange && !hasCollided)
        {
            MoveTowardsPlayer();
        }
        else
        {
            RandomMovement();
        }

    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    void MoveTowardsPlayer()
    {
        // Tracking position
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, player.position.z);
        Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
        transform.position += directionToPlayer * moveSpeed * Time.deltaTime;

        // Face the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    void RandomMovement()
    {
        if (Time.time - lastDirectionChangeTime > directionChangeInterval)
        {
            lastDirectionChangeTime = Time.time;
            currentMoveDirection = ChooseRandomDirection();
        }
        transform.position += currentMoveDirection * moveSpeed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(currentMoveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    Vector3 ChooseRandomDirection()
    {
        int randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0: return transform.forward;
            case 1: return -transform.forward; // backward
            case 2: return transform.right;
            case 3: return -transform.right; // left
            default: return Vector3.zero;
        }
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
