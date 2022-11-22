using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirIA : MonoBehaviour
{
    public Transform bulletOrigin;
    public float shootCD=3;
    public float shootTimer;
    Animator animator;
    public float xDistance;
    public float yDistance;

    public bool playerAtLeft;
    public Transform[] moveSpots;
    int spotsIndex;
    public float stopTime;
    public float speed = 6;
    float speedAux;
    public float waitTime;
    float startWaitTime = 1;
    bool shooting;
    ObjectPooler objectPooler;
    [SerializeField] Transform next;
    Vector3 desiredPos;
    Transform playerTransform;
    public bool chasingPlayer;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        animator = GetComponent<Animator>();
        //playerTransform = PlayerInventory.Instance.gameObject.transform;
        speedAux = speed;
    }

    private void Update()
    {

        if (!chasingPlayer)
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

            if (!shooting)
            {
                shootTimer = (shootTimer > 0) ? (shootTimer - Time.deltaTime) : 0;
                ChasePlayer();
            }
 
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
        //Debug.DrawRay(transform.position, playerTransform.position- desiredPos);
        transform.position = Vector2.MoveTowards(transform.position, desiredPos,4*Time.deltaTime);

  
        if (shootTimer<=0)
        {
            
            print("shoot");
            shootTimer = shootCD;
            Shot();
            //StartCoroutine(Shooting());
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

    IEnumerator Shooting()
    {
        shooting = true;
        //animator.SetTrigger("Shoot");
        Shot();
        yield return new WaitForSeconds(1);
    }
}
