using UnityEngine;

public class ParticleMover : MonoBehaviour
{
    [SerializeField] private float attractionSpeed = 15.0f;
    [SerializeField] private float delayBeforeAttracting = 2.0f;

    private Transform playerTransform;
    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;
    private float timer;
    private Vector3 playerHeight = new Vector3(0.0f, 1.8f, 0.0f);
    private bool isAttracting = false;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        timer = delayBeforeAttracting;

        int maxParticles = particleSystem.main.maxParticles;
        particles = new ParticleSystem.Particle[maxParticles];
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (!isAttracting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isAttracting = true;
            }
            else
            {
                return;
            }
        }

        int particleCount = particleSystem.GetParticles(particles);
        float step = attractionSpeed * Time.deltaTime;
        Vector3 playerPosition = playerTransform.position + playerHeight;
        ParticleSystemSimulationSpace simSpace = particleSystem.main.simulationSpace;
        Transform customSimSpace = particleSystem.main.customSimulationSpace;

        for (int i = 0; i < particleCount; i++)
        {
            Vector3 particleWorldPosition = GetWorldPosition(particles[i].position, simSpace, customSimSpace);

            Vector3 directionToPlayer = playerPosition - particleWorldPosition;
            Vector3 newPos = particleWorldPosition + (directionToPlayer.normalized * step);

            particles[i].position = transform.InverseTransformPoint(newPos);
        }

        particleSystem.SetParticles(particles, particleCount);
    }

    private Vector3 GetWorldPosition(Vector3 particlePosition, ParticleSystemSimulationSpace simSpace, Transform customSimSpace)
    {
        switch (simSpace)
        {
            case ParticleSystemSimulationSpace.World:
                return particlePosition;
            case ParticleSystemSimulationSpace.Custom:
                return customSimSpace.TransformPoint(particlePosition);
            default:
                return transform.TransformPoint(particlePosition);
        }
    }
}
