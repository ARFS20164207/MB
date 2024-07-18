using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class MatchStats : MonoBehaviour
{
    public static MatchStats instance;
    public List<BoardPlayer> players = new List<BoardPlayer>();
    public string matchResult;


    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else if (instance != this) enabled = false;
        else DontDestroyOnLoad(gameObject);
    }
    public BoardPlayer GetLocalPlayer()
    {
        foreach (var player in players)
        {
            if(player.photonView.IsMine) return player;
        }
        return null;
    }
    public BoardPlayer GetOnlinePlayer()
    {
        foreach (var player in players)
        {
            if (!player.photonView.IsMine) return player;
        }
        return null;
    }
}
