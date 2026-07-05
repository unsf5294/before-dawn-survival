using UnityEngine;

// Physics-based enemy movement + light steering (M0). Replaces MonsterMovement's
// transform-write movement (which fought the solver and let enemies stack). Seeks the
// player within trackRange, separates from nearby enemies, and wanders when out of
// range; all via rb.velocity in FixedUpdate. Attack still lives in MonsterMovement for
// now (both are replaced by EnemyBrain in M1).
[RequireComponent(typeof(Rigidbody))]
public class EnemyMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float trackRange = 10f;
    [SerializeField] private float separationRadius = 1.5f;
    [SerializeField] private float separationWeight = 1.5f;

    private Rigidbody rb;
    private Transform player;
    private int enemyMask;
    private readonly Collider[] neighbors = new Collider[8];

    private Vector3 wanderDir;
    private float nextWanderTime;
    private float knockbackUntil;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.angularDrag = 0.05f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        enemyMask = LayerMask.GetMask("Enemy");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if (enemyLayer >= 0) gameObject.layer = enemyLayer;
    }

    private void Start() => AcquirePlayer();

    private void FixedUpdate()
    {
        // While knocked back, let the impulse carry — don't override velocity.
        if (Time.time < knockbackUntil) return;
        if (!player && !AcquirePlayer()) return;

        Vector3 desired;
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist <= trackRange)
        {
            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;
            desired = toPlayer.normalized;
        }
        else
        {
            desired = Wander();
        }

        desired += Separation() * separationWeight;
        desired.y = 0f;
        if (desired.sqrMagnitude > 1f) desired.Normalize();

        Vector3 v = rb.velocity;
        Vector3 target = desired * moveSpeed;
        v.x = target.x;
        v.z = target.z; // preserve y for gravity
        rb.velocity = v;

        if (desired.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(desired, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, look, Time.fixedDeltaTime * 5f));
        }
    }

    private bool AcquirePlayer()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
        return player != null;
    }

    // Steer away from crowding neighbors so enemies don't pile onto one point.
    private Vector3 Separation()
    {
        Vector3 push = Vector3.zero;
        int n = Physics.OverlapSphereNonAlloc(transform.position, separationRadius, neighbors, enemyMask);
        for (int i = 0; i < n; i++)
        {
            if (neighbors[i].transform == transform) continue;
            Vector3 away = transform.position - neighbors[i].transform.position;
            away.y = 0f;
            float d = away.magnitude;
            if (d > 0.001f) push += away / (d * d); // stronger the closer they are
        }
        return push;
    }

    private Vector3 Wander()
    {
        if (Time.time >= nextWanderTime)
        {
            nextWanderTime = Time.time + 2f;
            Vector2 r = Random.insideUnitCircle.normalized;
            wanderDir = new Vector3(r.x, 0f, r.y);
        }
        return wanderDir * 0.5f;
    }

    /// <summary>Shove this enemy away with a velocity impulse (player's Repulsion ability).</summary>
    public void Knockback(Vector3 direction, float force)
    {
        direction.y = 0f;
        rb.velocity = Vector3.zero;
        rb.AddForce(direction.normalized * force, ForceMode.VelocityChange);
        knockbackUntil = Time.time + 0.4f;
    }
}
