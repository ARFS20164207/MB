using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class PhotonEventManager : MonoBehaviourPunCallbacks
{
    // Delegados y eventos personalizados para las acciones relacionadas con Photon
    //public delegate void PlayerEnteredRoomEvent(Player newPlayer);
    //public static event PlayerEnteredRoomEvent OnPlayerEnteredRoom;

    //public delegate void PlayerLeftRoomEvent(Player otherPlayer);
    //public static event PlayerLeftRoomEvent OnPlayerLeftRoom;

    // Otros eventos personalizados relacionados con Photon
    public static UnityEvent OnConnectedToPhoton;
    public static UnityEvent OnDisconnectedFromPhoton;
    public static UnityEvent OnJoinedRoom0;
    public static UnityEvent OnLeftRoom0;

    void Start()
    {
        // Conectar los m�todos a los eventos de Photon
        //OnPlayerEnteredRoom += HandlePlayerEnteredRoom;
        //OnPlayerLeftRoom += HandlePlayerLeftRoom;

        OnConnectedToPhoton = new UnityEvent();
        OnDisconnectedFromPhoton = new UnityEvent();
        OnJoinedRoom0 = new UnityEvent();
        OnLeftRoom0 = new UnityEvent();

        // Conectarse al servidor de Photon al inicio
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor de Photon.");
        OnConnectedToPhoton.Invoke();

        // Crear o unirse a una sala cuando se conecta al servidor
        PhotonNetwork.JoinOrCreateRoom("MiSala", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Desconectado de Photon debido a: {cause}");
        OnDisconnectedFromPhoton.Invoke();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Entraste a la sala: {PhotonNetwork.CurrentRoom.Name}");
        OnJoinedRoom0.Invoke();
    }

    /*public override void OnPlayerEnteredR(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} entr� a la sala.");
        OnPlayerEnteredRoom?.Invoke(newPlayer);
    }*/

    /*public override void OnPlayerLeftR(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} sali� de la sala.");
        OnPlayerLeftRoom?.Invoke(otherPlayer);
    }*/

    public override void OnLeftRoom()
    {
        Debug.Log("Saliste de la sala.");
        OnLeftRoom0.Invoke();
    }

    // M�todos que manejan los eventos de Photon
    private void HandlePlayerEnteredRoom(Player newPlayer)
    {
        // L�gica para manejar la entrada de un jugador a la sala
    }

    private void HandlePlayerLeftRoom(Player otherPlayer)
    {
        // L�gica para manejar la salida de un jugador de la sala
    }
}
