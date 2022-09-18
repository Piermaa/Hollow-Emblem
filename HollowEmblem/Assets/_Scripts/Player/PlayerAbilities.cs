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

    bool willDestroy;
    public bool slamUnlocked;

    public float slamForce;

    public float slamCD = 3;
    float slamTimer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        slamTimer -= Time.deltaTime;

        if(Input.GetButtonDown("Crouch") && slamUnlocked && slamTimer<0)
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
}
