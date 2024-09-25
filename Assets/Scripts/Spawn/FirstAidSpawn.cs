using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidSpawn : MonoBehaviour
{
    public int firstAidsOnStart = 5;
    public int allowedFirstAids = 5;
    public GameObject firstAid;
    public LayerMask groundLayer;
    bool alreadySpawn;
    int spawnedFirstAids = 0;


    void Start()
    {
        IEnumerator SpawnAids()
        {
            for (int i = 0; i < firstAidsOnStart; i++)
            {
                SpawnFirstAid();
                yield return new WaitForSeconds(2f);
            }
        }
        StartCoroutine(SpawnAids());
    }

    void Update()
    {
        if (!alreadySpawn && spawnedFirstAids < allowedFirstAids)
        {
            alreadySpawn = true;
            SpawnFirstAid();
            Invoke(nameof(ResetSpawner), 30f);
        }
    }

    void ResetSpawner()
    {
        alreadySpawn = false;
    }

    void SpawnFirstAid()
    {
        float xMax = 550;
        float zMax = 550;
        float x = transform.position.x + Random.Range(-xMax, xMax);
        float z = transform.position.z + Random.Range(-zMax, zMax);
        float y = GetGroundHeightAt(x, z) + 1f;
        Instantiate(firstAid, new Vector3(x, y, z), transform.rotation);
        spawnedFirstAids++;
    }

    float GetGroundHeightAt(float x, float z)
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(x, 300f, z), Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            return hit.point.y;

        return 0f;
    }

    public void DeleteFirstAid()
    {
        spawnedFirstAids--;
    }
}
