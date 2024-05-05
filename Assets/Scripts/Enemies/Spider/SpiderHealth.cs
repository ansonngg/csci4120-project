using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderHealth : EnemyHealth
{
    public BodyPart part;
    private Animator animator;
    private NavMeshAgent agent;
    private SpiderAI ai;
    private float despawnTime;

    // Start is called before the first frame update
    void Awake()
    {
        HP = maxHP;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ai = GetComponent<SpiderAI>();
    }

	private void Update()
	{
		if(dead && Time.time > despawnTime)
		{
            ReturnToPool();
		}
	}

	protected override void Death()
	{
        animator.SetBool("Death", true);
        ai.enabled = false;
        agent.enabled = false;
        part.enabled = false;

        Debug.Log("dies");

        despawnTime = Time.time + 3;
	}

    public void OnObjectInstantiate()
	{
        HP = maxHP;
        dead = false;
        animator.SetBool("Death", false);
        animator.SetBool("Walk", false);

        ai.enabled = true;
        agent.enabled = true;
        part.enabled = true;
    }

    public void ReturnToPool()
	{
        gameObject.SetActive(false);
        SpiderPool.Instance.BackToPool(this);
	}
}
