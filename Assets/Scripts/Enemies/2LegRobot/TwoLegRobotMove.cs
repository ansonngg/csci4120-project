using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TwoLegRobotMove : MonoBehaviour
{
    public Transform player;
    public float activateDistance = 50;
    public float maxSpeed = 2.5f;
    public float rotateSpeed = 2;
    public LayerMask obestacleLayer;
    public LayerMask terrainLayer;

    public float chaseDistance = 50;
    public float backOffDistance = 15;
    public float retreatDistance = 10;

    private NavMeshAgent agent;
    private TwoLegRobotHealth health;
    public bool activated = false;
    private bool facePlayer = false;

    enum State
	{
        chase,
        fire,
        backoff,
        retreat,
        none
	}

    private State currentState;
    private IEnumerator currentCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = maxSpeed;

        health = GetComponent<TwoLegRobotHealth>();
        health.SetAllEnable(false);
        currentState = State.none;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (!activated && !Physics.Raycast(transform.position + 2 * Vector3.up, player.position - (transform.position +  2 * Vector3.up), out RaycastHit hit, playerDistance, obestacleLayer))
		{
            activated = true;
            health.SetAllEnable(true);
		}

		if (activated)
		{
            if(playerDistance > chaseDistance)
			{
                if(currentState != State.chase)
				{
                    currentState = State.chase;

                    if(currentCoroutine != null)
				    {
                        StopCoroutine(currentCoroutine);
				    }

                    currentCoroutine = Chase();
                    StartCoroutine(currentCoroutine);
				}
			}
			else if(playerDistance > backOffDistance)
			{
                if (currentState != State.fire)
				{
                    currentState = State.fire;

                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                    }

                    currentCoroutine = Fire();
                    StartCoroutine(currentCoroutine);
				}
            }else if(playerDistance > retreatDistance)
			{
                if(currentState != State.backoff)
				{
                    currentState = State.backoff;

                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                    }

                    currentCoroutine = BackOff();
                    StartCoroutine(currentCoroutine);
                }
			}
			else
			{
                if (currentState != State.retreat)
                {
                    currentState = State.retreat;

                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                    }

                    currentCoroutine = Retreat();
                    StartCoroutine(currentCoroutine);
                }
            }
		}

		if (facePlayer)
		{
            Vector3 relativePos = player.position - transform.position;
            relativePos.y = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativePos), rotateSpeed * Time.deltaTime);
		}
    }

    private void SetModeRun()
	{
        agent.speed = maxSpeed;
        agent.updateRotation = true;
        facePlayer = false;
	}

    private void SetModeAttackMove()
	{
        agent.speed = 0.5f * maxSpeed;
        agent.updateRotation = false;
        facePlayer = true;
	}

    public void MoveToPlayer()
	{
        agent.destination = player.position;
	}

    private IEnumerator Chase()
	{
        SetModeRun();
		while (true)
		{
            agent.destination = player.position;
            yield return null;
		}
	}

    private IEnumerator Fire()
	{
        SetModeAttackMove();
		//move sideways at random interval
		while (true)
		{
            Vector3 toPlayer = (player.position - transform.position).normalized;
            toPlayer.y = 0;

            Vector3 moveDir = RandomSign() * Vector3.Cross(toPlayer, transform.up);
            float moveDist = Random.Range(10, 15);

            if(!Physics.Raycast(transform.position + Vector3.up, moveDir, out RaycastHit hit, moveDist, terrainLayer))
			{
                NavMeshHit nHit;
                if (NavMesh.SamplePosition(transform.position + moveDist * moveDir, out nHit, 2f, NavMesh.AllAreas))
				{
                    agent.destination = nHit.position;
				}
			}

            yield return new WaitForSeconds(Random.Range(15, 30));
        }
	}

    private IEnumerator BackOff()
	{
        SetModeAttackMove();
        while (true)
		{
            Vector3 toPlayer = (player.position - transform.position).normalized;
            toPlayer.y = 0;

            Vector3 moveDir = -1 * toPlayer;
            float moveDist = 5;

            if (!Physics.Raycast(transform.position + Vector3.up, moveDir, out RaycastHit hit, moveDist, terrainLayer))
            {
                NavMeshHit nHit;
                if (NavMesh.SamplePosition(transform.position + moveDist * moveDir, out nHit, 2f, NavMesh.AllAreas))
                {
                    agent.destination = nHit.position;
                }
            }

            yield return new WaitForSeconds(1);
        }
	}

    private IEnumerator Retreat()
	{
        SetModeRun();
        while (true)
        {
            Vector3 toPlayer = (player.position - transform.position).normalized;
            toPlayer.y = 0;

            Vector3 moveDir = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * player.forward;
            float moveDist = 30;

            if (!Physics.Raycast(transform.position + Vector3.up, moveDir, out RaycastHit hit, moveDist, terrainLayer))
            {
                NavMeshHit nHit;
                if (NavMesh.SamplePosition(transform.position + moveDist * moveDir, out nHit, 2f, NavMesh.AllAreas))
                {
                    agent.destination = nHit.position;
                }
			}
			else
			{
                moveDir *= -1;

                NavMeshHit nHit;
                if (NavMesh.SamplePosition(transform.position + moveDist * moveDir, out nHit, 2f, NavMesh.AllAreas))
                {
                    agent.destination = nHit.position;
                }
            }

            yield return new WaitForSeconds(10);
        }
    }


    //------------------------------------------------------------------------------
    public static int RandomSign()
    {
        return Random.value < 0.5f ? 1 : -1;
    }

}
