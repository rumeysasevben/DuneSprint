using System.Collections;
using UnityEngine;

public class UIIntroAnimation : MonoBehaviour
{
    public RectTransform titleText;
    public float animDuration = 0.6f;

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        Vector2 titleEnd = titleText.anchoredPosition;
        Vector2 titleStart = new Vector2(titleEnd.x, titleEnd.y + 300f);

        titleText.anchoredPosition = titleStart;

        float t = 0;
        while (t < animDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / animDuration);
            titleText.anchoredPosition = Vector2.Lerp(titleStart, titleEnd, normalized);
            yield return null;
        }
        titleText.anchoredPosition = titleEnd;
    }
}