using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkManager", menuName = "MostWanted/NetworkManager")]
public class NetworkManagerScript : ScriptableObject
{
    public bool startAsHost = true;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
    }
}