using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { SEARCHING, CHARGING, EMBISTING, RECOVERING }

public class Boss : MonoBehaviour
{
    BattleState state;

    public Animator animator;

    public GameObject embistingTrigger;

    public BoxCollider2D damageCollider;

    [Header("Raycast")]
    RaycastHit2D playerRc;
    RaycastHit2D wallRc;

    [Header("GameObject")]
    public GameObject leftSide;
    public GameObject rightSide;
    public GameObject waypointA;
    public GameObject waypointB;

    [Header("Bools")]
    public bool isRight;
    public bool goingRight;

    [Header("Transforms")]
    public Transform seekPlayerStart;
    public Transform playerTransform;

    [Header("Floats")]
    private float distanceOfRay = 25f;
    public float distanceOfWallRay = 0.5f;
    public float speed = 7.5f;
    public float embistingSpeed = 10f;
    public float backSpeed = 0.1f;

    [Header("LayerMasks")]
    public LayerMask playerLayer;
    public LayerMask spikeLayer;

    private void Awake()
    {
        damageCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.SetActive(true);
        embistingTrigger.SetActive(false);
        goingRight = true;
        state = BattleState.SEARCHING;
    }

    private void Update()
    {
        //Debug.DrawRay(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left) + new Vector3(distanceOfRay, 0, 0), Color.blue);
        //Debug.DrawRay(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left) + new Vector3(distanceOfWallRay, 0, 0), Color.red);
        BossStateExecution();
    }

    IEnumerator Searching()
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 theScale = transform.localScale;

        if (playerRc = Physics2D.Raycast(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left), distanceOfRay, playerLayer))
        {
            if (goingRight)
            {
                state = BattleState.CHARGING;
            }

            else
            {
                state = BattleState.CHARGING;
            } 
        }

        else
        {
            animator.SetBool("Walk", true);

            if (goingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypointA.transform.position, speed * Time.deltaTime);
            }

            else
            {
                transform.position = Vector2.MoveTowards(transform.position, waypointB.transform.position, speed * Time.deltaTime);
            }

            if (transform.position.x == waypointA.transform.position.x)
            {
                goingRight = false;
                theScale.x *= -1;
                transform.localScale = theScale;
            }

            else if (transform.position.x == waypointB.transform.position.x)
            {
                goingRight = true;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }

        yield return null;
    }

    IEnumerator Charging()
    {
        animator.SetBool("Walk", false);
        damageCollider.enabled = false;

        if (goingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 10);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 0.006f, transform.position.y), backSpeed * Time.deltaTime);
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -10);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 0.006f, transform.position.y), backSpeed * Time.deltaTime);
        }
        
        yield return new WaitForSeconds(2f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        state = BattleState.EMBISTING;

        yield return null;
    }

    void Embisting()
    {
        damageCollider.enabled = true;

        if (wallRc = Physics2D.Raycast(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left), distanceOfWallRay, spikeLayer))
        {
            animator.SetBool("Walk", false);
            state = BattleState.RECOVERING;
        }

        else
        {
            animator.SetBool("Walk", true);
            embistingTrigger.SetActive(true);

            if (goingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, rightSide.transform.position, embistingSpeed * Time.deltaTime);
            }

            else
            {
                transform.position = Vector2.MoveTowards(transform.position, leftSide.transform.position, embistingSpeed * Time.deltaTime);
            }
            
        }
    }

    IEnumerator Recovering()
    {
        embistingTrigger.SetActive(false);
        animator.SetBool("Walk", false);
        
        if (goingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 10);
        }
        
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -10);
        }

        yield return new WaitForSeconds(3f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        state = BattleState.SEARCHING;

        yield return null;
    }

    void BossStateExecution()
    {
        switch (state)
        {
            case BattleState.SEARCHING:
                StartCoroutine(Searching());
                break;

            case BattleState.CHARGING:
                StartCoroutine(Charging());
                break;

            case BattleState.EMBISTING:
                Embisting();
                break;

            case BattleState.RECOVERING:
                StartCoroutine(Recovering());
                break;
        }
    }

}
