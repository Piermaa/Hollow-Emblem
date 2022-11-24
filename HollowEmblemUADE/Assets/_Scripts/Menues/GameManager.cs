using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject victorySprite;
    [SerializeField] GameObject defeatSprite;


    public Animator animator;

    private static GameManager instance = null;

    public static GameManager Instance;
    
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

    public void Start()
    {
        Resume();
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void StartVictory()
    {
        animator.SetTrigger("Victory");
    }

    public void StartDefeat()
    {
        animator.SetTrigger("Defeat");
    }

}