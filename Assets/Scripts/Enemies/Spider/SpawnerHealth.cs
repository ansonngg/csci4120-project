using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerHealth : EnemyHealth
{
	public GameObject cylinder;
	public Material offMaterial;

	// Start is called before the first frame update
	protected override void Death()
	{
		cylinder.GetComponent<MeshRenderer>().material = offMaterial;
		GetComponent<SpiderSpawner>().enabled = false;
	}
}
