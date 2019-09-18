using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject m_playerPrefab;
    public GameObject m_playerModel;
    public GameObject m_playerHead;
    public GameObject m_ball;

    private IEnumerator Start() {
        if(m_playerModel == null || m_playerPrefab == null || m_playerHead == null){
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }

        else{
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);

            if(PlayerManager.s_localPlayerInstance == null){
                Debug.LogFormat("We are Instantiating Localplayer from {0}", SceneManagerHelper.ActiveSceneName);
                var player = PhotonNetwork.Instantiate(this.m_playerPrefab.name, new Vector3(0,0, 0), Quaternion.identity);
                var playerModel = Instantiate(m_playerModel, new Vector3(0,0,0), Quaternion.identity);
                var camera = Instantiate(m_playerHead, new Vector3(0,0, 0), Quaternion.identity);
                var head = playerModel.transform.GetChild(0).Find("CenterEyeAnchor");
                
                playerModel.GetComponent<OVRCameraRig>().enabled = true;
                playerModel.GetComponent<OVRManager>().enabled = true;
                playerModel.GetComponent<OVRHeadsetEmulator>().enabled = true;

                foreach(Transform t in player.transform){
                    switch(t.name){
                        case "CenterEyeAnchor":
                            t.GetComponent<CameraFollow>().SetTarget(UIUtils.FindChild("CenterEyeAnchor", playerModel.transform).gameObject);
                            break;

                        case "OVRControllerPrefabRight":
                            t.GetComponent<CameraFollow>().SetTarget(UIUtils.FindChild("OVRControllerPrefabRight", playerModel.transform).gameObject);
                            break;

                        case "OVRControllerPrefabLeft":
                            t.GetComponent<CameraFollow>().SetTarget(UIUtils.FindChild("OVRControllerPrefabLeft", playerModel.transform).gameObject);
                            break;
                    }
                }

                yield return new WaitForEndOfFrame();

                playerModel.GetComponent<PlayerMovement>().playerCamera = UIUtils.FindChild("CenterEyeAnchor", playerModel.transform).gameObject.GetComponent<Camera>();

                camera.transform.parent = playerModel.transform.GetChild(0);

                head.transform.localScale = new Vector3(.5f,.5f,.5f);
            }
            else{
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        yield return new WaitForSeconds(0.5f);
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(m_ball.name, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    public override void OnLeftRoom(){
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    private void LoadArena(){
        if(!PhotonNetwork.IsMasterClient){
            Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master client");
            return;
        }

        Debug.LogFormat("PhotonNetwork : loading level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);

        if(PhotonNetwork.IsMasterClient){
            Debug.LogFormat("New player is in room. IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            //LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);

        if(PhotonNetwork.IsMasterClient){
            Debug.LogFormat("Someone Left. IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            //LoadArena();
        }
    }
}
