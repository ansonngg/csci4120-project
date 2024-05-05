using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardRate;
    public float turnRate;
    public float jumpForce;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void FixedUpdate()
    {
        float forwardAmount = Input.GetAxis("Vertical") * forwardRate;
        float turnForce = Input.GetAxis("Horizontal") * turnRate;
        transform.Rotate(0, turnForce, 0);
        transform.position += transform.forward * forwardAmount * Time.deltaTime;
    }
}
