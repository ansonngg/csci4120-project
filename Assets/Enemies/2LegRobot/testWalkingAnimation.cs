using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testWalkingAnimation : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator animator;
    private float previousAngle = 0;

    public Transform target;
    public GameObject bone;
    public float turnSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
            

        if(agent.remainingDistance <= agent.stoppingDistance)
		{
            animator.SetBool("isWalking", false);
		}
		else
		{
            animator.SetBool("isWalking", true);
        }
    }

	private void LateUpdate()
	{

        float angle = lookAngleXZ(target);
        float diff = previousAngle - angle;

        //Debug.Log(diff);
        if (diff > turnSpeed && diff < 180 || diff <= -180)
		{
            previousAngle -= turnSpeed;
            bone.transform.localEulerAngles = new Vector3(90, 0, previousAngle);
		}else if(diff < -turnSpeed && diff > -180 || diff >= 180)
		{
            previousAngle += turnSpeed;
            bone.transform.localEulerAngles = new Vector3(90, 0, previousAngle);
        }
		else
		{
            previousAngle = angle;
            bone.transform.localEulerAngles = new Vector3(90, 0, angle);
        }

        if(previousAngle > 180)
		{
            previousAngle -= 360;
		}else if(previousAngle < -180)
		{
            previousAngle += 360;
		}
        //Debug.Log(bone.transform.localEulerAngles);
    }

    float lookAngleXZ(Transform target)
	{
        float x = target.position.x - transform.position.x;
        float z = target.position.z - transform.position.z;
        float angle = Vector3.Angle(new Vector3(x, 0, z), transform.forward);
        float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(new Vector3(x, 0, z), transform.forward)));
        angle *= sign;

        return angle;
    }
}
