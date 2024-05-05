using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWithinAngle : MonoBehaviour
{
    //public GameObject bulletPrefab;
    public string pooledPrefab; 
    public Transform target;

    public float angle = 5f;
    public float shootForce;
    public float spread = 0.15f;
    public float cooldown = 0.1f;

    public float minRange = 0;
    public float maxRange = 30;

    public LayerMask shootLayer;
    public LayerMask obestacleLayer;
    public float bulletDamage;

    public AudioSource fireSound;

    private bool isShooting;
    private float nextShot = 0;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        if (dist < maxRange && dist > minRange && lookAngleXZ(target) < angle)
		{
            //if(!Physics.Raycast(transform.position, target.position - transform.position, out RaycastHit hit, Vector3.Distance(transform.position, target.position), obestacleLayer))
			
            isShooting = true;
            if(Time.time > nextShot)
		    {
                GameObject bulletInstance = PoolManager.Instance.InstantatieFromPool(pooledPrefab, transform.position, transform.rotation);
                Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

                IFireable fireScript = bulletInstance.GetComponent<IFireable>();

                if(fireScript != null)
				{
                    if(fireSound!= null)
					{
                        fireSound.PlayOneShot(fireSound.clip, 1);
					}
                    fireScript.Fire(shootForce, (transform.forward + spread * randomForce.normalized), bulletDamage, shootLayer);
				}

                nextShot = Time.time + cooldown;
		    }
			
        }
		else
		{
            isShooting = false;
		}
    }

    public bool IsShooting()
	{
        return isShooting;
	}

    float lookAngleXZ(Transform point)
    {
        float x = point.position.x - transform.position.x;
        float z = point.position.z - transform.position.z;
        float angle = Vector3.Angle(new Vector3(x, 0, z), new Vector3(transform.forward.x, 0, transform.forward.z));
        //float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(new Vector3(x, 0, z), transform.forward)));
        //angle *= sign;

        return angle;
    }
}
