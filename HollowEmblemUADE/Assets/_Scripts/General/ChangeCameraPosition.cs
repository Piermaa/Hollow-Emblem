using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ChangeCameraPosition : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] Transform bossTarget;
    [SerializeField] Transform playerTarget;



    [SerializeField] GameObject[] invisibleWalls;

    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject bossCamera;

    [SerializeField] GameObject boss;
    [SerializeField] GameObject bossCanvas;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (GameObject wall in invisibleWalls)
            {
                wall.SetActive(true);
            }

            collision.TryGetComponent<PlayerRespawn>(out var pr);
            pr.SetRespawn(transform.position);

            vcam.Follow = bossTarget;
            bossCanvas.SetActive (true);
            boss.SetActive(true);
            //playerCamera.transform.position = bossCamera.transform.position;
            //playerCamera.SetActive(false);
            //bossCamera.SetActive(true);
        }
    }

    public void SetPlayerCamera()
    {
        foreach (GameObject wall in invisibleWalls)
        {
            wall.SetActive(false);
        }

        vcam.Follow = playerTarget;
        bossCanvas.SetActive(false);
        //playerCamera.SetActive(true);
        //bossCamera.SetActive(false);
    }
}
