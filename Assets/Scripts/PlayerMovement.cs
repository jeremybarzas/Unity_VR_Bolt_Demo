using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float m_speed;
    public Camera playerCamera;

    private void Awake()
    {
        // if(photonView.IsMine)
        // {
        //     PlayerManager.s_localPlayerInstance = this.gameObject;
        // }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        Vector3 stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) * Time.deltaTime;
        if (stick == Vector3.zero)
            return;

        Vector3 input = new Vector3(stick.x, 0, stick.y);

        //var facing = playerCamera.transform.forward;
        //Vector3 forward = new Vector3(facing.x, 0, facing.z);

        //Vector3 movement = (forward + input).normalized * m_speed;

        var rightDirection = input.x * new Vector3(playerCamera.transform.right.x, 0, playerCamera.transform.right.z);
        var forwardDirection = input.z * new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z);        

        var movement = (rightDirection + forwardDirection) * m_speed;

        transform.Translate(movement);
    }
}
