using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RobotArmHealth : EnemyHealth
{
    public RobotArmAction action;
    public List<GameObject> bodyParts;
    public LaserController laser;
	public RigBuilder rig;

	public GameObject particle;
	public bool canTakeDamage = false;

	public Win gameManager;

	public override void TakeDamage(float damage)
	{
		if (canTakeDamage)
		{
			base.TakeDamage(damage);
		}
	}

	protected override void Death()
	{
		rig.enabled = false;
		laser.ToggleAll(false);
		action.StopAllCoroutines();
		action.enabled = false;

		Instantiate(particle, transform.position, transform.rotation);

		foreach(GameObject part in bodyParts)
		{
			part.AddComponent(typeof(Rigidbody));
		}

		gameManager.deadArm++;
	}

}
