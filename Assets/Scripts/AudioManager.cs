using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip slideSound;
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private AudioClip coinCollectSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayJump()
    {
        PlaySFX(jumpSound);
    }

    public void PlaySlide()
    {
        PlaySFX(slideSound);
    }

    public void PlayImpact()
    {
        PlaySFX(impactSound);
    }

    public void PlayCoinCollect()
    {
        PlaySFX(coinCollectSound);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}