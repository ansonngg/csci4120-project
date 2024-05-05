using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLegRobotAim : MonoBehaviour
{
    private float previousAngle = 0;

    public Transform player;
    public Transform target;
    public float turnSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = lookAngleXZ(player);
        float diff = previousAngle - angle;

        
        if (diff > turnSpeed && (diff < 180 || diff <= -180))
        {
            previousAngle -= turnSpeed;
            target.localPosition = Quaternion.AngleAxis(-previousAngle, Vector3.up) * Vector3.forward;
        }
        else if (diff < -turnSpeed && (diff > -180 || diff >= 180))
        {
            previousAngle += turnSpeed;
            target.localPosition = Quaternion.AngleAxis(-previousAngle, Vector3.up) * Vector3.forward;
        }
        else
        {
            previousAngle = angle;
            target.localPosition = Quaternion.AngleAxis(-previousAngle, Vector3.up) * Vector3.forward;
        }

        if (previousAngle > 180)
        {
            previousAngle -= 360;
        }
        else if (previousAngle < -180)
        {
            previousAngle += 360;
        }
    }

    float lookAngleXZ(Transform point)
    {
        float x = point.position.x - transform.position.x;
        float z = point.position.z - transform.position.z;
        float angle = Vector3.Angle(new Vector3(x, 0, z), transform.forward);
        float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(new Vector3(x, 0, z), transform.forward)));
        angle *= sign;

        return angle;
    }
}
