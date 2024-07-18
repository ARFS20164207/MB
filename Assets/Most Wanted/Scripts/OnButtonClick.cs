using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButtonClick : MonoBehaviour
{
    Button button;

    [Header("Change Scene Action")]
    [SerializeField] bool enableNextScene = true;
    [SerializeField] SceneReference nextScene;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if(button == null)
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClickChangeScene);
        } 
    }

   private void OnClickChangeScene()
    {
        if (!enableNextScene) return;
        GameSettings.LoadScene(nextScene);
    }
}
