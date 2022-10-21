using Inv.Model;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private InventorySO inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null)
        {
            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0)
                item.DestroyItem();
            else
                item.Quantity = reminder;
        }
    }
}
