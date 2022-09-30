using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] BasicIA basicIA;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            basicIA.MustChasePlayer(collision.gameObject.transform);
            print("playerinrange");
        }
    }
}
