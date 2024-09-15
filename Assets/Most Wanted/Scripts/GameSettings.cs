using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    public static void LoadScene( SceneReference scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void LoadScene(int value)
    {
        SceneManager.LoadScene(value);
    }
    public void ExitApp()
    {
        Application.Quit();
    }


}
