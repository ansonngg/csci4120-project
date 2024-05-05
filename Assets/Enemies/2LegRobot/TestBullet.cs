using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    private Vector3 previousPos;
    public LayerMask layer;
    // Start is called before the first frame update
    void Start()
    {
        previousPos = transform.position;
    }

	
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(previousPos, transform.position - previousPos, out hit ,Vector3.Distance(previousPos, transform.position), layer))
		{
            Debug.Log(hit.collider.name);
            Destroy(gameObject);
		}
    }
}
