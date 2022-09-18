using UnityEngine;

public class GameManager : MonoBehaviour
{
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
}