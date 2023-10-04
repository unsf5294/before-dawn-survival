using System.Collections;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private int attackDamage = 40;
    [SerializeField] private float attackRange = 5.0f; 
    [SerializeField] private float coneAngle = 45.0f;
    [SerializeField] private float RotationSpeed = 0.1f;
    private Animator animator;
    private bool isAttacking = false;
    private float attackAnimationDuration; 
    private int currentAttack = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleAttack();
        HandleShieldBash();
        HandleMovement();
    }

    void HandleMovement()
    {
        if (isAttacking) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        if (moveDirection.magnitude > 0f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, RotationSpeed);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void PerformAttack()
    {
        // Get all monsters
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            Vector3 toMonster = (monster.transform.position - transform.position).normalized;
            float angleToMonster = Vector3.Angle(transform.forward, toMonster);

            // Check if the monster is within the attack range
            if (Vector3.Distance(transform.position, monster.transform.position) <= attackRange && angleToMonster <= coneAngle)
            {
                MonsterHealth monsterHealth = monster.GetComponent<MonsterHealth>();
                if (monsterHealth)
                {
                    monsterHealth.TakeDamage(attackDamage);
                }
            }
        }
    }
void HandleAttack()
{
    if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
    {
        Debug.Log("Attack triggered: " + currentAttack);
        animator.SetBool("IsMoving", false);
        isAttacking = true;

        PerformAttack();

        if (currentAttack == 1)
        {
            animator.SetBool("Attack1", true);
            currentAttack++;
        }
        else if (currentAttack == 2)
        {
            animator.SetBool("Attack2", true);
            currentAttack++;
        }
        else if (currentAttack == 3)
        {
            animator.SetBool("Attack3", true);
            currentAttack = 1;  // Reset
        }
        StartCoroutine(ResetAttackAnimation());
    }
}


    IEnumerator ResetAttackAnimation()
    {
        if (currentAttack == 1)
        {
            attackAnimationDuration = 3.292f / 10.0f;
        }
        else
        {
            attackAnimationDuration = 5.375f / 10.0f;
        }
        yield return new WaitForSeconds(attackAnimationDuration);
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
        isAttacking = false;
    }

    void HandleShieldBash()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isAttacking)
        {
            isAttacking = true;
            PerformAttack();
            animator.SetBool("IsMoving", false);
            animator.SetBool("ShieldBash", true);
            StartCoroutine(ResetShieldBashAnimation());
        }
    }

    IEnumerator ResetShieldBashAnimation()
    {
        float shieldBashDuration = 3.292f / 10.0f; 
        yield return new WaitForSeconds(shieldBashDuration);
        animator.SetBool("ShieldBash", false);
        isAttacking = false;
    }
}
