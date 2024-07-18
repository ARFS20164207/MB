using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineProfiles : MonoBehaviourPunCallbacks
{
    public List<Profile> profiles;

    public static OnlineProfiles instance;

    // Start is called before the first frame update
    void Start()
    {
       instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        SetTurns();
    }

    void SetTurns()
    {
        int index = 0;
        foreach (Profile profile in profiles)
        {
            profile.Team = index;
            UIProfile.instance.SetProfile(index,profile.nickname,profile.picture,false);
            index++;
        }
    }
    
}
