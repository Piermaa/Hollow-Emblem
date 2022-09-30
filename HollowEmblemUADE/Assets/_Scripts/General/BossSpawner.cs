using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject[] boss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            collision.TryGetComponent<PlayerRespawn>(out var respawn);
            respawn.SetRespawn(transform.position);

            foreach (GameObject go in boss)
            {
                if (go.gameObject!=null)
                {
                    go.SetActive(true);
                }
               
            }
        }
    }

}
