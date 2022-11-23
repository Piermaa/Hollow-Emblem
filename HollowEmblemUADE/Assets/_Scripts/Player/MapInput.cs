using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShowStates { SHOWINGINVENTORY, SHOWINGMAP, HIDING}

public class MapInput : MonoBehaviour
{
    public GameObject map;
    public GameObject inventory;
    GameObject auxiliar;

    public Animator animator;
    public Rigidbody2D rb;

    public ShowStates state;

    public bool canInput;

    private void Start()
    {
        canInput = true;
        state = ShowStates.HIDING;
    }

    void Update()
    {
        AppearSomething();
    }

    void AppearSomething()
    {   
        if (state == ShowStates.HIDING)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && canInput)
            {
                if (auxiliar != map)
                {
                    state = ShowStates.SHOWINGMAP;
                    animator.SetBool("ShowMap", true);
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (auxiliar != inventory && canInput)
                {
                    state = ShowStates.SHOWINGINVENTORY;
                    animator.SetBool("ShowMap", true);
                }
            }
        }

        else
        {
            rb.velocity = Vector2.zero;
            
            if (Input.GetKeyDown(KeyCode.Tab) && canInput)
            {
                if (auxiliar != map)
                {
                    HideAuxiliar();
                    state = ShowStates.SHOWINGMAP;
                    Show();
                }

                else
                {
                    HideAuxiliar();
                    animator.SetBool("ShowMap", false);
                    animator.SetTrigger("Disappear");
                    auxiliar = null;
                    state = ShowStates.HIDING;
                }
            }

            if (Input.GetKeyDown(KeyCode.I) && canInput)
            {
                if (auxiliar != inventory)
                {
                    HideAuxiliar();
                    state = ShowStates.SHOWINGINVENTORY;
                    Show();
                }

                else
                {
                    HideAuxiliar();
                    animator.SetBool("ShowMap", false);
                    animator.SetTrigger("Disappear");
                    auxiliar = null;
                    state = ShowStates.HIDING;
                }
            }
        }
    }

    void Show()
    {
        switch (state)
        {
            case ShowStates.SHOWINGMAP:
                map.SetActive(true);
                auxiliar = map;
                break;

            case ShowStates.SHOWINGINVENTORY:
                inventory.SetActive(true);
                auxiliar = inventory;
                break;
        }
    }

    public void CanInput()
    {
        canInput = true;
    }

    public void CanNotInput()
    {
        canInput = false;
    }

    void HideAuxiliar()
    {
        auxiliar.SetActive(false);
    }
}