using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CloneLinkModel : MonoBehaviour
{
    private GameObject m_clone;
    public GameObject clone{get{return m_clone;}}

    public Transform m_targetParent;
    
    public void CreateClone(){
        m_clone = Instantiate(gameObject);
        m_clone.AddComponent<CameraFollow>().m_Target = gameObject;
        var tview = m_clone.AddComponent<PhotonTransformView>();
        m_clone.GetComponent<PhotonView>().ObservedComponents.Add(tview);

        if(m_targetParent != null){
            m_clone.transform.parent = m_targetParent;
        }
    }
}
