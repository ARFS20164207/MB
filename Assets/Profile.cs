using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviourPun
{
    public string nickname;
    public Sprite picture;
    public bool isTurn;
    public int Team;


    // Start is called before the first frame update
    void Start()
    {
        nickname = photonView.Owner.NickName;
        
        if ( nickname == "")
        {
            photonView.Owner.NickName = "Player " + Random.Range(10001, 99999);
            nickname = photonView.Owner.NickName;
        }
        OnlineProfiles.instance.profiles.Add(this);
    }

}
