using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject canvas;

    public GameObject panelMenu;
    public GameObject panelOptions;

    public GameManager gameManager;
    public static SceneChanger Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        IniciarComponentes();
        canvas.SetActive(true);
        panelMenu.SetActive(true);
        if (panelOptions != null)
        {
            panelOptions.SetActive(false);
        }

        gameManager = FindObjectOfType<GameManager>();
    }

    public void IniciarComponentes()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void Iniciar()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void ReturnToMainMenu()
    {
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        SceneManager.LoadScene(0, LoadSceneMode.Single);

    }

    public void Options()
    {
        panelMenu.SetActive(false);
        if (panelOptions!=null)
        {
            panelOptions.SetActive(true);
        }
      
    }

    public void Volver()
    {
        panelMenu.SetActive(true);
        if (panelOptions != null)
        {
            panelOptions.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void Resume()
    {
        gameManager.Resume();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif
    }

    public void GameOver()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}

