using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    [Header("Shake Settings")]
    [SerializeField] private float defaultDuration = 0.3f;
    [SerializeField] private float defaultMagnitude = 0.3f;

    private Vector3 originalPosition;
    private Coroutine currentShake;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    // Varsayılan değerlerle sallanma
    public void Shake()
    {
        Shake(defaultDuration, defaultMagnitude);
    }

    // Özel değerlerle sallanma
    public void Shake(float duration, float magnitude)
    {
        if (currentShake != null)
            StopCoroutine(currentShake);

        currentShake = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(
                originalPosition.x + x,
                originalPosition.y + y,
                originalPosition.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        currentShake = null;
    }
}