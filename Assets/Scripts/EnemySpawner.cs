using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Enemies;
    [SerializeField] GameObject miniBoss;
    [SerializeField] Transform[] spawnPoints;
    float initialSpawnInterval = 2f;
    float spawnIntervalDecreaseRate = 0.1f;
    float minimumSpawnInterval = 0.5f;
    float spawnDuration = 60f; // 1 minute

    private float spawnInterval;
    private float nextSpawnTime = 0f;
    private float elapsedTime = 0f;

    void Start()
    {
        spawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < spawnDuration)
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnInterval;
                spawnInterval = Mathf.Max(spawnInterval - spawnIntervalDecreaseRate, minimumSpawnInterval);
            }
        }
        else if (elapsedTime >= spawnDuration && miniBoss != null)
        {
            SpawnMiniBoss();
            miniBoss = null; // Ensure the miniboss spawns only once
        }
    }

    void SpawnEnemy()
    {
        // Choose a random enemy
        int enemyIndex = Random.Range(0, Enemies.Length);

        // Choose a spawn point
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPosition = spawnPoints[spawnPointIndex].position;

        // Instantiate the enemy
        Instantiate(Enemies[enemyIndex], spawnPosition, Quaternion.identity);
    }

    void SpawnMiniBoss()
    {
        // Choose a random spawn point for the miniboss
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPosition = spawnPoints[spawnPointIndex].position;

        // Instantiate the miniboss
        Instantiate(miniBoss, spawnPosition, Quaternion.identity);
        Debug.Log("Miniboss spawned!");
    }
}
