using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("Center")]
    [Header("-Lobby")]
    [SerializeField] Button btnOnline;
    [Header("Down")]
    [Header("-Backlogs")]
    [SerializeField] TextMeshProUGUI txtLogs;
    [SerializeField] TextMeshProUGUI txtLogsDescriptions;
    [SerializeField] Image imgConnectButton;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PrintLog(string state, string context, string description)
    {
        if (txtLogs == null || txtLogsDescriptions == null) { return; }
        txtLogs.text = "";
        txtLogsDescriptions.text = "";
        txtLogs.text += "Status: " + state + " \n";
        txtLogs.text += "Context: " + context + " \n";
        txtLogsDescriptions.text += "Description: " + description + " \n";
    }
    public void SetOnlineBtn(bool enable = true)
    {
        if (btnOnline == null) { return; }
        btnOnline.interactable = enable;
    }
    public void SetState(NetworkState netState)
    {
        switch (netState)
        {
            case NetworkState.Disconected:
                imgConnectButton.color = Color.red;
                break;
            case NetworkState.OnLobby:
                imgConnectButton.color = Color.green;
                break;
            case NetworkState.OnRoom:
                imgConnectButton.color = Color.green;
                break;
        }

    }
}
