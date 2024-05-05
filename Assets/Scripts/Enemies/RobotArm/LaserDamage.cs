using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

public class LaserDamage : MonoBehaviour
{
	public int damage = 30;
	public LayerMask playerLayer;

	private bool hasHit = false;
	private void Update()
	{
		if (!hasHit)
		{
			RaycastHit hit;
			if(Physics.SphereCast(transform.position, 1f, transform.up, out hit, 50, playerLayer))
			{
				Debug.Log("hit" + hit.collider.name);
				PlayerStats stat = hit.collider.gameObject.GetComponent<PlayerStats>();

				if (stat != null)
				{
					stat.ApplyDamage(damage);
					hasHit = true;
				}
			}
		}
	}
	private void OnEnable()
	{
		hasHit = false;
	}
}
