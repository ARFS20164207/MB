using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTeam : MonoBehaviour
{
    public string profileName = "Default";
    public  Texture2D Profile;
    public BTeams myTeam;
    public bool myTurn = false;
}
public enum BTeams
{
    Fist = 0,
    Second = 1
}
