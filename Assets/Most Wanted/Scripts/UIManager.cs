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
    [Header("Left")]
    [SerializeField] TextMeshProUGUI txtCoin1;
    [SerializeField] TextMeshProUGUI txtCoin2;
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
    public void SetCoin(int coin1, int coin2)
    {
        if (txtCoin1 == null || txtCoin2 == null) { return; }
        txtCoin1.text = coin1.ToString();
        txtCoin2.text = coin2.ToString();
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
