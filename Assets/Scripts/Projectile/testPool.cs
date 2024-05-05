using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPool : MonoBehaviour
{
    public LayerMask targetLayer;
    private float readyTime = 0;
    // Update is called once per frame
    void Update()
    {
        if(Time.time > readyTime)
		{
            readyTime += 0.1f;
            GameObject bullet = PoolManager.Instance.InstantatieFromPool("bullet", transform.position, transform.rotation);
            bullet.GetComponent<PooledBullet>().Fire(10000, transform.forward, 10, targetLayer);
		}
    }
}
