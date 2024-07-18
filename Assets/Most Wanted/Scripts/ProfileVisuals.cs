using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileVisuals : MonoBehaviourPun
{
    public GameObject playerParent;
    public Image profilePicture;
    public TMP_Text profileName;

    public Image playerReadyImage;
    public bool isPlayerReady;
    public int playerID;

    [SerializeField] Color On, Off;

    [SerializeField] MatchMaking manager;


    // Start is called before the first frame update
    void Start()
    {
        isPlayerReady = false;
        playerReadyImage.color = Off;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ToogleReady()
    {
        if (playerID != PhotonNetwork.LocalPlayer.ActorNumber) return;

        isPlayerReady = !isPlayerReady;

        photonView.RPC("SetReady", RpcTarget.All, isPlayerReady,playerID);
    }
    [PunRPC]
    public void SetReady(bool isReady, int actor)
    {
        manager.GetVisualByID(actor).SetReadyLocal(isReady);
    }
    public void SetReadyLocal(bool isReady)
    {
        isPlayerReady = isReady;
        playerReadyImage.color = isReady ? On : Off;
    }
    public void SetName(string name)
    { profileName.text = name.ToLower();}
}
