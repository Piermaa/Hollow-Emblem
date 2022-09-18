using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaController : MonoBehaviour
{
    public int stamina=5;
    int slammed = 2;
    public void ParryReceived(int staminaLoss)
    {
        stamina -= staminaLoss;
    }
    public bool CheckStamina()
    {
        if (stamina>0)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Slam"))
        {
            stamina -= slammed;
        }
    }
}
