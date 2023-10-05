using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab; 
    [SerializeField] private Transform player; 
    [SerializeField] private float spawnInterval = 10.0f; 
    private float spawnRadius = 10.0f; 

    private void Start()
    {
        
        InvokeRepeating("SpawnMonster", 0f, spawnInterval);
    }

    void SpawnMonster()
    {
        if (monsterPrefab && player)
        {

            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0; // Keep them at the same vertical level
            Vector3 spawnPosition = transform.position + randomOffset;

            GameObject spawnedMonster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);


            MonsterMovement monsterMovement = spawnedMonster.GetComponent<MonsterMovement>();
            if (monsterMovement)
            {
                monsterMovement.SetPlayer(player);
            }
        }
    }
}
