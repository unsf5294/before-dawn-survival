using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private int attackDamage = 40;
    [SerializeField] private float attackRange = 5.0f; 
    [SerializeField] private float coneAngle = 45.0f;
    [SerializeField] private float RotationSpeed = 0.1f;
    [SerializeField] private UnityEvent Ability1;
    [SerializeField] private UnityEvent Ability2;
    [SerializeField] private UnityEvent Ability3;
    [SerializeField] private float AbilityCooldown = 10;
    [SerializeField] private SkillAvailabilityUI skillUI;

    private enum AbilityType { None, Ability1, Ability2, Ability3 }
    private bool[] hasAbility = new bool[4];  // Index 0 is unused for simplicity
    // private bool[] hasAbility = new bool[4] { true, true, true, true }; // only for debug

    private int baseAttackDamage;
    private float baseMoveSpeed;
    private float CurrentCD;
    private Animator animator;
    private bool isAttacking = false;
    private float attackAnimationDuration; 
    private int currentAttack = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        baseAttackDamage = attackDamage;
        baseMoveSpeed = moveSpeed;
        StartCoroutine(GrantAbilitiesOverTime());
    }

    void Update()
    {
        HandleAttack();
        HandleShieldBash();
        HandleMovement();
        HandleAbility();
    }

    void HandleMovement()
    {
        if (isAttacking) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Get camera forward and right vectors, with no vertical component
        Vector3 camFwd = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

        // Adjust move direction relative to camera orientation
        Vector3 moveDirection = vertical * camFwd + horizontal * camRight;

        if (moveDirection.magnitude > 1.0f)
        {
            moveDirection = moveDirection.normalized;
        }

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
        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Mouse0)) && !isAttacking)
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
        if ((Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Mouse1)) && !isAttacking)
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

    IEnumerator GrantAbilitiesOverTime()
    {
        yield return new WaitForSeconds(10);  // Wait for 10s
        GrantRandomAbility();
        yield return new WaitForSeconds(30);  // Wait for another 30s
        GrantRandomAbility();
        yield return new WaitForSeconds(60);  // Wait for another minute
        GrantRandomAbility();
    }

    void GrantRandomAbility()
    {
        AbilityType randomAbility;
        do
        {
            randomAbility = (AbilityType)Random.Range(1, 4);  // Random ability between 1 to 3
        } while (hasAbility[(int)randomAbility]);  // Ensure the ability is not already granted

        hasAbility[(int)randomAbility] = true;
        skillUI.SetSkillUnavailable((int)randomAbility, false);
        skillUI.ShowSkillNotification((int)randomAbility-1);
    }

    void HandleAbility()
    {
        if (CurrentCD > 0) return;  // If cooldown is still active, return

        if (Input.GetKeyDown(KeyCode.Alpha1) && hasAbility[(int)AbilityType.Ability1])
        {
            // Ability 1 logic 
            Ability1.Invoke();
            StartCoroutine(Cooldown());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && hasAbility[(int)AbilityType.Ability2])
        {
            // Ability 2 logic
            StartCoroutine(BoostAttackDamage());
            Ability2.Invoke();
            StartCoroutine(Cooldown());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && hasAbility[(int)AbilityType.Ability3])
        {
            // Ability 3 logic
            StartCoroutine(BoostMoveSpeed());
            Ability3.Invoke();
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator BoostAttackDamage()
    {
        attackDamage *= 2;
        yield return new WaitForSeconds(10);  // Wait for 10 seconds
        attackDamage = baseAttackDamage;  // Restore the original attack damage
    }

    IEnumerator BoostMoveSpeed()
    {
        moveSpeed *= 1.5f;
        yield return new WaitForSeconds(10);  // Wait for 10 seconds
        moveSpeed = baseMoveSpeed;  // Restore the original move speed
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(AbilityCooldown);
        AbilityCooldown = 0;
    }
}
