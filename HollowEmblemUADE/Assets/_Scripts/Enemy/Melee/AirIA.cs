using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirIA : MonoBehaviour
{
    [Header ("Objects")]
    public Transform bulletOrigin;
    private Rigidbody2D rb;
    private Animator animator;
    private ObjectPooler objectPooler;

    [Header ("Patrolling Parameters")]
    public Transform[] moveSpots;
    [SerializeField] Transform next;
    public float stopTime;
    public float speed = 6;
    public float waitTime;
    private float startWaitTime = 1;
    private float speedAux;
    private int spotsIndex;
 

    [Header("Combat Parameters")]
    public float xDistance;
    public float yDistance;
    public float shootCD = 3;
    public float shootTimer;
    public bool playerAtLeft;
    public bool chasingPlayer;
    private bool shooting;
    private Transform playerTransform;
    private Vector3 desiredPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        objectPooler = ObjectPooler.Instance;
        animator = GetComponent<Animator>();
        speedAux = speed;
    }

    private void Update()
    {
        if (!shooting)
        {
            if (!chasingPlayer) // SI NO ESTA PERSIGUIENDO AL JUGADOR:
            { //PATRULLA
                next = moveSpots[spotsIndex];
                stopTime -= Time.deltaTime;

                if (stopTime >= 0)
                {
                    speed = 0;
                }
                else
                {
                    speed = speedAux;
                }
                //SE MUEVE HACIA EL SIGUIENTE SPOT
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(moveSpots[spotsIndex].transform.position.x, transform.position.y), speed * Time.deltaTime);
                //SE REVISA QUE TAN PROXIMO ESTA AL SIGUIENTE SPOT
                if (Vector2.Distance(transform.position, new Vector2(moveSpots[spotsIndex].position.x, transform.position.y)) < 0.3f)
                {
                    if (waitTime <= 0)
                    {
                        //SE MODIFICA EL SIGUIENTE SPOT Y SE GIRA EL SPRITE
                        if (moveSpots[spotsIndex] != moveSpots[moveSpots.Length - 1])
                        {
                            Vector3 theScale = transform.localScale;
                            theScale.x *= -1;
                            transform.localScale = theScale;
                            spotsIndex++;
                        }
                        else
                        {
                            Vector3 theScale = transform.localScale;
                            theScale.x *= -1;
                            transform.localScale = theScale;
                            spotsIndex = 0;
                        }
                        waitTime = startWaitTime;
                    }
                    else
                    {
                        waitTime -= Time.deltaTime;
                    }

                }
            }//(!chasing player)
            else
            {
                if (!shooting)
                {
                    shootTimer = (shootTimer > 0) ? (shootTimer - Time.deltaTime) : 0;
                    ChasePlayer();
                }
            }
        }//(!shooting)
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void MustChasePlayer(Transform playerTr)
    {
        playerTransform = playerTr;
        chasingPlayer = true;
    }
    void ChasePlayer()
    {
        playerAtLeft = (playerTransform.position.x < transform.position.x) ? true : false;
        float dirMultpiplier = playerAtLeft ? 1 : -1;

        desiredPos = playerTransform.position + new Vector3(xDistance*dirMultpiplier, yDistance);
        transform.position = Vector2.MoveTowards(transform.position, desiredPos,4*Time.deltaTime);

        //HAY ALGUNA FORMA DE NO EJECUTAR ESTO A CADA FRAME Y EJECUTARLO SOLO CUANDO EL VALOR CAMBIA????
        Vector3 theScale = transform.localScale;
        theScale.x = dirMultpiplier;
        transform.localScale = theScale;
     
        if (shootTimer<=0)
        {
            print("shoot");
            shootTimer = shootCD;
            StartShoot();
        }
    }
    public void StopWalking(float time) 
    {
        stopTime = time;
    }
    public void Shot()
    {
        objectPooler.SpawnFromPool("Bullet", bulletOrigin.position, Quaternion.Euler(playerTransform.position - bulletOrigin.position), (playerTransform.position));//- transform.position);
    }
    public void StartShoot()
    {
        animator.SetTrigger("Shot");
        shooting = true;
    }

    public void FinishShot()
    {
        shooting = false;
    }

}
