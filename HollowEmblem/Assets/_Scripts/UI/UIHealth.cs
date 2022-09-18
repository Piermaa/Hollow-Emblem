using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealth : MonoBehaviour
{
    public GameObject[] hearths;
    public GameObject[] emptyHearts;
    [SerializeField] HealthController healthController;
    private void Update()
    {
        UpdateMaxHealth();
        UpdateHealth();
    }

    public void UpdateMaxHealth()
    {

        for (int i = 0; i < healthController.maxHealth; i++)
        {
            emptyHearts[i].SetActive(true);
        }
       
    }

    public void UpdateHealth()
    {

        for (int i = 0; i+1 <= healthController.healthPoints; i++)
        {
            hearths[i].SetActive(true);
        }
        for (int i = healthController.maxHealth +1; i  >= healthController.healthPoints; i--)
        {
            hearths[i].SetActive(false);
        }
    }
}
