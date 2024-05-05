using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LegSolver : MonoBehaviour
{
    public float forwardStepDistance = 1f;
    public float sideStepDistance = 0.5f;
    public float rotDistance = 60;

    public float initOffset;
    public Vector3 stepOffset;
    public float stepHeight = 0.3f;
    public float stepSpeed = 1f;

    public LegSolver otherLeg;
    public Transform IKTarget;
    public LayerMask stepLayer;
    public NavMeshAgent agent;

    public AudioSource stepAudio;

    private Vector3 currentPosition;
    private Quaternion currentRotation;
    private Vector3 oldPosition;
    private Quaternion oldRotation;
    private Vector3 newPosition;
    private Quaternion newRotation;

    private float lerp = 1;
    public float nextStep = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        IKTarget.position += initOffset * transform.forward;

        currentPosition = oldPosition = newPosition = IKTarget.position;
        currentRotation = oldRotation = newRotation = IKTarget.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 3f, stepLayer))
		{
            Vector2 hitPosXZ = new Vector2(hit.point.x - currentPosition.x, hit.point.z - currentPosition.z);
			{
                if(Mathf.Abs(Vector2.Dot(hitPosXZ, new Vector2(transform.right.x, transform.right.z))) > sideStepDistance || 
                    Mathf.Abs(Vector2.Dot(hitPosXZ, new Vector2(transform.forward.x, transform.forward.z))) > forwardStepDistance || 
                    Quaternion.Angle(transform.rotation, currentRotation) > rotDistance)
		        {
                    if(lerp >= 1 && otherLeg.Still() && Time.time > nextStep)
			        {
                        //grounded = true;
                        newPosition = hit.point + 0.75f * agent.velocity + transform.TransformDirection(stepOffset);
                        newRotation = transform.rotation;

                        lerp = 0;
                        nextStep += 0.1f;
			        }
		        }
			}

            if(lerp < 1)
		    {
                Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
                tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight; ;

                currentPosition = tempPosition;
                currentRotation = Quaternion.Lerp(oldRotation, newRotation, lerp);
                lerp += Time.deltaTime * stepSpeed;
                if(lerp >= 1)
				{
                    stepAudio.Play();
				}
            }
		    else
		    {
                oldPosition = newPosition;
                oldRotation = newRotation;
            }
		}
		else
		{
            currentPosition = transform.position + -1.5f * transform.up;
		}


        //Debug.Log(currentPosition);
        IKTarget.position = currentPosition;
        IKTarget.rotation = currentRotation;
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(currentPosition, 0.2f);
	}

    public bool Still()
	{
        return (lerp >= 1 || lerp < 0);
	}
}
