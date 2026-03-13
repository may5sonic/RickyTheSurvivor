using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public int maxEnemies = 10;
    
    public float minX = -8f;
    public float maxX = 8f;
    public float minZ = -8f;
    public float maxZ = 8f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        if (enemies.Length >= maxEnemies)
            return;

        float randomX = transform.position.x + Random.Range(minX, maxX);
        float randomZ = transform.position.z + Random.Range(minZ, maxZ);
        
        Vector3 roughSpawnPosition = new Vector3(randomX, transform.position.y, randomZ); 

        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(roughSpawnPosition, out hit, 10.0f, NavMesh.AllAreas))
        {
            Instantiate(enemyPrefab, hit.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Still can't find the NavMesh! Is the spawner placed above the baked floor?");
        }
    }
}