using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject m_playerPrefab;
    public GameObject m_playerHead;

    private IEnumerator Start() {
        if(m_playerPrefab == null){
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
        }

        else{
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);

            if(PlayerManager.s_localPlayerInstance == null){
                Debug.LogFormat("We are Instantiating Localplayer from {0}", SceneManagerHelper.ActiveSceneName);
                var player = PhotonNetwork.Instantiate(this.m_playerPrefab.name, new Vector3(0,0.5f, 0), Quaternion.identity, 0);
                var camera = Instantiate(m_playerHead, new Vector3(0,0, 0), Quaternion.identity);

                yield return new WaitForEndOfFrame();

                camera.transform.parent = player.transform.GetChild(0);
                player.transform.GetChild(0).Find("CenterEyeAnchor").parent = camera.transform;
            }
            else{
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
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

            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);

        if(PhotonNetwork.IsMasterClient){
            Debug.LogFormat("Someone Left. IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            LoadArena();
        }
    }
}
