using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] BasicIA basicIA;
    [SerializeField]AirIA airIA;
    private void Awake()
    {
   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            if (basicIA != null)
            {
                basicIA.MustChasePlayer(collision.gameObject.transform);
            }
            else 
            {
                airIA.MustChasePlayer(collision.gameObject.transform);
            }
          
            print("playerinrange");
        }
    }
}
