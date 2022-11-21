using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class Slot
{
    public int positionX, positionY, amount;
    public Item item;
    public Image image;
    public GameObject slotGameObject;
    public TextMeshProUGUI tMPro;
    public UnityEvent itemEvent;
    public Button button;
    public PopUp slotPopUp;
   
    public Slot(Item i, int pX, int pY, int a, Image im, GameObject go,TextMeshProUGUI t,Button b)
    {
        this.item = i;
        this.positionX = pX;
        this.positionY = pY;
        this.amount = a;
        this.image = im;
        this.button = b;
        this.slotGameObject=go;
        this.tMPro = t; 
    }
}

public class PlayerInventory : MonoBehaviour
{
    private PlayerCombat combat;
    Sprite sprite;
    public int rowSize = 4;
    public int colSize = 2;
    public Slot[,] slots;

    public static PlayerInventory Instance;
    ItemManager itemManager;
    public GameObject[] rows;
    public int auxAmount;

    private void Awake()
    {
        itemManager = ItemManager.Instance;
        Instance = this;
    }
    void Start()
    {
        combat = GetComponent<PlayerCombat>();
        slots = new Slot[rowSize, colSize];

        // Y ES EL VERTICAL, MAXIMO 3
        // X HORIZONTAL, MAXIMO 6
        // ARRAY[X,Y]
        for (int y = 0; y < slots.GetLength(1); y++)
        {
            var butRow = rows[y].GetComponentsInChildren<Button>();

            for (int x = 0; x < slots.GetLength(0); x++)
            {
                slots[x, y] = new Slot(null, x, y, 0, null, butRow[x].gameObject, null, butRow[x]);
                slots[x, y].tMPro = slots[x, y].slotGameObject.GetComponentInChildren<TextMeshProUGUI>();
                slots[x, y].tMPro.text = " ";
                var buttonImage = slots[x, y].button.gameObject.GetComponent<Image>();
      
                var images =slots[x, y].button.gameObject.GetComponentsInChildren<Image>();

                slots[x, y].image = images[1];
                slots[x, y].image.enabled = false;
            }
        }
    }

    private void Update()
    {

    }
    /// <summary>
    /// Se a�ade cierta cantidad de items al inventario, para ello buscar� un lugar disponible en el inventario
    /// </summary>
    /// <param name="itemToAdd">Tipo de item a agregar</param>
    /// <param name="amountToAdd">Cantidad a agregar</param>
    public void AddItem(Item itemToAdd, int amountToAdd)
    {
        Slot actualSlot = SearchSlot(itemToAdd, amountToAdd);

        if (actualSlot != null && auxAmount != 0)
        {
            actualSlot.itemEvent = itemToAdd.itemEvent;
            actualSlot.item = itemToAdd;
            actualSlot.amount += auxAmount;
            actualSlot.image.enabled = true;
            actualSlot.image.sprite = itemToAdd.sprite;
            actualSlot.tMPro.text = actualSlot.amount.ToString();

            var popUpReference = actualSlot.button.gameObject.GetComponentsInChildren<PopUp>();
            Debug.Log("POPUPS: " + popUpReference.Length);
            if (actualSlot.slotPopUp == null)
            {
                CreatePopUp(actualSlot);
            }
        }
    }

    /// <summary>
    /// Variante por si ya se tiene el slot que se quiere afectar
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <param name="actualSlot"></param>
    public void AddItem(Item itemToAdd, int amountToAdd, Slot actualSlot)
    {
        actualSlot.item = itemToAdd;
        actualSlot.amount += auxAmount;
        if (actualSlot.amount >= itemToAdd.maxStackeable)
        {
            actualSlot.amount = itemToAdd.maxStackeable;
        }
        actualSlot.image.enabled = true;
        actualSlot.image.sprite = itemToAdd.sprite;
        actualSlot.tMPro.text = actualSlot.amount.ToString();


        var popUpReference = actualSlot.button.gameObject.GetComponentsInChildren<PopUp>();
        Debug.Log("POPUPS: " +popUpReference.Length);
        if (actualSlot.slotPopUp == null)
        {
            CreatePopUp(actualSlot);
        }
    }
    /// <summary>
    /// Recorre el array de Slots hasta encontrar un lugar donde colocar el item que se quiere a�adir
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <returns></returns>
    public Slot SearchSlot(Item itemToAdd, int amountToAdd)
    {
        int restantAmount = amountToAdd;
        auxAmount = restantAmount; //AUX AMOUNT ES UTILIZADO COMO CANTIDAD DE OBJETOS A AGREGAR EN CASO DE QUE EL MONTO A AGREGAR SEA MODIFICADO MIENTRAS SE BUSCA UN SLOT QUE PUEDAN OCUPAR LOS ITEMS
        print("auxAmount: " + auxAmount);

        for (int i = 0; i < 3; i++) // REPITE TRES VECES, MEDIDA DE SEGURIDAD EN CASO DE QUE NO HAYA LUGAR DONDE METER LOS ITEMS, MEJORA DE UN WHILE
        {
            if (restantAmount > 0) // SI TODAVIA QUEDA MONTO QUE AGREGAR:
            {
                for (int y = 0; y < slots.GetLength(1); y++) // LOOPEA EN LAS COLUMNAS
                {
                    for (int x = 0; x < slots.GetLength(0); x++)
                    {
                        Debug.Log(auxAmount);
                        if (slots[x, y].item == null) // SI EL SLOT EST� VACIO
                        {
                            if (restantAmount < itemToAdd.maxStackeable)
                            {
                                return slots[x, y]; //SI EL SLOT EST� VAC�O Y EL MONTO A AGREGAR ES MENOR AL MAXIMO STACKEABLE POR SLOT
                            }
                            else // SI EL SLOT EST� VAC�O, PERO SE EST� POR AGREGAR UNA CANTIDAD MAYOR A LA MAXIMA ACUMULABLE POR ESE ITEM
                            {
                                restantAmount -= itemToAdd.maxStackeable;
                                auxAmount = restantAmount;
                                FillSlot(slots[x, y], itemToAdd);
                            }
                        }
                        if (slots[x, y].item == itemToAdd) //SI ES EL MISMO TIPO DE ITEM EL DEL SLOT
                        {
                            if (!(slots[x, y].item.maxStackeable == slots[x, y].amount)) //SI EL SLOT NO EST� LLENO:
                            {
                                int available = slots[x, y].item.maxStackeable - slots[x, y].amount;

                                if (restantAmount < available) // SI EL MONTO A AGREGAR NO HACE SUPERAR EL M�XIMO STACKEABLE, CREO QUE PUEDE NO SER NECESARIO !!
                                {
                                    return slots[x, y];  //Devuelve este slot, porque se puede agregar aunque ocupando 
                                }
                                else // SI NO HAY TANTOS ESPACIOS DISPONIBLES COMO OBJETOS A AGREGAR
                                {
                                    restantAmount -= available;
                                    auxAmount = restantAmount;
                                    FillSlot(slots[x, y], itemToAdd);
                                }
                            }
                        }

                    }// X foR

                }// Y for

            }// if that check remaining items to add

        }//firewall for
        return null;
    }

