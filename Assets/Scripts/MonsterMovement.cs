using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 2.0f;
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
    private PlayerHealth playerHealth;
    private bool inactive;
   

    private void Start()
    {
        inactive = false;
        animator = GetComponent<Animator>();
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (!inactive)
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

            if (hasCollided && !isAttacking) 
            {
                HandleAttack();
            }           
        }
    }

    public IEnumerator pushTo(Vector3 Destination)
    {
        Vector3 Origin = transform.position;
        float totalMovementTime = 4f; //the amount of time you want the movement to take
        float currentMovementTime = 0f;//The amount of time that has passed
        inactive = true;
        while (Vector3.Distance(transform.localPosition, Destination) > 0)
        {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(Origin, Destination, currentMovementTime / totalMovementTime);
            yield return null;
        }
        inactive = false;
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
    }
}


private void OnCollisionExit(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        hasCollided = false;
    }
}

private void HandleAttack()
{

    isAttacking = true;

    animator.SetBool("isAttack", true);

    StartCoroutine(ResetAttackAnimation());
    Debug.Log(isAttacking);
    if (playerHealth)
    {
        playerHealth.TakeDamage(damage);
    }   
    lastAttackTime = Time.time;

}

IEnumerator ResetAttackAnimation()
{
    yield return new WaitForSeconds(attackCooldown);
    animator.SetBool("isAttack", false);
    isAttacking = false;
}


}

