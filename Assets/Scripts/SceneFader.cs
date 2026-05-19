using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, 1 - (t / fadeDuration));
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator FadeOut(int sceneIndex)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, t / fadeDuration);
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
    }
}