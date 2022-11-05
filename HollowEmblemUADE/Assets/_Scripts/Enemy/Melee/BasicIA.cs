using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicIA : MonoBehaviour
{

    public Transform[] moveSpots;
    int spotsIndex;
    public float stopTime;
    public float speed = 6;
    float speedAux;
    public float waitTime;
    float startWaitTime = 1;

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
        Vector3 theScale = transform.position - playerTransform.position;



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
}
