using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIncr : MonoBehaviour
{
    public int dmgToAdd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collision.gameObject.TryGetComponent<PlayerCombat>(out PlayerCombat playerCombat);

            playerCombat.damage+= dmgToAdd;
            this.gameObject.SetActive(false);
        }
    }
}

