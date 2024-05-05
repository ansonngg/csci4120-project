using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BodyPartEnum;

public class BodyPart : MonoBehaviour
{
    public float maxHP = 100;
    public float resistance = 0;
    public PartType partType = PartType.None;
    public EnemyHealth totalHealth;
    
    private float HP;
    private bool broken = false;

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
    }

    public void ApplyDamage(int damage)
	{
        float damageTaken = Mathf.Max(damage - resistance, 1);
        HP -= damageTaken;
        totalHealth.TakeDamage(damageTaken);

        if(HP <= 0 && !broken)
		{
            broken = true;
            PartBreak();
		}
	}

    protected virtual void PartBreak()
	{
            totalHealth.PartBroken(partType);
        Debug.Log("Break" + this.name);
	}

    public bool IsBroken()
	{
        return broken;
	}

}
