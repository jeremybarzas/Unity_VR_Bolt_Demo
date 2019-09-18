using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject s_localPlayerInstance;
    public float m_speed;

    private void Awake() {
        if(photonView.IsMine){
            PlayerManager.s_localPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }

    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 0.5f, 0f);
        }
    }

    void Update()
    {
        var stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) * m_speed * Time.deltaTime;

        s_localPlayerInstance.transform.position += new Vector3(stick.x, 0, stick.y);
    }
}
