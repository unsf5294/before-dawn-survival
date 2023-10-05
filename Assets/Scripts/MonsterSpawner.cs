using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab; 
    [SerializeField] private Transform player; // Add this reference to the player's transform
    [SerializeField] private float spawnInterval = 10.0f; 
    private float spawnRadius = 10.0f; // The radius around the spawner where monsters can spawn

    private void Start()
    {
        // Start the spawn loop
        InvokeRepeating("SpawnMonster", 0f, spawnInterval);
    }

    void SpawnMonster()
    {
        if (monsterPrefab && player)
        {
            // Random position generation around the spawner within the defined radius
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0; // Keep them at the same vertical level
            Vector3 spawnPosition = transform.position + randomOffset;

            GameObject spawnedMonster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

            // Set the player reference for the MonsterMovement script on the newly spawned monster
            MonsterMovement monsterMovement = spawnedMonster.GetComponent<MonsterMovement>();
            if (monsterMovement)
            {
                monsterMovement.SetPlayer(player);
            }
        }
    }
}
