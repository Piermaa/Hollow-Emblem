using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicIA : MonoBehaviour
{
    float chasingSpeed=3;
    public Transform[] moveSpots;
    int spotsIndex;
    public float stopTime;
    public float speed = 6;
    float speedAux;
    public float waitTime;
    float startWaitTime = 1;
    bool playerAtLeft;
    [SerializeField] Transform next;

    Transform playerTransform;
    bool chasingPlayer;

    private void Start()
    {
        speedAux = speed;
    }

    private void Update()
    {
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

        if (!chasingPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(moveSpots[spotsIndex].transform.position.x, transform.position.y), speed * Time.deltaTime);

           

            if (Vector2.Distance(transform.position, new Vector2(moveSpots[spotsIndex].position.x, transform.position.y)) < 0.3f)
            {
                if (waitTime <= 0)
                {
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
        }
        else
        {

            speed = chasingSpeed;
            ChasePlayer();
        }
    }

    public void MustChasePlayer(Transform playerTr)
    {
        playerTransform = playerTr;
        chasingPlayer = true;
    }
    void ChasePlayer()
    {
        Vector2 dir = new Vector2(playerTransform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, dir, speed*Time.deltaTime);
        

        playerAtLeft = (playerTransform.position.x < transform.position.x);
        float dirMultpiplier = playerAtLeft ? -1 : 1;

        //HAY ALGUNA FORMA DE NO EJECUTAR ESTO A CADA FRAME Y EJECUTARLO SOLO CUANDO EL VALOR CAMBIA????
        Vector3 theScale = transform.localScale;
        theScale.x = dirMultpiplier;
        transform.localScale = theScale;

        if (Vector2.Distance(transform.position, playerTransform.position) < 2)
        {
            waitTime = startWaitTime;
            StopWalking(0.2f);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }


        if (theScale.x < 0)
        {
            theScale.x = -1;
        }
        else
        {
            theScale.x=1;
        }

        theScale = new Vector3(theScale.x,1,1);
        transform.localScale = theScale;

    }
    public void StopWalking(float time) 
    {
        stopTime = time;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyStop") &&chasingPlayer)
        {
            chasingPlayer = false;
            playerAtLeft = false;
        }
    }
}
