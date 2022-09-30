using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAdder : MonoBehaviour
{
    public int healthToAdd; 


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.TryGetComponent<HealthController>(out HealthController healthController);
            healthController.AddMaxHealth(healthToAdd);
            this.gameObject.SetActive(false);
        }
    }
}
