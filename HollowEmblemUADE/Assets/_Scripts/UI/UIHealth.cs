using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public GameObject[] emptyHearts;

    public List<Image> healthSprites = new List<Image>();
    [SerializeField] HealthController healthController;

    private void Start()
    {
        UpdateMaxHealth();
       
    }
    private void Update()
    {
        UpdateHealth();
    }

    public void UpdateMaxHealth()
    {
        healthSprites.Clear();
        for (int i = 0; i < healthController.maxHealth; i++)
        {
            emptyHearts[i].SetActive(true);
            healthSprites.Add(emptyHearts[i].GetComponent<Image>());
        }
    }

    public void UpdateHealth()
    {

        for (int i = 0; i + 1 <= healthController.healthPoints; i++)
        {
            healthSprites[i].color = Color.cyan;
        }

        for (int i = healthController.maxHealth-1; i >= healthController.healthPoints; i--)
        {
            healthSprites[i].color = Color.white;
        }

    }
}
