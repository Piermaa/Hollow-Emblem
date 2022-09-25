using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerAbilities : MonoBehaviour
{

    [SerializeField] PlayerSounds sounds;

    CharacterController2D controller;
    PlayerMovement movement;
    public bool isParrying;
    Animator animator;
    Rigidbody2D rb;


    StateManager stateManager;
    public bool unlockAll;
    bool willDestroy;
    public bool slamUnlocked;

    public float slamForce;

    public float slamCD = 3;
    float slamTimer;
    private void Start()
    {
        stateManager = GetComponent<StateManager>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();
        if (unlockAll)
        {
            AutoUnlock();
        }
    }

    private void Update()
    {
        slamTimer -= Time.deltaTime;

        if(Input.GetButtonDown("Jump") && Input.GetAxis("Vertical") < 0 && slamUnlocked && slamTimer<0)
        {
            if (!controller.CheckGround())
            {
                animator.SetBool("Falling", true);
                rb.AddForce(Vector2.down*slamForce, ForceMode2D.Impulse);
                willDestroy = true;
                controller.MustSlam();
                slamTimer = slamCD;
            }
            //else
            //{
            //    animator.SetTrigger("Parry");
            //}
        }

        if(controller.CheckGround())
        {
            animator.SetBool("Falling", false);
            if (willDestroy)
            {
                animator.SetTrigger("Slam");
            }
         
        }
    }

    IEnumerator Destroy(GameObject ground)
    {
        ground.TryGetComponent<Animator>(out Animator gAnimator);
        
        
        gAnimator.SetTrigger("Destroy");
        yield return null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.collider.name);
        if ((collision.collider.tag == "Ground" || collision.collider.tag == "DestructibleGround") && willDestroy)
        {
            if (collision.collider.tag == "DestructibleGround")
            {
                StartCoroutine(Destroy(collision.gameObject));
              
            }
            sounds.PlaySound(sounds.slam);
        }

         willDestroy = false;
       
    }

    void AutoUnlock()
    {
        stateManager.enabled = enabled;
        slamUnlocked = true;
        TryGetComponent<PlayerMovement>(out var movement);
        movement.dashUnlocked = true;
    }

    public void AbilityUnlock(string unlockedAb)
    {
        switch (unlockedAb)
        {
            case "Slime":
                stateManager.enabled = enabled;
                break;
            case "Dash":
                TryGetComponent<PlayerMovement>(out var movement);
                movement.dashUnlocked = true;
                    break;
            case "Slam":
                slamUnlocked = true;
                break;
        }
    }
}
