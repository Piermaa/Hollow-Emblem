using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StaminaController))]
//using Lean.Pool;
public class EnemyShoot : MonoBehaviour
{
    StaminaController staminaController;
    ObjectPooler objectPooler;
    public string ammunition;
    public float fireRate=10;
    float fireCooldown;

    public Transform spawnPos;

    private void Start()
    {
        staminaController = GetComponent<StaminaController>();
        objectPooler = ObjectPooler.Instance;
        fireCooldown = fireRate;
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if(fireCooldown<0 && staminaController.CheckStamina())
        {
            fireCooldown = fireRate;
            objectPooler.SpawnFromPool(ammunition, spawnPos.position, spawnPos.rotation);
            print("spawning bullet");
        }

    }
}
