using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    public float speed;
    public float timeToDespawn;
    public float timeLimit = 5;

    public Vector3 moveDirection;
    public void OnObjectSpawn()
    {
        moveDirection += Vector3.up;
        moveDirection -= transform.position;
        moveDirection.Normalize();
    }

   
    private void Update()
    {
        //Vector3 newDir= Vector3.
        //transform.position = Vector3.MoveTowards(transform.position,moveDirection,Time.deltaTime * speed);
        transform.Translate(moveDirection * Time.deltaTime * speed);

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
}
