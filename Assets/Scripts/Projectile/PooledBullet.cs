using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

public class PooledBullet : MonoBehaviour, IPooledObject, IFireable
{
    private Rigidbody rb;
    private Vector3 previousPos;
    private LayerMask layer = ~0;

    private float bulletDamage;
    // Start is called before the first frame update

    void Awake()
	{
        rb = GetComponent<Rigidbody>();
	}
    
    void Update()
    {
        
        RaycastHit hit;

        if (Physics.Raycast(previousPos, transform.position - previousPos, out hit, Vector3.Distance(previousPos, transform.position), layer))
        {
            //Debug.DrawRay(previousPos, (transform.position - previousPos).normalized * Vector3.Distance(previousPos, transform.position), Color.red, 1);
            //Debug.Log(hit.collider.name + " Ray");
            PlayerStats playerStats = hit.collider.GetComponent<PlayerStats>();

            if(playerStats != null)
			{
                playerStats.ApplyDamage((int)bulletDamage);
			}

            ReturnToPool();
        }

        previousPos = transform.position;
    }

	private void OnCollisionEnter(Collision collision)
	{
        //Debug.Log(collision.gameObject.name + " Collision");
        ReturnToPool();
    }

    public void Fire(float force, Vector3 direction, float damage, LayerMask targetLayer)
	{
        rb.AddForce(force * direction);
        bulletDamage = damage;
        layer = targetLayer;
	}

    public void OnObjectInstantiate()
    {
        previousPos = transform.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void ReturnToPool()
	{
        gameObject.SetActive(false);
	}
}
