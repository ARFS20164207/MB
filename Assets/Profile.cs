using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile : MonoBehaviourPun
{
    public string nickname;
    public Sprite picture;
    public BTeams myTeam;
    public bool isTurn;
    public int Team;


    // Start is called before the first frame update
    void Start()
    {
        GetInstanceProfileManage();
    }
    private void GetInstanceProfileManage()
    {
        try
        {
            ProfileManage.instance.AddProfile(this);
        }
        catch (System.Exception)
        {
            Invoke(nameof(GetInstanceProfileManage), 1);
            throw;
        }
    }
}
public enum BTeams
{
    Fist = 0,
    Second = 1
}
