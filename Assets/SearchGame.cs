using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGame : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        FindRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        FindRoom();
    }
    void FindRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LoadLevel(2);
            }
            else
            {
                PhotonNetwork.JoinRandomOrCreateRoom();
            }
        }
    }
}
