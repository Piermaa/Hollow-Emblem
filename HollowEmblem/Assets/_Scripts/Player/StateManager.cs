using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    CharacterController2D controller2D;
    private PlayerMovement playerMovement;
    private LiquidMovement liquidMovement;
    [SerializeField]  private SpriteRenderer sprite;
    private Rigidbody2D rb2D;
    private PlayerCombat playerCombat;
  
    public bool nearToLiquid;

    Animator animator;
    [SerializeField]GameObject slime;

    [SerializeField] PlayerSounds sounds;
    private void Start()
    {

        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        liquidMovement = GetComponent<LiquidMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        if (sprite==null)
        {
            sprite = GetComponent<SpriteRenderer>();
        } 
        rb2D = GetComponent<Rigidbody2D>();
        controller2D = GetComponent<CharacterController2D>();
      
    }
    public enum PlayerState
    {
        Solid,
        Liquid
    }
    public PlayerState state;


    void Update()
    {
        if (nearToLiquid && Input.GetKeyDown(KeyCode.Space))
        {
            sounds.PlaySound(sounds.slime);

            if (state == PlayerState.Solid)
            {
                StateChange(PlayerState.Liquid);
                print(state);
            }
            else
            {
                StateChange(PlayerState.Solid);
                print(state);
            }
          
        }
    }

    public void StateChange(PlayerState playerState)
    {
        state = playerState;
        rb2D.constraints = RigidbodyConstraints2D.FreezeAll;

        switch (playerState)
        {
            case PlayerState.Liquid:
                playerMovement.enabled = false;
                liquidMovement.enabled = true;
                controller2D.enabled = false;
                sprite.enabled = false;
                rb2D.gravityScale = 0;
                playerCombat.attackPoint = playerCombat.playerCenter;
                animator.enabled = false;
                slime.SetActive(true);
                //rb2D.bodyType = RigidbodyType2D.Kinematic;
                break;

            case PlayerState.Solid:
                playerMovement.enabled = true;
                liquidMovement.enabled = false;
                controller2D.enabled = true;
                sprite.enabled = true;
                rb2D.gravityScale = 2;
                playerCombat.attackPoint = playerCombat.attackPosition;
                animator.enabled = true;
                slime.SetActive(false);
                //rb2D.bodyType = RigidbodyType2D.Dynamic;
                break;
        }

        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Liquid"))
        {
            nearToLiquid = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Liquid"))
        {
            nearToLiquid = false;
            if (state!=PlayerState.Solid)
            {
                StateChange(PlayerState.Solid);
            }
        }
    }
}
