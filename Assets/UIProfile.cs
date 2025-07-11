using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Most_Wanted.Scripts.Base;
using Most_Wanted.Scripts.V2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProfile : MonoBehaviour
{
    public bool fixedCamera;
    public List<Image> profileImages = new List<Image>();
    public List<TMP_Text> profileNicknames = new List<TMP_Text>();
    public List<Image> profileTurns = new List<Image>();
    public CinemachineVirtualCamera p1, p2;
    
    public static UIProfile instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        BoardEvents.Instance.OnPlayerTurn.AddListener(OnPlayerTurn);
    }
    public void OnPlayerTurn(IPlayer player, bool state)
    {
        if (BoardGame.instance.player1 == player)
        {
            profileTurns[0].color = state? Color.green : Color.red;
            if(!fixedCamera)p1.gameObject.SetActive(state);
            profileTurns[1].color = !state? Color.green : Color.red;
            if(!fixedCamera)p2.gameObject.SetActive(!state);
        }
        else if(BoardGame.instance.player2 == player)
        {
            profileTurns[1].color = state? Color.green : Color.red;
            if(!fixedCamera)p2.gameObject.SetActive(state);
            profileTurns[0].color = !state? Color.green : Color.red;
            if(!fixedCamera)p1.gameObject.SetActive(!state);
        }
    }
    public void SetProfile(int index,string nickname = "",Sprite image = null,bool isturn = false)
    {
        if (index >= profileImages.Count) return;

        profileNicknames[index].text = nickname;
        profileImages[index].sprite = image;
        profileTurns[index].color = isturn? Color.green : Color.red;

    }

}
