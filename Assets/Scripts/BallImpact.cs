using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallImpact : MonoBehaviour
{
    Rigidbody rb;
    public float m_forceMultipler;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("terrain"))
            return;

        var veloCalc = col.gameObject.GetComponent<VelocityCalculator>();

        if (veloCalc == null)
            return;
        
        var force = col.gameObject.GetComponent<VelocityCalculator>().m_velocity;
        force.y = force.y < 0 ? 0 : force.y;

        rb.AddForceAtPosition((force * m_forceMultipler), col.contacts[0].point, ForceMode.Impulse);
        //rb.AddTorque((force * m_forceMultipler), ForceMode.VelocityChange);
    }
}
