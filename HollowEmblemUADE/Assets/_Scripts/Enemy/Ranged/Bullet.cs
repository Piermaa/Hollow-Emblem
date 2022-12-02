using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IPooledObject
{
    Rigidbody2D rb;
    public float speed;
    public float timeToDespawn;
    public float timeLimit = 5;

    public Vector3 moveDirection;
    public void OnObjectSpawn()
    {
        moveDirection += Vector3.up;
        moveDirection -= transform.position;
        moveDirection.Normalize();
        rb.AddForce(moveDirection.normalized * speed, ForceMode2D.Force);
        if (rb.velocity.x < 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x = 1;
            this.transform.localScale = theScale;
        }
        else
        {
            Vector3 theScale = transform.localScale;
            theScale.x = -1;
            this.transform.localScale = theScale;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        timeToDespawn += Time.deltaTime;

        if (timeToDespawn>timeLimit)
        {
            //var pool =  FindObjectOfType<ObjectPooler>();
           this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<HealthController>(out var health))
            {
                health.TakeDamage(1);
            }
        }
        this.gameObject.SetActive(false);
    }


    //-----------------------Feliz Cumple Dante-----------------------|
    //                          $10000                                |
    //                                                                |
    //                                                                |
    //                                                                |
    //                     navidad                                    |
    //                                                                |
    //                                                                |
    //                                                                |
    //----------------------------------------------------------------|
}
