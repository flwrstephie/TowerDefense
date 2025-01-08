using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public MeshCollider spawnArea; 
    public int enemiesPerWave = 5; 
    public float spawnInterval = 1f; 

    private int currentWave = 1; 
    private int totalWaves = 3; 

    void Start()
    {
        StartCoroutine(SpawnWave());
    }
    private IEnumerator SpawnWave()
    {
        while (currentWave <= totalWaves)
        {
            Debug.Log("Wave " + currentWave + " starting!");

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            currentWave++;
            yield return new WaitForSeconds(5f); 
        }

        Debug.Log("All waves completed!");
    }

    private void SpawnEnemy()
    {
        
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
            spawnArea.bounds.center.y, 
            Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
        );

        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }
}
