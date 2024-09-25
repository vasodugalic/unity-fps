

using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public int enemiesOnStart = 30;
    public int allowedEmeies = 50;
    public GameObject enemyAlien;
    public GameObject enemyInsect;
    public LayerMask groundLayer;
    bool alreadySpawn;
    int spawnedEnemies = 0;

    void Start()
    {
        IEnumerator SpawnEnemies()
        {
            for (int i = 0; i < enemiesOnStart; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }
        }
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        if (!alreadySpawn && spawnedEnemies < allowedEmeies)
        {
            alreadySpawn = true;
            SpawnEnemy();
            Invoke(nameof(ResetSpawner), 5f);
        }
    }
    
    void ResetSpawner()
    {
        alreadySpawn = false;
    }

    void SpawnEnemy()
    {
        float xMax = 550;
        float zMax = 550;
        float x = transform.position.x + Random.Range(-xMax, xMax);
        float z = transform.position.z + Random.Range(-zMax, zMax);
        float y = GetGroundHeightAt(x, z);

        GameObject enemy = Random.Range(0, 2) == 0 ? enemyAlien : enemyInsect;

        Instantiate(enemy, new Vector3(x, y, z), transform.rotation);
        spawnedEnemies++;
    }

    float GetGroundHeightAt(float x, float z)
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(x, 300f, z), Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            return hit.point.y;

        return 0f;
    }

    public void DeleteEnemy()
    {
        spawnedEnemies--;
    }
}
