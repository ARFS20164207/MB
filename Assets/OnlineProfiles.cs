using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineProfiles : MonoBehaviourPunCallbacks, ProfileManage
{
    public List<Profile> profiles { get; set;}

    private void Start()
    {
        Initialize();
    }

    private void LateUpdate()
    {
        SetTurns();
    }

    public void Initialize()
    {
        ProfileManage.instance = this;
        profiles = new List<Profile>();
    }

    public void AddProfile(Profile profile)
    {
        profile.nickname = photonView.Owner.NickName;

        if (profile.nickname == "")
        {
            photonView.Owner.NickName = "Player " + Random.Range(10001, 99999);
            profile.nickname = photonView.Owner.NickName;
        }
        ProfileManage.instance.profiles.Add(profile);
    }

    public void SetTurns()
    {
        if (profiles.Count == 0) return;
        int index = 0;
        foreach (Profile profile in profiles)
        {
            profile.Team = index;
            UIProfile.instance.SetProfile(index, profile.nickname, profile.picture, false);
            index++;
        }
    }

    public Profile GetProfile(int index)
    {
        throw new System.NotImplementedException();
    }
}
