using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject m_controlPannel;
    [SerializeField] private GameObject m_progressLabel;
    private bool m_isConnecting;

    public byte m_maxPlayersPerRoom = 4;
    string m_gameVersion = "1";

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start() {
        m_progressLabel.SetActive(false);
        m_controlPannel.SetActive(true);
    }

    public void Connect(string roomName){
        m_isConnecting = true;

        m_progressLabel.SetActive(true);
        m_controlPannel.SetActive(false);

        if(PhotonNetwork.IsConnected){
            var roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, new TypedLobby(), new string[]{});
        }

        else
        {
            PhotonNetwork.GameVersion = m_gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //PUNCallBacks

    public override void OnConnectedToMaster(){
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN.");
        if(m_isConnecting){
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause){
        m_progressLabel.SetActive(false);
        m_controlPannel.SetActive(true);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message){
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
        PhotonNetwork.CreateRoom("Anthony", new RoomOptions());
    }

    public override void OnJoinedRoom(){
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        Debug.Log("We load th 'Room for 1'");

        PhotonNetwork.LoadLevel("Room for 1");
    }
}
