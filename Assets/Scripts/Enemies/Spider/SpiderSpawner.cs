using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    public float spawnCooldown;
    public float burstCount;
    public Transform spawnPosition;

    public float spawnDistance;
    public Transform player;

    private float nextSpawn = 0;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.position, transform.position) < spawnDistance && Time.time > nextSpawn)
		{
            nextSpawn = Time.time + spawnCooldown;
                
            for(int i = 0; i < burstCount ; i++)
			{
                SpiderPool.Instance.InstantiateFromPool(spawnPosition.position, spawnPosition.rotation);
			}
		}
    }
}
