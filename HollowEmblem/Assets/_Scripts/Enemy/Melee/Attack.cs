using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] float attackTime;
    [SerializeField] Animator animator;
    [SerializeField] StaminaController _staminaController;
    [SerializeField] BasicIA ia;

    [SerializeField] float attackCooldown=5;
    [SerializeField]float attackTimer;

    [SerializeField] AudioSource attackSound;
    bool canAttack;
    public bool playerInRange;
    private void Start()
    {
        attackTimer = attackCooldown;
    }
    private void Update()
    {
        if (attackTimer < 0)
        {
            canAttack = true;
     
        }
        else 
        {
            attackTimer -= Time.deltaTime;
        }

        if(playerInRange&&canAttack)
        {
            MeleeAttack();
        }
    }

 
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            print("playerinrange");
            playerInRange = true;  
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("playerexit;");
            playerInRange = false;
        }
    }

    void MeleeAttack()
    {
      
        if (_staminaController.CheckStamina()&&canAttack)
        {
            ia.StopWalking(attackTime); //the enemy will stop walkinguntil the attackTime expires
            animator.SetTrigger("Attack");
            canAttack = false;
            attackTimer = attackCooldown;

            attackSound.Play();
        }
    }
}
