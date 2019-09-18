using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(InputField))]
public class PlayerNameInput : MonoBehaviour
{
    const string m_playerNamePrefKey = "PlayerName";
    private InputField inputField;

    private void Start() {
        string defaultname = "";
        inputField = gameObject.GetComponent<InputField>();

        if(inputField != null){
            if(PlayerPrefs.HasKey(m_playerNamePrefKey)){
                defaultname = PlayerPrefs.GetString(m_playerNamePrefKey);
                inputField.text = defaultname;
            }
        }

        PhotonNetwork.NickName = defaultname;
    }

    public void SetPlayerName(){
        if(string.IsNullOrEmpty(inputField.text)){
            Debug.LogError("Player name is null or empty");
            return;
        }

        PhotonNetwork.NickName = inputField.text;

        PlayerPrefs.SetString(m_playerNamePrefKey, inputField.text);
    }
}
