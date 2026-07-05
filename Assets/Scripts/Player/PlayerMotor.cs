using UnityEngine;

// Physics-based player movement (M0). Replaces the transform-write movement that used
// to live in CharacterControl.HandleMovement and fought the Rigidbody solver. Reads
// InputReader.Move (camera-relative) and drives the Rigidbody's velocity in
// FixedUpdate. Attack/ability logic still lives in CharacterControl for now; it is
// split into dedicated components in M1.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [Range(0f, 1f)]
    [SerializeField] private float rotationLerp = 0.15f;

    // Timed multiplier hook for the speed-buff ability (driven by CharacterControl).
    public float SpeedMultiplier { get; set; } = 1f;
    // Set false to halt movement (e.g. while attacking).
    public bool CanMove { get; set; } = true;

    private Rigidbody rb;
    private Animator animator;
    private Camera cam;
    private Vector3 desiredDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Allow yaw (facing) but never tip over — replaces the angularDrag = 999999 hack.
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.angularDrag = 0.05f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        animator = GetComponent<Animator>();

        int playerLayer = LayerMask.NameToLayer("Player");
        if (playerLayer >= 0) gameObject.layer = playerLayer;
    }

    private void Start() => cam = Camera.main;

    private void Update()
    {
        if (cam == null) cam = Camera.main;
        Vector2 move = CanMove ? InputReader.Move : Vector2.zero;

        // Flatten camera axes so movement is camera-relative on the ground plane.
        Vector3 camFwd = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(cam.transform.right, new Vector3(1, 0, 1)).normalized;
        desiredDir = move.y * camFwd + move.x * camRight;
        if (desiredDir.sqrMagnitude > 1f) desiredDir.Normalize();

        animator.SetBool("IsMoving", desiredDir.sqrMagnitude > 0.0001f);
    }

    private void FixedUpdate()
    {
        Vector3 target = desiredDir * (moveSpeed * SpeedMultiplier);
        Vector3 v = rb.velocity;
        v.x = target.x;
        v.z = target.z; // preserve y for gravity
        rb.velocity = v;

        if (desiredDir.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(desiredDir, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, look, rotationLerp));
        }
    }
}
