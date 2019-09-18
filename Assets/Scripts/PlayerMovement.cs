using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float m_speed;

    private void Awake() {
        // if(photonView.IsMine){
        //     PlayerManager.s_localPlayerInstance = this.gameObject;
        // }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        var stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) * m_speed * Time.deltaTime;

        transform.Translate(stick.x, 0, stick.y);
    }
}
