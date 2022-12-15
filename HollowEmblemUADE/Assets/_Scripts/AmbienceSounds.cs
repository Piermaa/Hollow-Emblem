using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSounds : MonoBehaviour
{

    private AudioSource audioSource;
    [SerializeField] AudioClip[] sounds;

    public float cooldown = 15f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CooldownUpdate();
    }

    void CooldownUpdate()
    {
        if (!ChangeCameraPosition.bossIsActive)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0)
            {
                cooldown = 15f;

                audioSource.clip = sounds[Random.Range(0, 11)];
                audioSource.Play();
            }
        }
    }
}
