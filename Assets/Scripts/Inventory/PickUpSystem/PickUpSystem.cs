using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] private InventorySO inventoyData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Item�� ItemSO�� �ٸ���.
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            int reminder = inventoyData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0)
                item.DestroyItem();
            else
                item.Quantity = reminder;
        }
    }
}