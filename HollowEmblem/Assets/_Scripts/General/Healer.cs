using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    int healingAmount=2;

    bool canHeal;
    public GameObject healer;
    public float healCD;
    float healTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healTimer -= Time.deltaTime;
        if(healTimer<0)
        {
            canHeal = true;
        }
        healer.SetActive(canHeal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&canHeal)
        {
           
                HealthController health;
                collision.TryGetComponent<HealthController>(out health);
      
                canHeal = false;
                health.healthPoints += healingAmount;
                healTimer = healCD;
                print("playerHeaLED:");
            
        }
    }
}
