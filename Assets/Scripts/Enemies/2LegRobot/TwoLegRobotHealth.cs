using BodyPartEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class TwoLegRobotHealth : EnemyHealth
{

	public RigBuilder rig;
	public TwoLegRobotMove move;
	public NavMeshAgent agent;
	public TwoLegRobotAim aim;
	public FireWithinAngle gun;
	public FireWithinAngle Lcannon;
	public FireWithinAngle Rcannon;
	public LegSolver LLeg;
	public LegSolver RLeg;

	public ParticleSystem explosion;
	public MeshRenderer Eye;
	public Material offMaterial;

	public override void TakeDamage(float damage)
	{
		if (!move.activated)
		{
			move.MoveToPlayer();
		}
		base.TakeDamage(damage);
	}
	public override void PartBroken(PartType part)
	{
		switch (part)
		{
			case PartType.None:
				{
					return;
				}
			case PartType.Movement:
				{
					move.maxSpeed = 1;
					return;
				}
			case PartType.Aiming:
				{
					aim.turnSpeed = 0.05f;
					gun.spread = 0.3f;
					Lcannon.spread = 0.3f;
					Rcannon.spread = 0.3f;
					return;
				}
			case PartType.PrimaryFire:
				{
					return;
				}
			case PartType.SecondaryFire:
				{
					return;
				}
			default: return;
		}
	}

	public void SetAllEnable(bool b)
	{
		rig.enabled = b;
		agent.enabled = b;
		aim.enabled = b;
		gun.enabled = b;
		Lcannon.enabled = b;
		Rcannon.enabled = b;
		LLeg.enabled = b;
		RLeg.enabled = b;
	}

	protected override void Death()
	{

		SetAllEnable(false);
		move.StopAllCoroutines();
		move.enabled = false;

		Eye.material = offMaterial;

		Rigidbody rb = (Rigidbody) gameObject.AddComponent(typeof(Rigidbody));

		Instantiate(explosion, transform.position, transform.rotation);
		

	}
}
