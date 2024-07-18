using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RuleManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private Image container;
    [SerializeField] SceneReference startScene;
    private int index = 0;
    // Start is called before the first frame update
    
    public void OnBtnNext()
    {
        index = (index + 1) % sprites.Count;
        container.sprite = sprites[index];
        print(index);
    }
    public void OnBtnPrevious()
    {
        index = (index - 1 + sprites.Count) % sprites.Count;
        container.sprite = sprites[index];
        print(index);

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(startScene);
    }
}
