using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

public class PooledCannon : MonoBehaviour, IPooledObject, IFireable
{
	public string pooledParticle;
	public GameObject particle;
	public LayerMask playerLayer;
	public LayerMask obestacleLayer;

	public float radius;

	private float bulletDamage;
	private Rigidbody rb;
	private Vector3 previousPos;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{

		RaycastHit hit;

		if (Physics.Raycast(previousPos, transform.position - previousPos, out hit, Vector3.Distance(previousPos, transform.position), playerLayer))
		{
			//Debug.DrawRay(previousPos, (transform.position - previousPos).normalized * Vector3.Distance(previousPos, transform.position), Color.red, 1);
			//Debug.Log(hit.collider.name + " Ray");
			ApplyDamage();
			ReturnToPool();
		}

		previousPos = transform.position;
	}

	private void OnCollisionEnter(Collision collision)
	{
		ApplyDamage();

		Instantiate(particle, transform.position, transform.rotation);
		ReturnToPool();
	}

	private void ApplyDamage()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, radius, playerLayer);

		foreach (Collider collider in colliders)
		{
			PlayerStats player = collider.GetComponent<PlayerStats>();
			if (player != null && !Physics.Raycast(transform.position, collider.transform.position - transform.position, out RaycastHit hit, Vector3.Distance(transform.position, collider.transform.position), obestacleLayer))
			{
				player.ApplyDamage((int)bulletDamage);
			}
		}
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

	public void Fire(float force, Vector3 direction, float damage, LayerMask targetLayer)
	{
		rb.AddForce(force * direction);
		bulletDamage = damage;
		playerLayer = targetLayer;
	}
}
