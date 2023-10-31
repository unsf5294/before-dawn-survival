using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab1; // The monster prefab to spawn
    public GameObject monsterPrefab2;
    public GameObject monsterPrefab3;
    private List<GameObject> monsters = new List<GameObject>();
    [SerializeField] private GameObject monsterSpawner; // Parent object to all spawned enemies
    [SerializeField] private Transform player;
    [SerializeField] private float initialDelay = 30f; // Time before first spawn
    [SerializeField] private float SpawnRadiusMin = 30f;
    [SerializeField] private float SpawnRadiusMax = 50f;
    [SerializeField] private float SpawnInterval = 15f;

    private void Start()
    {
        //Get each child in parent and add to list
        foreach (Transform monster in monsterSpawner.transform)
        {
            monsters.Add(monster.gameObject);
        }

        StartCoroutine(SpawnRoutine(monsterPrefab1, 1));
        StartCoroutine(SpawnRoutine(monsterPrefab2, 1));
        StartCoroutine(SpawnRoutine(monsterPrefab3, 2));
    }

    private IEnumerator SpawnRoutine(GameObject monsterPrefab, float intervalFactor)
    {
        yield return new WaitForSeconds(initialDelay);
        float startTime = Time.time;

        //Spawn one enemy per interval
        while (true)
        {
            SpawnMonster(SpawnRadiusMin, SpawnRadiusMax, monsterPrefab);
            yield return new WaitForSeconds(SpawnInterval*intervalFactor);
        }
    }

    private void SpawnMonster(float minRadius, float maxRadius, GameObject monsterPrefab)
    {
        Vector3 spawnPosition = player.position + (Random.onUnitSphere * Random.Range(minRadius, maxRadius));
        spawnPosition.y = 0; // Assuming you want to spawn on the ground level

        GameObject newMonster = (GameObject) Instantiate(monsterPrefab, spawnPosition, Quaternion.identity, monsterSpawner.transform);
    }

    public void DeactivateMonsters()
    {
        foreach (GameObject monster in monsters)
        {
            monster.SetActive(false);
        }
    }

    public void setSpawnInterval(float spawnInterval)
    {
        SpawnInterval = spawnInterval;
    }

    public float getSpawnInterval()
    {
        return SpawnInterval;
    }
}
