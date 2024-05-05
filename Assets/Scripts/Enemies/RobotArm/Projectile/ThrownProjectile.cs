using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

public class ThrownProjectile : MonoBehaviour, IPooledObject
{
    private bool attach = false;
    private Rigidbody rb;
    private BoxCollider collider;
    public Transform claw;

    public float radius;
    public int damage;
    public LayerMask playerLayer;
    public LayerMask obestacleLayer;
    public GameObject particle;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (attach)
		{
            transform.position = claw.position;
            transform.rotation = claw.rotation;
		}
    }

    public void StartAttach(Transform clawPos)
	{
        attach = true;
        claw = clawPos;
        rb.useGravity = true;
        gameObject.layer = claw.gameObject.layer;
	}

    public void ThrowProjectile(Vector3 targetPos)
    {
        attach = false;
        rb.velocity = Vector3.zero;
        Vector3 throwDir = (targetPos - claw.position) + 0f * Vector3.up;
        rb.AddForce(100 * throwDir);
        rb.AddTorque(10 * throwDir);

        collider.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ApplyDamage();

        Instantiate(particle, transform.position, transform.rotation);
        ReturnToPool();
    }

    private void ApplyDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, playerLayer);

        foreach (Collider collider in colliders)
        {
            PlayerStats player = collider.GetComponent<PlayerStats>();
            if (player != null && !Physics.Raycast(transform.position, collider.transform.position - transform.position, out RaycastHit hit, Vector3.Distance(transform.position, collider.transform.position), obestacleLayer))
            {
                player.ApplyDamage((int)damage);
            }
        }
    }

    public void OnObjectInstantiate()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        collider.enabled = false;
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
