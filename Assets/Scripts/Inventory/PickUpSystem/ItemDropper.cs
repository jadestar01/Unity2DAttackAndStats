using Inventory.Model;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemDropper : MonoBehaviour
{
    public enum DB
    {
        EquipItemDB,
        ConsumeItemDB,
        UpgradeItemDB
    };

    [SerializeField] DBLoader dbLoader;
    [SerializeField] InventorySO inventorySO;
    [SerializeField] GameObject dropItem;
    [SerializeField] GameObject Player;
    public DB dataBase;
    public int code;
    public int quantity = 1;

    [Button]
    void ItemDrop()
    {
        if (dataBase == DB.EquipItemDB)
        {
            if (dbLoader.EquipItemDB[code] == null)
                return;
        }
        else if (dataBase == DB.ConsumeItemDB)
        {
            if (dbLoader.ConsumeItemDB[code] == null)
                return;
        }
        else if (dataBase == DB.ConsumeItemDB)
        {
            if (dbLoader.UpgradeItemDB[code] == null)
                return;
        }

        GameObject droppedItem = Instantiate(dropItem, Player.transform.position, Quaternion.identity);
        if (dataBase == DB.EquipItemDB)
        {
             ItemSO item = dbLoader.EquipItemDB[code];
             droppedItem.GetComponent<Item>().InventoryItem = item;
        }
        else if (dataBase == DB.ConsumeItemDB)
        {
            ItemSO item = dbLoader.ConsumeItemDB[code];
            droppedItem.GetComponent<Item>().InventoryItem = item;
        }
        else if (dataBase == DB.ConsumeItemDB)
        {
            ItemSO item = dbLoader.UpgradeItemDB[code];
            droppedItem.GetComponent<Item>().InventoryItem = item;
        }
        droppedItem.GetComponent<Item>().Quantity = quantity;
    }

    [Button]
    void AddItem()
    {
        ItemSO item;
        if (dataBase == DB.EquipItemDB)
        {
            item = dbLoader.EquipItemDB[code];
            inventorySO.AddItem(item, quantity);
        }
        else if (dataBase == DB.ConsumeItemDB)
        {
            item = dbLoader.ConsumeItemDB[code];
            inventorySO.AddItem(item, quantity);
        }
        else if (dataBase == DB.ConsumeItemDB)
        {
            item = dbLoader.UpgradeItemDB[code];
            inventorySO.AddItem(item, quantity);
        }
    }
}
