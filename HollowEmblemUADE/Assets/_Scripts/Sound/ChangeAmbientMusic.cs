using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAmbientMusic : MonoBehaviour
{
    [SerializeField] GameObject[] bosses;

    public AudioSource audioSource;

    [SerializeField] AudioClip ambientClip;
    [SerializeField] AudioClip spiderBossFightMusic;
    [SerializeField] AudioClip slamBossFightMusic;
    [SerializeField] AudioClip finalBossFightMusic;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeSong()
    {
        if (ChangeCameraPosition.bossIsActive)
        {
            if (bosses[0] != null)
            {
                if (bosses[0].activeInHierarchy)
                {
                    audioSource.clip = spiderBossFightMusic;
                    audioSource.Play();
                    print("boss1plays");
                }


            }

            if (bosses[1] != null)
            {
                if (bosses[1].activeInHierarchy)
                {
                    audioSource.clip = slamBossFightMusic;
                    audioSource.Play();
                }
            }

            if (bosses[2] != null)
            {
                if (bosses[2].activeInHierarchy)
                {
                    audioSource.clip = finalBossFightMusic;
                    audioSource.Play();
                }
            }
        }

        else
        {
            audioSource.clip = ambientClip;
            audioSource.Play();
        }
    }
}