    public void EmptySlot(Slot slot)
    {
        slot.amount = 0;
        slot.itemEvent = null;
        slot.tMPro.text = "";
        slot.item = null;
        slot.image.sprite = null;
        slot.image.enabled = false;
        var popUpReference = slot.button.gameObject.GetComponentsInChildren<PopUp>();

        foreach (PopUp popUps in popUpReference)
        {
            Destroy(popUps.gameObject);
        }
    }

    /// <summary>
    /// Llena un slot dado por un item dado
    /// </summary>
    /// <param name="slot">Slot a llenar</param>
    /// <param name="itemType">Tipo de item que se colocar� en el slot</param>
    public void FillSlot(Slot slot, Item itemType)
    {
        slot.itemEvent = itemType.itemEvent;
        slot.item = itemType;
        slot.amount = itemType.maxStackeable;
        slot.tMPro.text = slot.amount.ToString();
        slot.image.enabled = true;
        slot.image.sprite = itemType.sprite;
        var popUpReference = slot.button.gameObject.GetComponentsInChildren<PopUp>();
        Debug.Log("POPUPS: " + popUpReference.Length);
        if (slot.slotPopUp==null)
        {
            CreatePopUp(slot);
        }
    }

    public void CreatePopUp(Slot slot)
    {
        slot.item.popUp = Instantiate(itemManager.popUpPrefabs, slot.slotGameObject.transform);
        slot.item.popUp.TryGetComponent<PopUp>(out var popUpRef);
        slot.item.popUp.gameObject.SetActive(false);
        popUpRef.slot = slot;
        slot.slotPopUp = popUpRef;
        //slot.itemEvent.
        slot.button.onClick.AddListener(popUpRef.ActivatePopUp);
   
        switch(slot.item.name)
        {
            case "Heal":
          
                slot.slotPopUp.useButton.onClick.AddListener(delegate { ConsumeItem(slot); });
                slot.slotPopUp.useButton.onClick.AddListener(itemManager.UseHeal);
                break;

            case "Ammo":
                //slot.slotPopUp.useButton.onClick += combat.Reload();
                slot.slotPopUp.useButton.onClick.AddListener(combat.Reload);
                break;

        }
    }

    public void ConsumeItem(Slot slot)
    {
        slot.amount -= slot.item.usedPerEvent;
        slot.tMPro.text = slot.amount.ToString();

        if (slot.amount <= 0)
        {
            EmptySlot(slot);
        }
    }

    public Slot SearchAmmo()
    {
        Slot lessAmmoSlot=null;// = slots[0,0];
        for (int y = 0; y < slots.GetLength(1); y++)
        {
            for (int x = 0; x < slots.GetLength(0); x++)
            {
                if (slots[x,y].item != null)
                {
                    if (slots[x, y].item.name == "Ammo")
                    {
                        if (lessAmmoSlot == null)
                        {
                            lessAmmoSlot = slots[x, y];
                        }
                        if (lessAmmoSlot.amount > slots[x, y].amount)
                        {
                            lessAmmoSlot = slots[x, y];
                        }
                    }

                }

            }
        }
        return lessAmmoSlot;
    }

    public void GetAmmoFromInventory()
    {

        int spaceAvailableOnClip=0; 
    
        Slot newSlot;
        //
        do
        {
            spaceAvailableOnClip = combat.maxAmmo - combat.currentAmmo;
            print(spaceAvailableOnClip);
            newSlot = SearchAmmo();
            if (newSlot != null)
            {
                if (spaceAvailableOnClip > newSlot.amount)
                {
                    combat.currentAmmo += newSlot.amount;
                    EmptySlot(newSlot);
                }
                else
                {
                    combat.currentAmmo += spaceAvailableOnClip;
                    newSlot.amount -= spaceAvailableOnClip;
                    newSlot.tMPro.text = newSlot.amount.ToString();
                }
            }

            if ((newSlot != null) && newSlot.amount <= 0)
            {
                EmptySlot(newSlot);
            }


        } while (spaceAvailableOnClip > 0 && newSlot != null);

       
    }
}//class





