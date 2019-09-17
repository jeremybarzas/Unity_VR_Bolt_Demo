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

        var force = col.gameObject.GetComponent<VelocityCalculator>().m_velocity;
        rb.AddForceAtPosition((force * m_forceMultipler), transform.position, ForceMode.Impulse);
    }
}
