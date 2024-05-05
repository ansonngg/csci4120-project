using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DarkTreeFPS;

public class SpiderAI : MonoBehaviour
{
    public Transform player;
    public float damageDistance;
    public int damage;

    private Animator animator;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.position;

        if(agent.velocity.magnitude > 1)
		{
            animator.SetBool("Walk", true);
		}
		else
		{
            animator.SetBool("Walk", false);
		}

        if(Vector3.Distance(player.position, transform.position) < damageDistance)
		{
            PlayerStats stat = player.GetComponent<PlayerStats>();

            if(stat != null)
			{
                stat.ApplyDamage(damage);
                GetComponent<SpiderHealth>().ReturnToPool();
			}
		}
    }
}
