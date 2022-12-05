using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FBBattleState { SPAWNING, IDLE, MELEEATTACK, FLOORATTACK, SHOOTATTACK};

public class FinalBossScript : MonoBehaviour
{
    public FBBattleState state;

    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField]
    Animator[] floorSpots;

    private RaycastHit2D hit;
    public LayerMask playerLayer;
    [SerializeField] Animator player;



    [Header("Bools")]
    public bool canSpawning;
    public bool canIdle;
    public bool canMelee;
    public bool canFly;
    public bool canFloor;
    bool hasFloorAttacked;
    public bool canShoot;
    public bool isRight;
    public bool canChangeScale;
    public bool isFlying;

    private float distanceOfRay = -4f;
    private float speed = 3f;
    private float flySpeed = 5.5f;

    [Header ("Transform")]
    [SerializeField] Transform raycastStart;
    [SerializeField] Transform hangingSpot;
    [SerializeField] Transform floorSpot;

    public int attackIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        canSpawning = true;
        state = FBBattleState.SPAWNING;
    }

    void Update()
    {
        //Debug.DrawRay(raycastStart.position, raycastStart.TransformDirection(Vector2.left) + new Vector3(distanceOfRay, 0, 0), Color.green);
        BossStateExecution();
        ChangeScale();
    }

    IEnumerator SpawningAnimation()
    {
        yield return new WaitForSeconds(1f);
        canIdle = true;
        state = FBBattleState.IDLE;
        yield return null;
    }

    IEnumerator Vulnerable()
    {
        canMelee = true;

        yield return new WaitForSeconds(2f);

        attackIndex = Random.Range(0, 3);
        AttackExecution();
        canIdle = true;
        
        yield return null;
        
    }

    void MeleeAttackActivator()
    {
        if ((hit = Physics2D.Raycast(raycastStart.position, raycastStart.TransformDirection(Vector2.left), distanceOfRay, playerLayer)))
        {
            canChangeScale = false;
            animator.SetTrigger("MeleeAttack");
        }

        else
        {
            Vector2 playerVector = new Vector2(player.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, playerVector, speed * Time.deltaTime);
            //animator.SetBool("isWalking", true);
        }
    }

    void FloorAttackActivator()
    {
        if (transform.position == hangingSpot.transform.position)
        {
            if (!hasFloorAttacked)
            {
                Debug.Log("Yanosemuevee");
                animator.SetBool("Flying", false);
                animator.SetTrigger("FloorAttack");
                foreach (Animator anim in floorSpots)
                {
                    anim.SetTrigger("Attack");
                }
                hasFloorAttacked = true;
            }
        }
        else 
        {
                print("SE MUEVEEE");
                transform.position = Vector2.MoveTowards(transform.position, hangingSpot.transform.position, flySpeed * Time.deltaTime);
                if (!animator.GetBool("Flying"))
                {
                    animator.SetBool("Flying", true);
                }
        }
    }

    void ShootAttackActivator()
    {
        animator.SetTrigger("ShootAttack");
    }

    void BossStateExecution()
    {
        switch (state)
        {
            case FBBattleState.SPAWNING:
                if (canSpawning)
                {
                    canSpawning = false;
                    StartCoroutine(SpawningAnimation());
                }

                break;

            case FBBattleState.IDLE:

                if (canIdle)
                {
                    canFloor = true;
                    animator.SetBool("Flying", false);
                    //sr.sprite = idleSprite;
                    canIdle = false;
                    hasFloorAttacked = false;
                    StartCoroutine(Vulnerable());
                }
                else
                {
                    Vector2 toFloor = new Vector2(transform.position.x, floorSpot.transform.position.y);
                    //sr.sprite = flySprite;
                    if (Vector2.Distance(transform.position, toFloor) > 0.1f)
                    {
                        animator.SetBool("Flying", true);
                        transform.position = Vector2.MoveTowards(transform.position, toFloor, flySpeed * Time.deltaTime);
                    }
                    else 
                    {
                        canIdle = false;
                        hasFloorAttacked = false;
                       
                        canFloor = true;
                        animator.SetBool("Flying", false);
                    }
                  
                }  

                break;

            case FBBattleState.MELEEATTACK:
                if (canMelee)
                {
                    canMelee = false;   
                    MeleeAttackActivator();
                }

                break;

            case FBBattleState.FLOORATTACK:
                 FloorAttackActivator();
                
               
                    
                
                break;

            case FBBattleState.SHOOTATTACK:
                if (canShoot)
                {
                    canShoot = false;
                    ShootAttackActivator();
                }

                break;
        }
    }

    void AttackExecution()
    {
        switch (attackIndex)
        {
            case 0:
                state = FBBattleState.MELEEATTACK;
                break;

            case 1:
                state = FBBattleState.FLOORATTACK;
                break;

            case 2:
                state = FBBattleState.SHOOTATTACK;
                break;
        }
    }

    void ChangeScale()
    {
        if (canChangeScale)
        {
            isRight = (player.transform.position.x < transform.position.x);
            float dirMultpiplier = isRight ? -1 : 1;

            Vector3 theScale = transform.localScale;
            theScale.x = dirMultpiplier;
            transform.localScale = theScale;

            if (theScale.x < 0)
            {
                theScale.x = -0.3720472f;
            }

            else
            {
                theScale.x = 0.3720472f;
            }

            theScale = new Vector3(theScale.x, 0.3720472f, 0.3720472f);
            transform.localScale = theScale;
        }
    }

    public void PutIdle()
    {
        state = FBBattleState.IDLE;
    }
}
