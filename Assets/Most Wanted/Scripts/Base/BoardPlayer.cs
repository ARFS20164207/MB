using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class BoardPlayer : MonoBehaviourPun, IPunObservable
{
    public Player player;
    public BoardTeam myTeam;
    public int ID = 0;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Envía datos al otro jugador (se ejecuta en el jugador que envía los datos)
            BTeams selectedTeam = myTeam.myTeam;
            stream.SendNext((int)selectedTeam);
     
    //stream.SendNext(transform.rotation);
}
        else
        {
            // Recibe datos del otro jugador (se ejecuta en el jugador que recibe los datos)

            myTeam.myTeam = (BTeams)(int)stream.ReceiveNext();
            //syncedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
