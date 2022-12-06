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

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider secondHealthSlider;

    [SerializeField] HealthController healthController;

    [SerializeField] RectTransform sliderValuePosition;
    [SerializeField] RectTransform secondSliderValuePosistion;

    public bool canMaximize;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (GameObject wall in invisibleWalls)
            {
                wall.SetActive(true);
            }

            canMaximize = true;

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

        PutMaxHealth();

        vcam.Follow = playerTarget;
        bossCanvas.SetActive(false);

        //playerCamera.SetActive(true);
        //bossCamera.SetActive(false);
    }

    public void PutMaxHealth()
    {
        if (canMaximize)
        {
            canMaximize = false;

            healthController.maxHealth++;

            sliderValuePosition.transform.localPosition = new Vector2(sliderValuePosition.transform.localPosition.x + 50, sliderValuePosition.transform.localPosition.y);
            sliderValuePosition.transform.localScale = new Vector2(sliderValuePosition.transform.localScale.x + 0.3f, sliderValuePosition.transform.localScale.y);

            secondSliderValuePosistion.transform.localPosition = new Vector2(secondSliderValuePosistion.transform.localPosition.x + 50, secondSliderValuePosistion.transform.localPosition.y);
            secondSliderValuePosistion.transform.localScale = new Vector2(secondSliderValuePosistion.transform.localScale.x + 0.3f, secondSliderValuePosistion.transform.localScale.y);

            healthSlider.maxValue = healthController.maxHealth;
            secondHealthSlider.maxValue = healthController.maxHealth;

            healthController.healthPoints = healthController.maxHealth;

            healthSlider.value = healthController.healthPoints;
            secondHealthSlider.value = healthController.healthPoints;
        }
        
    }
}
