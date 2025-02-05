using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip bgm;
    public AudioClip attackbgm;
    public AudioClip jumping;
    public AudioClip getCoin;
    public AudioClip death;

    private float bgmResumeTime = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        bgmSource.volume = 0.3f;
        PlayBGM(bgm);
    }

    // Play normal BGM and resume from where it left off
    public void ResumeBGM()
    {
        if (bgmSource.clip == bgm)
        {
            bgmSource.time = bgmResumeTime;
            bgmSource.Play();
        }

    }

    // Pause BGM and store its playback time
    public void PauseBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmResumeTime = bgmSource.time;
            bgmSource.Pause();
        }
    }

    // Play a looping background music track
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmSource.clip != bgmClip)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    // Play a short sound effect
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}