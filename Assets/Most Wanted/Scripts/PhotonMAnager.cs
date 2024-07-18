using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PhotonMAnager : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonMAnagerScript connect;
    public static PhotonMAnager instance;

    public void ConnectPhoton()
    {
        if (PhotonNetwork.IsConnected) return;
        UIManager.instance.SetState(netState: NetworkState.Disconected);
        PhotonNetwork.ConnectUsingSettings();
        UIManager.instance.PrintLog("Conectando", "Local", "intentando conectar al servidor");
    }
    private void Awake()
    {
        if (instance == null)
        {
            // Si no hay una instancia todavía, establece esta como la instancia
            instance = this;
            // No destruye este objeto cuando se carga una nueva escena
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // Si ya existe una instancia y no somos esa instancia, destruye este objeto
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if(connect.connectOnStart) { ConnectPhoton(); }
    }

    public override void OnConnectedToMaster()
    {
        UIManager.instance.SetState(netState: NetworkState.OnLobby);
        UIManager.instance.PrintLog("Conectado", "En Lobby", "intentando conectar a un Room");
        UIManager.instance.SetOnlineBtn();
    }
    public void FindRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.EmptyRoomTtl = 2000;
        roomOptions.PublishUserId = true;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public override void OnCreatedRoom()
    {
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }
    public override void OnJoinedRoom()
    {
        UIManager.instance.PrintLog("Conectado", "En Room", "Sincronizando con otros jugadores");
        UIManager.instance.SetState(netState: NetworkState.OnRoom);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }
    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        base.OnCustomAuthenticationResponse(data);
    }
    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        base.OnCustomAuthenticationFailed(debugMessage);
    }
    public override void OnConnected()
    {
        base.OnConnected();
        UIManager.instance.SetState(netState: NetworkState.OnLobby);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        UIManager.instance.PrintLog("Desconectado", "Local", cause.ToString());
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
public enum NetworkState
{
    Disconected,
    OnLobby,
    OnRoom,
    AFK,
    Paused

}
