using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DustParticleUI : MonoBehaviour
{
    public int particleCount = 20;
    public Sprite particleSprite;
    public Color particleColor = new Color(0.83f, 0.67f, 0.44f, 0.6f);
    public float minSize = 5f;
    public float maxSize = 15f;
    public float minSpeed = 20f;
    public float maxSpeed = 60f;

    private List<RectTransform> particles = new List<RectTransform>();
    private List<Vector2> velocities = new List<Vector2>();
    private RectTransform canvasRect;

    void Start()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        for (int i = 0; i < particleCount; i++)
        {
            GameObject p = new GameObject("Dust_" + i);
            p.transform.SetParent(transform, false);

            Image img = p.AddComponent<Image>();
            if (particleSprite != null)
                img.sprite = particleSprite;
            img.color = particleColor;

            RectTransform rt = p.GetComponent<RectTransform>();
            float size = Random.Range(minSize, maxSize);
            rt.sizeDelta = new Vector2(size, size);
            rt.anchoredPosition = new Vector2(
                Random.Range(-canvasRect.rect.width / 2, canvasRect.rect.width / 2),
                Random.Range(-canvasRect.rect.height / 2, canvasRect.rect.height / 2)
            );

            particles.Add(rt);
            velocities.Add(new Vector2(Random.Range(minSpeed, maxSpeed), Random.Range(minSpeed / 2, maxSpeed / 2)));
        }
    }

    void Update()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].anchoredPosition += velocities[i] * Time.deltaTime;

            if (particles[i].anchoredPosition.x > canvasRect.rect.width / 2)
                particles[i].anchoredPosition = new Vector2(-canvasRect.rect.width / 2, particles[i].anchoredPosition.y);
            if (particles[i].anchoredPosition.y > canvasRect.rect.height / 2)
                particles[i].anchoredPosition = new Vector2(particles[i].anchoredPosition.x, -canvasRect.rect.height / 2);
        }
    }
}