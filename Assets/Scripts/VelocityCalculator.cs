using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
    public Vector3 m_velocity;
    Vector3 prevPos;
    Vector3 curPos;
    
    void Start()
    {
        prevPos = transform.position;
    }

    private void FixedUpdate()
    {
        curPos = transform.position;
        m_velocity = (curPos - prevPos) / Time.deltaTime;
        prevPos = transform.position;
    }
}
