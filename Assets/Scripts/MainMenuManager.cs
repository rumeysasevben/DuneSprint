using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(SceneFader.instance.FadeOut(1));
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}