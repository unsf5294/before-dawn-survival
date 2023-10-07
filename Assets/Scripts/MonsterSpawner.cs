using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // The monster prefab to spawn
    [SerializeField] private Transform player;
    [SerializeField] private float initialDelay = 30f; // Time before first spawn
    [SerializeField] private float firstSpawnRadiusMin = 30f;
    [SerializeField] private float firstSpawnRadiusMax = 50f;
    [SerializeField] private float firstSpawnInterval = 15f;
    [SerializeField] private float secondSpawnRadiusMin = 15f;
    [SerializeField] private float secondSpawnRadiusMax = 30f;
    [SerializeField] private float secondSpawnInterval = 10f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(initialDelay);
        float startTime = Time.time;

        // Initial Spawn Behavior
        while (Time.time - startTime < 120f) // Continue this for 2 minutes
        {
            SpawnMonster(firstSpawnRadiusMin, firstSpawnRadiusMax);
            yield return new WaitForSeconds(firstSpawnInterval);
        }

        // After 2 minutes change to the new spawn behavior
        while (true)
        {
            SpawnMonster(secondSpawnRadiusMin, secondSpawnRadiusMax);
            yield return new WaitForSeconds(secondSpawnInterval);
        }
    }

    private void SpawnMonster(float minRadius, float maxRadius)
    {
        Vector3 spawnPosition = player.position + (Random.onUnitSphere * Random.Range(minRadius, maxRadius));
        spawnPosition.y = 0; // Assuming you want to spawn on the ground level

        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
    }
}
