using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FBBattleState { SPAWNING, IDLE, MELEEATTACK, FLOORATTACK, SHOOTATTACK};

public class FinalBossScript : MonoBehaviour
{
    public FBBattleState state;

    public bool canIdle;

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        state = FBBattleState.SPAWNING;
        canIdle = false;
    }

    void Update()
    {
        BossStateExecution();
    }

    IEnumerator SpawningAnimation()
    {
        Debug.Log("SPAWNING");

        canIdle = true;
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(1f);

        state = FBBattleState.IDLE;
        yield return null;
    }

    IEnumerator Vulnerable()
    {
        Debug.Log("IDLE");

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(2f);

        canIdle = false;
        state = FBBattleState.MELEEATTACK;
        yield return null;
    }

    void MeleeAttackActivator()
    {
        Debug.Log("MELEEATTACK");

        canIdle = true;
        animator.SetTrigger("MeleeAttack");
    }

    void BossStateExecution()
    {
        switch (state)
        {
            case FBBattleState.SPAWNING:
                StartCoroutine(SpawningAnimation());
                break;

            case FBBattleState.IDLE:
                StartCoroutine(Vulnerable());
                break;

            case FBBattleState.MELEEATTACK:
                MeleeAttackActivator();
                break;

            case FBBattleState.FLOORATTACK:
                break;

            case FBBattleState.SHOOTATTACK:
                break;
        }
    }

    public void PutIdle()
    {
        state = FBBattleState.IDLE;
    }
}
