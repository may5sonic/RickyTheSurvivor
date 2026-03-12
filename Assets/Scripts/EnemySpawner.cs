using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Enemy prefab to spawn
    public GameObject enemyPrefab;

    // How often an enemy appears
    public float spawnInterval = 2f;

    // Number of enemies allowed at the same time
    public int maxEnemies = 10;

    // X and Z range for spawn positions
    public float minX = -8f;
    public float maxX = 8f;
    public float minZ = -8f;
    public float maxZ = 8f;

    void Start()
    {
        // Repeatedly call SpawnEnemy every few seconds
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Count how many enemy objects are currently in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        // If we already have too many enemies, do not spawn more
        if (enemies.Length >= maxEnemies)
            return;

        // Generate a random position within the allowed range
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        Vector3 spawnPosition = new Vector3(randomX, 0.5f, randomZ);

        // Create the enemy in the scene
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}