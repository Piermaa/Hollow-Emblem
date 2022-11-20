using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PopUp : MonoBehaviour
{
    public Slot slot;
    public Button discardButton;
    public Button useButton;
    public int xIndex;
    public int yIndex;
    public PlayerInventory inventory;
    void Start()
    {
        inventory = PlayerInventory.Instance;
    }

    void Update()
    {
        
    }

    public void ActivatePopUp()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
    public void Discard()
    {
        inventory.EmptySlot(slot);
        Destroy(this.gameObject);
    }

    public void UseItem()
    {
        slot.itemEvent.Invoke();
        slot.amount -= slot.item.usedPerEvent;
    }
}
