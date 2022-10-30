using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ColorPallete;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        //�������� ���¿� ���� ���� ����
        [SerializeField] private List<InventoryItem> inventoryItems;
        [field: SerializeField] public int Size { get; private set; } = 36;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;     //�κ��丮 ������Ʈ�� ��ųʸ� ����

        //�κ��丮 ����
        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            if (item.InStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while (quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }
            quantity = AddStackableItem(item, quantity);
            InformAboutChange();                    //�κ��丮 ����
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity,
                //�������� ��ȭ�� ���ٸ�, �Ϲ����� ���� ����������, �׷��� �ʴٸ�, ��ȭ�� ���� �����Ѵ�.
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
        {
            //�ϳ��� isEmpty��� false, �ƴϸ� true
            for (int i = 0; i < inventoryItems.Count - 10; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    return false;
            }
            return true;
        }
        /*
        private bool IsInventoryFull()
            => inventoryItems.Where(item => item.IsEmpty).Any() == false;
         */

        private int AddStackableItem(ItemSO item, int quantity) //item�� ������ ������, quantity�� ������ ����
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)              //�κ��丮�� ��ĭ�� ã�´�. 
                    continue;
                if (inventoryItems[i].item.ID == item.ID)   //��ĭ�� �����۰� �����Ϸ��� �������� ���ٸ�, 
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleToTake;
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

        public void RemoveItem(int itemIndex, int amout)
        {
            if (inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].IsEmpty)
                    return;
                int reminder = inventoryItems[itemIndex].quantity - amout;
                if (reminder <= 0)
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                else
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder);

                InformAboutChange();
            }
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            //�κ��丮�� �� �ִٸ�, �ش� �����۰� ��ȣ�� ��ųʸ��� �����Ͽ� �����Ѵ�. �� �κ��丮 ������ġ��.
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            //1�� �巡���� ��, 2�� ����� ��
            InventoryItem item1;
            item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();
            if (inventoryItems[itemIndex_1].item.ID == inventoryItems[itemIndex_2].item.ID)
            {
                if (inventoryItems[itemIndex_1].quantity + inventoryItems[itemIndex_2].quantity <= inventoryItems[itemIndex_1].item.MaxStackSize)
                {
                    //���� ���� �ִ� �������� ���ٸ�
                    //1�� �����, 2�� �� ����ŭ ä���.
                    inventoryItems[itemIndex_2] = inventoryItems[itemIndex_2].ChangeQuantity(inventoryItems[itemIndex_1].quantity + inventoryItems[itemIndex_2].quantity);
                    inventoryItems[itemIndex_1] = InventoryItem.GetEmptyItem();
                    InformAboutChange();
                }
                else
                {
                    //�������� �ƽ��� �ƴ϶��, �������� �ƽ��� ���ϰ� 1�� �������� �ִ´�.
                    int amount = (inventoryItems[itemIndex_2].quantity + inventoryItems[itemIndex_1].quantity) - inventoryItems[itemIndex_2].item.MaxStackSize;
                    inventoryItems[itemIndex_1] = inventoryItems[itemIndex_1].ChangeQuantity(amount);
                    inventoryItems[itemIndex_2] = inventoryItems[itemIndex_2].ChangeQuantity(inventoryItems[itemIndex_1].item.MaxStackSize);
                    InformAboutChange();
                }
            }
        }

        private void InformAboutChange()
        {
            //�������� ���ҵǾ��ٸ�, �κ��丮�� ������ �ִ� ���̱⿡, �κ��丮 ��ųʸ��� �����Ѵ�.
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }

    [Serializable]
    //��ü�迭���� ����ü�迭�� ����
    public struct InventoryItem
    {
        public int quantity;
        public ItemQuality quality;
        public ItemSO item;
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;
        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity,
                itemState = new List<ItemParameter>(this.itemState)
            };
        }

        public static InventoryItem GetEmptyItem()
            => new InventoryItem
            {
                item = null,
                quantity = 0,
                itemState = new List<ItemParameter>()
            };
    }
}