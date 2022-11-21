using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInput : MonoBehaviour
{
    public bool isShowingMap;
    public bool isShowingIventory;

    public GameObject map;
    public GameObject inventory;

    public Animator animator;
    public Rigidbody2D rb;

    void Update()
    {
        mapInput();
    }

    void mapInput ()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isShowingMap = !isShowingMap;
        }

        if (isShowingMap)
        {
            animator.SetBool("ShowMap", true);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            animator.SetBool("ShowMap", false);
        }
    }
    void mapAppear()
    {
        map.SetActive(true);
    }

    void mapDisappear()
    {
        map.SetActive(false);
    }

    void inventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isShowingIventory = !isShowingIventory;
        }

        if (isShowingIventory)
        {
            animator.SetBool("ShowInventory", true);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            animator.SetBool("ShowInventory", false);
        }
    }
    void InventoryAppear()
    {
        inventory.SetActive(true);
    }

    void InventoryDisappear()
    {
        inventory.SetActive(false);
    }

}
