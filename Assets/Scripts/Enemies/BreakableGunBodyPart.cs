using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGunBodyPart : BodyPart
{
	public FireWithinAngle gun;
	protected override void PartBreak()
	{
		gun.enabled = false;
		base.PartBreak();
	}
}
