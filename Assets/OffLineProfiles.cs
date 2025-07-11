using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffLineProfiles : MonoBehaviour , ProfileManage
{
    public List<Profile> profiles {get;set;}

    private void Start()
    {
        Initialize();
    }

    private void LateUpdate()
    {
        SetTurns();
    }
    [ContextMenu("Initialize")]
    public void Initialize()
    {
        profiles = new List<Profile>();
        ProfileManage.instance = this;
    }

    public void AddProfile(Profile profile)
    {
        if (profile.nickname == "")
        {
            profile.nickname = "Player " + Random.Range(10001, 99999);
        }

        if(profiles.Count == 0)
        {
            profile.myTeam = BTeams.Fist;
        }
        else { profile.myTeam = BTeams.Second; }

        ProfileManage.instance.profiles.Add(profile);
    }

    public void SetTurns()
    {
        if (profiles.Count == 0) return;
        int index = 0;
        foreach (Profile profile in profiles)
        {
            profile.Team = index;
            UIProfile.instance.SetProfile(index, profile.nickname, profile.picture, profile.isTurn);
            index++;
        }
    }

    public Profile GetProfile(int index)
    {
        if (profiles.Count == 0) return null;
        if (index < 0) { index = (index % profiles.Count) + profiles.Count; }
        return profiles[index% profiles.Count];
    }
}
