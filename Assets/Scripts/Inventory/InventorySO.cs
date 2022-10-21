using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inv.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItems> inventoryItems;
        [field: SerializeField] public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItems>> OnInventoryUpdated;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItems>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItems.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            if (item.IsStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    if(IsInventoryFull())
                        return quantity;
                    while (quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }
            quantity = AddStackableItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            InventoryItems newItem = new InventoryItems
            {
                item = item,
                quantity = quantity,
                itemState = new List<ItemParameter>(itemState == null ? item.DefaultParametersList : itemState)
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull()
            => inventoryItems.Where(item => item.IsEmpty).Any() == false;

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                if (inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleTotake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;
                    if (quantity > amountPossibleTotake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleTotake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            while (quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
            return quantity;
        }

        public void AddItem(InventoryItems item)
        {
            AddItem(item.item, item.quantity);
        }

        public Dictionary<int, InventoryItems> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItems> returnValue = new Dictionary<int, InventoryItems>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItems GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        internal void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItems item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            if (inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].IsEmpty)
                    return;
                int reminder = inventoryItems[itemIndex].quantity - amount;
                if (reminder == 0)
                    inventoryItems[itemIndex] = InventoryItems.GetEmptyItem();
                else
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder);
                InformAboutChange();
            }
        }
    }

    [Serializable]
    public struct InventoryItems
    {
        public int quantity;
        public ItemSO item;
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;

        public InventoryItems ChangeQuantity(int newQuantity)
        {
            return new InventoryItems
            {
                item = this.item,
                quantity = newQuantity,
                itemState = new List<ItemParameter>(this.itemState)
            };
        }

        public static InventoryItems GetEmptyItem()
            => new InventoryItems
            {
                item = null,
                quantity = 0,
                itemState = new List<ItemParameter>()
            };
    }
}