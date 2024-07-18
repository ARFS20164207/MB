using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonDebugConnect : MonoBehaviourPunCallbacks
{
    public GameObject Manager;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected) PhotonNetwork.ConnectUsingSettings();
        else StartGame();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        StartGame();
    }

    public void StartGame()
    {
        Manager.SetActive(true);
    }
}
