using System.Collections;
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
    private Animator animator;
    private bool hasCollided = false;
    private bool isAttacking = false;
   

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (!hasCollided & player)
        {
            if (Vector3.Distance(player.position, transform.position) <= trackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                RandomMovement();
            }
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
        transform.position += currentMoveDirection * (moveSpeed / 2) * Time.deltaTime;

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
        if (!isAttacking)
        {
            StartCoroutine(HandleAttack(collision));
        }
    }
}

private void OnCollisionExit(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        hasCollided = false;
        StopCoroutine(HandleAttack(collision)); // Stop the attack sequence
        isAttacking = false;
    }
}

IEnumerator HandleAttack(Collision collision)
{
    while (hasCollided) // Keep attacking as long as there's a collision
    {
        isAttacking = true;

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            animator.SetBool("isAttack", true);            

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth)
            {
                playerHealth.TakeDamage(damage);
            }
            animator.SetBool("isAttack", false);

            lastAttackTime = Time.time; 
        }
        yield return null; // Wait for next frame
    }
    isAttacking = false;
}
}

