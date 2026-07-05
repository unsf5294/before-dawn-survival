using System.Collections;
using UnityEngine;

// Enemy melee attack on contact (M0). Movement moved to EnemyMotor; this now only
// handles colliding with the player and dealing damage. Replaced wholesale by
// EnemyBrain (telegraphed attacks) in M1.
public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackCooldown = 2.0f;

    private Animator animator;
    private bool hasCollided = false;
    private bool isAttacking = false;
    private PlayerHealth playerHealth;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!player)
        {
            var found = GameObject.FindGameObjectWithTag("Player");
            if (found) player = found.transform;
        }
        if (player) playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (hasCollided && !isAttacking)
        {
            HandleAttack();
        }
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
        playerHealth = playerTransform ? playerTransform.GetComponent<PlayerHealth>() : null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) hasCollided = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) hasCollided = false;
    }

    private void HandleAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttack", true);
        StartCoroutine(ResetAttackAnimation());
        if (playerHealth)
        {
            StartCoroutine(DamageDelay(0.5f));
        }
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(attackCooldown);
        animator.SetBool("isAttack", false);
        isAttacking = false;
    }

    private IEnumerator DamageDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (playerHealth) playerHealth.TakeDamage(damage);
    }
}
