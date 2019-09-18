using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private bool m_didHaveTarget = false;
    public GameObject m_Target;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if(m_Target == null){
            Debug.Log("No target for the camera to follow.");
            return;
        }

        SetTarget(m_Target);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Target == null){
            Debug.Log("No target for the camera to follow.");

            if(m_didHaveTarget){
                Destroy(gameObject);
            }

            return;
        }

        transform.position = m_Target.transform.position;
        transform.rotation = m_Target.transform.rotation;
    }

    public void SetTarget(GameObject tar){
        m_didHaveTarget = true;
        m_Target = tar;
        transform.position = m_Target.transform.position;
        transform.rotation = m_Target.transform.rotation;
    }
}
