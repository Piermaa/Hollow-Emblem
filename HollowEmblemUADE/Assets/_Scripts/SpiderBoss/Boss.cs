using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { SEARCHING, CHARGING, EMBISTING, RECOVERING }

public class Boss : MonoBehaviour
{
    BattleState state;

    RaycastHit2D playerRc;
    RaycastHit2D wallRc;

    public Transform seekPlayerStart;

    private float distanceOfRay = 25;
    public float distanceOfWallRay = 0.5f;

    public LayerMask playerLayer;
    public LayerMask spikeLayer;

    private void Start()
    {
        gameObject.SetActive(true);
        state = BattleState.SEARCHING;
    }
    private void Update()
    {
        BossStateExecution();
    }

    //IEnumerator SearchingPlayer()
    //{
    //    yield return null;
    //    playerRc = Physics2D.Raycast(shootStart.position, shootStart.TransformDirection(Vector2.left), 20, playerLayer);
    //    yield return new WaitForSeconds(2f);
    //}

    void Searching()
    {
        playerRc = Physics2D.Raycast(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left), distanceOfRay, playerLayer);
        Debug.DrawRay(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left) + new Vector3 (distanceOfRay, 0,0), Color.blue);
    }

    void Charging()
    {

    }

    void Embisting()
    {
        wallRc = Physics2D.Raycast(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left), distanceOfWallRay, spikeLayer);
        Debug.DrawRay(seekPlayerStart.position, seekPlayerStart.TransformDirection(Vector2.left) + new Vector3(distanceOfWallRay, 0, 0), Color.red);
    }

    void Recovering()
    {

    }

    void BossStateExecution()
    {
        switch (state)
        {
            case BattleState.SEARCHING:
                Searching();
                break;

            case BattleState.CHARGING:
                Charging();
                break;

            case BattleState.EMBISTING:
                Embisting();
                break;

            case BattleState.RECOVERING:
                Recovering();
                break;
        }
    }

}
