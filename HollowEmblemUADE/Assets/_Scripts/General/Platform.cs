using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Collider2D boxCollider;
    bool playerCollides;
    public float distanceToPlayerTolerated;
    private void Start()
    {
        boxCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(playerCollides)
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    boxCollider.enabled = false;
                }
            }
        }
    }

    IEnumerator ReestablishCollider()
    {
        yield return new WaitForSeconds(1f);
        playerCollides = false;
        boxCollider.enabled = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&& !playerCollides)
        {
            print("PlayerOverPlat");
            playerCollides = true;
            float distance = (Vector2.Distance(new Vector3(transform.position.x, transform.position.y), new Vector3(transform.position.x, collision.transform.position.y, 0)));
            Debug.Log(distance);

            if (0.5f > distance)
            {
                boxCollider.enabled = false;
                StartCoroutine(ReestablishCollider());
            }
            else if (distance < 1.87f)
            {
                collision.gameObject.transform.position += new Vector3(0, 1.88f - distance);
            }
           
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ReestablishCollider());
            
        }
    }
}
