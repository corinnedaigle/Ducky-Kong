using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource BGMSource;

    public AudioClip jumping;
    public AudioClip death;
    public AudioClip getCoin;

    public AudioClip attackbgm;
    public AudioClip bgm;


    private void Start()
    {
        SFXSource = GetComponent<AudioSource>();
        SFXSource.volume = 0.5f;

        BGMSource = GetComponent<AudioSource>();
        SFXSource.volume = 0.271f;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        BGMSource.PlayOneShot(clip);
    }
}