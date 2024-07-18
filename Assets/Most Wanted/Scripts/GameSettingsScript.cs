using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameSetting", menuName = "MostWanted/GameSetting")]
public class GameSettingsScript : ScriptableObject
{
    ScreenOrientation screenModeOrientation = ScreenOrientation.LandscapeLeft;

    public void SetOrientation() =>  Screen.orientation = screenModeOrientation;
    public void LoadScene(int value) => SceneManager.LoadScene(value);
    public void ExitApp() => Application.Quit();
    


}
