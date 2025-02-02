using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;

    public AudioClip jumping;
    public AudioClip death;

    private void Start()
    {
        SFXSource = GetComponent<AudioSource>();
        SFXSource.volume = 0.5f;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}