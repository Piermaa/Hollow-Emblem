using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CREADO PARA QUE SE LE PUEDA INDICAR TIPO Y CANTIDAD AL MOMENTO DE SER GENERADO
[RequireComponent(typeof(SpriteRenderer))]
public class PickupableItem : MonoBehaviour
{
    public int amount=1;
    [Tooltip("Ammo, Heal, etc, revisar nombre de ItemManager")]
    public string itemName;
    Item itemToAdd;
    ItemManager itemManager;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        itemManager = ItemManager.Instance;
        itemManager.itemDictionary.TryGetValue(itemName, out itemToAdd);
        //Debug.Log(itemToAdd.name);
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemToAdd.sprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            var inventory = PlayerInventory.Instance;
            inventory.AddItem(itemToAdd, amount);
            Destroy(this.gameObject);
        }
    }
}
