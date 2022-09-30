using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float timeToDespawn;
    public float timeLimit = 5;


   
    private void Update()
    {
        transform.Translate(new Vector2(1,0) * Time.deltaTime * speed);

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
            if (collision.gameObject.GetComponent<Target>() != null)
            {
                Target target = collision.gameObject.GetComponent<Target>();

                target.TakeDamage(1);
            }
        }
        this.gameObject.SetActive(false);
    }
}
