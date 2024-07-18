using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceConnect : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        gameManager.SetActive(true);

    }
}
