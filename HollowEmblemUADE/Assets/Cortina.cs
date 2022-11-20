using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DisplayState { MAP,INVENTORY,HIDE};
public class Cortina : MonoBehaviour
{
    bool isShowing;
    public GameObject mapPanel;
    public GameObject inventoryPanel;

    GameObject panelAux;


    Animator animator;
    public DisplayState displayState;
    void Update()
    {
        if (!isShowing)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                displayState = DisplayState.INVENTORY;
                animator.SetTrigger("ShowPanel");
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                displayState = DisplayState.MAP;
                animator.SetTrigger("ShowPanel");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (panelAux != inventoryPanel)
                {
                    HidePanel();
                    displayState = DisplayState.INVENTORY;
                    DisplayPanel();
                }
                else
                {
                    HidePanel();
                    animator.SetTrigger("HidePanel");
                }

            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (panelAux != mapPanel)
                {
                    HidePanel();
                    displayState = DisplayState.MAP;
                    DisplayPanel();
                }
                else
                {
                    HidePanel();
                    animator.SetTrigger("HidePanel");
                }

            }
        }

    }

    //Llamado al final de la animacion
    public void DisplayPanel()
    {
        switch (displayState)
        {
            case DisplayState.INVENTORY:
                inventoryPanel.SetActive(true);
                panelAux = inventoryPanel;
                break;

            case DisplayState.MAP:
                mapPanel.SetActive(true);
                panelAux = mapPanel;
                break;
        }
    }

    public void HidePanel()
    {
        panelAux.SetActive(false);
    }
}
