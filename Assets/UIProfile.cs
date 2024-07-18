using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProfile : MonoBehaviour
{
    public List<Image> profileImages = new List<Image>();
    public List<TMP_Text> profileNicknames = new List<TMP_Text>();
    public List<Image> profileTurns = new List<Image>();

    public static UIProfile instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void SetProfile(int index,string nickname = "",Sprite image = null,bool isturn = false)
    {
        if (index >= profileImages.Count) return;

        profileNicknames[index].text = nickname;
        profileImages[index].sprite = image;
        profileTurns[index].color = isturn? Color.green : Color.red;

    }

}
