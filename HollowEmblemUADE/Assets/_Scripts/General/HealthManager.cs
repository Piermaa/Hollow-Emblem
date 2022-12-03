using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] HealthController playerHealth;

    [Header("CommonToAllHealthControllers")]
    public Material takingDamageMaterial;

    private void Start()
    {
        var healths =FindObjectsOfType<HealthController>();

        foreach(HealthController h in healths)
        {
            h.takingDamageMaterial = takingDamageMaterial;
        }
    }
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
