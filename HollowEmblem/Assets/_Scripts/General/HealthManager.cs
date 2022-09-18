using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] HealthController playerHealth;

    IEnumerator BecameInvinsible()
    {
        playerHealth.inmune = true;
        yield return new WaitForSecondsRealtime(0.9f);
        playerHealth.inmune = false;
        yield break;
    }

    public void SetInmunity()
    {
        StartCoroutine(BecameInvinsible());
    }
}
