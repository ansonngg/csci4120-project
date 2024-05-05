using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BodyPartEnum;

public class EnemyHealth : MonoBehaviour
{
    public float maxHP = 500;

    protected bool dead = false;
    protected float HP;
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
    }

    public virtual void TakeDamage(float damage)
	{
        HP -= damage;

        Debug.Log("Enemy Health = "+HP);

        if(HP <= 0 && !dead)
		{
            dead = true;
            Death();
		}
	}

    public virtual void PartBroken(PartType part)
	{
		switch (part)
		{
            case PartType.None:
				{
                    return;
				}
            case PartType.Movement:
				{
                    return;
				}
            case PartType.Aiming:
				{
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

    protected virtual void Death()
	{
        gameObject.SetActive(false);

	}
}
