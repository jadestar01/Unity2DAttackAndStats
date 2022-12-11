using Inventory.Model;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] DBLoader dbLoader;
    [SerializeField] InventorySO inventorySO;
    [SerializeField] GameObject dropItem;
    [SerializeField] GameObject Player;
    public int code;
    public int quantity = 1;

    [Button]
    void ItemDrop()
    {
        //droppedItem.GetComponent<Item>().InventoryItem = item;
        if (dbLoader.EquipItemDB.ContainsKey(code))
        {
            GameObject droppedItem = Instantiate(dropItem, Player.transform.position, Quaternion.identity);
            droppedItem.GetComponent<Item>().InventoryItem = dbLoader.EquipItemDB[code];
            droppedItem.GetComponent<Item>().Quantity = quantity;
        }
        else if (dbLoader.ConsumeItemDB.ContainsKey(code))
        {
            GameObject droppedItem = Instantiate(dropItem, Player.transform.position, Quaternion.identity);
            droppedItem.GetComponent<Item>().InventoryItem = dbLoader.ConsumeItemDB[code];
            droppedItem.GetComponent<Item>().Quantity = quantity;
        }
        else if (dbLoader.UpgradeItemDB.ContainsKey(code))
        {
            GameObject droppedItem = Instantiate(dropItem, Player.transform.position, Quaternion.identity);
            droppedItem.GetComponent<Item>().InventoryItem = dbLoader.UpgradeItemDB[code];
            droppedItem.GetComponent<Item>().Quantity = quantity;
        }
        else
        {
            Debug.Log("Can not Drop Item! There is no item (Code:" + code + ")");
        }
    }

    [Button]
    void AddItem()
    {
        if (dbLoader.EquipItemDB.ContainsKey(code))
        {
            inventorySO.AddItem(dbLoader.EquipItemDB[code], quantity);
        }
        else if (dbLoader.ConsumeItemDB.ContainsKey(code))
        {
            inventorySO.AddItem(dbLoader.ConsumeItemDB[code], quantity);
        }
        else if (dbLoader.UpgradeItemDB.ContainsKey(code))
        {
            inventorySO.AddItem(dbLoader.UpgradeItemDB[code], quantity);
        }
        else
        {
            Debug.Log("Can not Add Item! There is no item (Code:" + code + ")");
        }
    }
}
