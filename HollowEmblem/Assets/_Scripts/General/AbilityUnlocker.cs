using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlocker : MonoBehaviour
{
    [Tooltip("Dash (includes dj), Slam, Slime")]
    public string unlockedAb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (unlockedAb)
            {
                case "Slime":
                    StateManager stateManager;
                    collision.gameObject.TryGetComponent<StateManager> (out stateManager);
                    stateManager.enabled = enabled;
                    break;
                case "Dash":
                    PlayerMovement playerMovement;
                    collision.gameObject.TryGetComponent<PlayerMovement>(out playerMovement);
                    playerMovement.dashUnlocked = true;
                    break;
                case "Slam":
                    PlayerAbilities playerAbilities;
                    collision.gameObject.TryGetComponent<PlayerAbilities>(out playerAbilities);
                    playerAbilities.slamUnlocked = true;
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
