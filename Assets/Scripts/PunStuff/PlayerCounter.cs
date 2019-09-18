using UnityEngine;
using Photon.Pun;

public class PlayerCounter : MonoBehaviour
{
    private int m_playerCount = 0;

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.PlayerList.Length != m_playerCount){
            foreach(Transform t in transform){
                t.gameObject.SetActive(false);
            }
            
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++){
                transform.GetChild(i).gameObject.SetActive(true);
            }

            m_playerCount = PhotonNetwork.PlayerList.Length;
        }
    }
}
