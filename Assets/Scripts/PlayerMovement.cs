using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float m_speed;

    void Update()
    {
        var stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) * m_speed * Time.deltaTime;

        transform.Translate(stick.x, 0, stick.y);
    }
}
