using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    private bool spawningLasers;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] Laser laserToSpawn;
    void Start()
    {
        SetSpawningLasers(true);
    }

    public void SetSpawningLasers(bool shouldSpawnLasers)
    {
        spawningLasers = shouldSpawnLasers;
        if (spawningLasers)
        {
            StartCoroutine(SpawnLasers());
        }
    }

    IEnumerator SpawnLasers()
    {
        while (spawningLasers)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            Instantiate(laserToSpawn, transform.position, Quaternion.identity);
        }
    }
}
