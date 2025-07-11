using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchMaking : MonoBehaviourPunCallbacks
{
    [SerializeField] BoardPlayer prefabPlayer;
    [SerializeField] List<BoardPlayer> playerList;
    [SerializeField] List<ProfileVisuals> playerVisuals;
    [SerializeField] SceneReference OnlineGame;
    public void FindRoom()
    {
        if (PhotonNetwork.InRoom) return;
        PhotonMAnager.instance.FindRoom();

    }
    public void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2) return;
        foreach (var player in playerVisuals) { if (!player.isPlayerReady) return; }
        MatchStats.instance.players = playerList;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(OnlineGame);
        }

    }
    public override void OnJoinedRoom()
    {
        print("Ingresando a la Sala");
        base.OnJoinedRoom();
        if (!PhotonNetwork.IsConnected) return;
        GameObject go = PhotonNetwork.Instantiate(prefabPlayer.name, transform.position, Quaternion.identity);

        BoardPlayer boardPlayer = go.GetComponent<BoardPlayer>();
        if (boardPlayer == null) return;
        Profile boardTeam = boardPlayer.myTeam;
        DontDestroyOnLoad(boardPlayer.gameObject);
        boardTeam.myTeam = PhotonNetwork.IsMasterClient ? BTeams.Fist : BTeams.Second;
        boardPlayer.name = "Player No. " + PhotonNetwork.LocalPlayer.ActorNumber + " : " + PhotonNetwork.LocalPlayer.NickName;
        boardTeam.nickname = boardPlayer.name;
        boardPlayer.ID = PhotonNetwork.LocalPlayer.ActorNumber;



        photonView.RPC("IntanciateNewPlayer", RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer);
        photonView.RPC("UpdatePlayers", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        foreach (ProfileVisuals player in playerVisuals)
        {
            player.SetReadyLocal(false);
            if (player.playerID == otherPlayer.ActorNumber)
            {
                player.SetName("...");
                player.playerID = 0;
            }
        }
    }
    [PunRPC]
    public void IntanciateNewPlayer(Player actor)
    {
        BoardPlayer[] players = FindObjectsOfType<BoardPlayer>();

        BoardPlayer boardPlayer = null;
        foreach (BoardPlayer player in players)
        {
            if (player.photonView.Owner == actor) boardPlayer = player;
        }
        if (boardPlayer == null) return;
        Profile boardTeam = boardPlayer.myTeam;
        DontDestroyOnLoad(boardPlayer.gameObject);
        boardTeam.myTeam = actor.IsMasterClient ? BTeams.Fist : BTeams.Second;
        boardPlayer.name = "Player No. " + actor.ActorNumber + " : " + actor.NickName;
        boardTeam.nickname = boardPlayer.name;
        boardPlayer.ID = actor.ActorNumber;
    }
    [PunRPC]
    public void UpdatePlayers(Player playerID)
    {

        BoardPlayer[] players = FindObjectsOfType<BoardPlayer>();

        foreach (BoardPlayer player in players)
        {
            if (player.photonView.IsMine)
            {
                ProfileVisuals tempProfile = playerVisuals[0];
                tempProfile.SetName(player.name + (" *"));
                tempProfile.playerID = player.ID;
                playerList.Add(player);
            }
            else
            {
                ProfileVisuals tempProfile = playerVisuals[1];
                tempProfile.SetName(player.name);
                tempProfile.playerID = player.ID;
                playerList.Add(player);
                //tempProfile.photonView.TransferOwnership(playerID);
            }

        }



    }

    public ProfileVisuals GetVisualByID(int finder) { return playerVisuals.First(x => x.playerID == finder); }


    public void SetVisualReady()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 0)
        {

        }
    }

    IEnumerator WaitingForPlayer()
    {
        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom.PlayerCount == 2);
        MatchStats.instance.players = playerList;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(OnlineGame);
        }
    }
}
