using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmControl : MonoBehaviour
{
    public Transform basebone;
    public Transform IKTarget;
    public Transform upperClaw;
    public Transform lowerClaw;
    public Transform target;

    public float baseRotOffset;
    public float turnSpeed = 0.1f;

    private IEnumerator rotateBase;
    private bool rotateBaseRunning;
    private IEnumerator moveArm;
    private bool moveArmRunning = false;
    private IEnumerator openClaw;
    private bool openClawRunning = false;
    private IEnumerator rotateClaw;
    private bool rotateClawRunning = false;

    public void rotateBaseTo(float targetRot, float moveTime)
	{
		if (rotateBaseRunning)
		{
            StopCoroutine(rotateBase);
		}

        targetRot = (targetRot + baseRotOffset) % 360;

        float ogRot = basebone.eulerAngles.y;
        float clockwiseAngle;
        float counterClockwiseAngle;

        if (targetRot < ogRot)
        {
            counterClockwiseAngle = targetRot - ogRot;
            clockwiseAngle = targetRot - (ogRot - 360);
		}
		else
		{
            counterClockwiseAngle = (targetRot - 360) - ogRot;
            clockwiseAngle = targetRot - ogRot;
		}

        if (Mathf.Abs(counterClockwiseAngle) > Mathf.Abs(clockwiseAngle))
		{
            rotateBase = SetBaseBoneRotationClockwise(targetRot, moveTime);
            StartCoroutine(rotateBase);
		}
		else
		{
            rotateBase = SetBaseBoneRotationCounterClockwise(targetRot, moveTime);
            StartCoroutine(rotateBase);
        }
	}

    public void rotateBaseTo(float targetRot, float moveTime, bool clockwise)
	{
        targetRot = (targetRot + baseRotOffset) % 360;
        if (rotateBaseRunning)
        {
            StopCoroutine(rotateBase);
        }

        if (clockwise)
		{
            rotateBase = SetBaseBoneRotationClockwise(targetRot, moveTime);
            StartCoroutine(rotateBase);
		}
		else
		{
            rotateBase = SetBaseBoneRotationCounterClockwise(targetRot, moveTime);
            StartCoroutine(rotateBase);
        }
	}

    public void LookPlayer(float duration)
	{
        if (rotateBaseRunning)
        {
            StopCoroutine(rotateBase);
        }

        rotateBase = FacePlayer(duration);
        StartCoroutine(rotateBase);
    }

    public void moveClawTo(Vector3 targetPos, float moveTime)
	{
		if (moveArmRunning)
		{
            StopCoroutine(moveArm);
		}

        moveArm = SetIkTargetPos(targetPos, moveTime);
        StartCoroutine(moveArm);
	}

    public void rotateClawTo(float targetRot, float moveTime)
	{
		if (rotateClawRunning)
		{
            StopCoroutine(rotateClaw);
		}

        rotateClaw = setClawRotation(targetRot, moveTime);
        StartCoroutine(rotateClaw);
	}

    public void openClawTo(float targetAngle, float moveTime)
    {
        if (openClawRunning)
        {
            StopCoroutine(openClaw);
        }

        openClaw = setClawAngle(targetAngle / 2, moveTime);
        StartCoroutine(openClaw);
    }

    private IEnumerator SetBaseBoneRotationClockwise(float targetRot, float moveTime)
	{
        rotateBaseRunning = true;
        float ogRot = basebone.eulerAngles.y;
        float startTime = Time.time;
        float lerp = 0;

        if (targetRot < ogRot)
        {
            ogRot -= 360;
        }
            
        while (lerp <= 1)
        {
            lerp = (Time.time - startTime) / moveTime;
            basebone.eulerAngles = new Vector3(0, Mathf.Lerp(ogRot, targetRot, lerp) % 360, 0);
            yield return null;
        }
        
        rotateBaseRunning = false;
	}

    private IEnumerator SetBaseBoneRotationCounterClockwise(float targetRot, float moveTime)
    {
        rotateBaseRunning = true;
        float ogRot = basebone.eulerAngles.y;
        float startTime = Time.time;
        float lerp = 0;

        if (targetRot > ogRot)
        {
            targetRot -= 360;
        }

        while (lerp <= 1)
        {
            lerp = (Time.time - startTime) / moveTime;
            basebone.eulerAngles = new Vector3(0, Mathf.Lerp(ogRot, targetRot, lerp) % 360, 0);
            yield return null;
        }

        rotateBaseRunning = false;
    }

    private IEnumerator FacePlayer(float duration)
	{
        rotateBaseRunning = true;
        float previousAngle = basebone.eulerAngles.y;
        float finishTime = Time.time + duration;

        while(Time.time < finishTime)
		{
            float angle = ClampAngle(lookAngleXZ(target) + baseRotOffset);
            float diff = previousAngle - angle;

            //Debug.Log(gameObject.name + " " + previousAngle +" "+ angle);
            //Debug.Log(diff);
            if (diff > turnSpeed && diff < 180 || diff <= -180)
            {
                previousAngle = ClampAngle(previousAngle - turnSpeed);
                basebone.transform.eulerAngles = new Vector3(0, previousAngle, 0);
            }
            else if (diff < -turnSpeed && diff > -180 || diff >= 180)
            {
                previousAngle = ClampAngle(previousAngle + turnSpeed);
                basebone.transform.eulerAngles = new Vector3(0, previousAngle, 0);
            }
            else
            {
                previousAngle = ClampAngle(angle);
                basebone.transform.eulerAngles = new Vector3(0, previousAngle, 0);
            }

            yield return null;
		}

        rotateBaseRunning = false;
    }

    private IEnumerator SetIkTargetPos(Vector3 targetPos, float moveTime)
	{
        moveArmRunning = true;
        Vector3 ogPos = IKTarget.localPosition;
        float startTime = Time.time;
        float lerp = 0;


        while(lerp <= 1)
		{
            lerp = (Time.time - startTime) / moveTime;
            IKTarget.localPosition = Vector3.Lerp(ogPos, targetPos, lerp);
            yield return null;
		}

        moveArmRunning = false;
    }

    private IEnumerator setClawAngle(float targetAngle, float moveTime)
    {
        openClawRunning = true;
        float ogAngle = upperClaw.localEulerAngles.z;
        float startTime = Time.time;
        float lerp = 0;


        while (lerp <= 1)
        {
            lerp = (Time.time - startTime) / moveTime;
            Vector3 angle = new Vector3(0, 0, Mathf.Lerp(ogAngle, targetAngle, lerp));
            upperClaw.localEulerAngles = angle;
            lowerClaw.localEulerAngles = -angle;
            yield return null;
        }

        openClawRunning = false;
    }

    private IEnumerator setClawRotation(float targetRot, float moveTime)
    {
        rotateClawRunning = true;
        float ogRot = IKTarget.localEulerAngles.z;
        float startTime = Time.time;
        float lerp = 0;


        while (lerp <= 1)
        {
            lerp = (Time.time - startTime) / moveTime;
            Vector3 rot = new Vector3(0, 0, Mathf.Lerp(ogRot, targetRot, lerp));
            IKTarget.localEulerAngles = rot; 
            yield return null;
        }

        rotateClawRunning = false;
    }

    //-----------------------------------------------------------------------------------------------------

    float lookAngleXZ(Transform point)
    {
        float x = point.position.x - transform.position.x;
        float z = point.position.z - transform.position.z;
        float angle = Vector3.Angle(new Vector3(x, 0, z), new Vector3(transform.right.x, 0, transform.right.z));
        float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(new Vector3(x, 0, z), transform.right)));
        angle *= sign;

        return angle;
    }

    float ClampAngle(float angle)
	{
        angle = angle % 360;
        if(angle < 0)
		{
            angle += 360;
		}

        return angle;
	}
}

