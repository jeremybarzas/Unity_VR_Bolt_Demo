using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMomentum : MonoBehaviour
{
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    { 
        // this is gathered data from the stick
        Vector3 momentum = rb.mass * rb.velocity;
        var force = momentum * Time.deltaTime;

        // this is going to be done on the ball
        Vector3 tempStickTipPos = new Vector3(1, 1, 1);
        rb.AddForceAtPosition(force, tempStickTipPos, ForceMode.Impulse);
    }
}
