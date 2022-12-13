using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAmbientMusic : MonoBehaviour
{
    //[SerializeField] GameObject[] bosses;

    private AudioSource audioSource;

    [SerializeField] AudioClip ambientClip;
    [SerializeField] AudioClip bossFightMusic;
    [SerializeField] AudioClip finalBossFightMusic;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeSong()
    {
        if (ChangeCameraPosition.bossIsActive)
        {
            audioSource.clip = bossFightMusic;
            audioSource.Play();
        }

        else
        {
            audioSource.clip = ambientClip;
            audioSource.Play();
        }
    }
}
