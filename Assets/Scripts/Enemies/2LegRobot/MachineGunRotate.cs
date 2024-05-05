using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunRotate : MonoBehaviour
{
    public FireWithinAngle gun;
    public float rotateSpeed = 10;

    void FixedUpdate()
    {
		if (gun.enabled && gun.IsShooting())
		{
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (transform.localEulerAngles.y + rotateSpeed) % 360, transform.localEulerAngles.z); 
		}
    }
}
