using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ColorPallete;
using Mono.Cecil.Cil;
using static Inventory.Model.EquippableItemSO;
using UnityEditor.UIElements;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        //아이템의 상태와 개수 등을 저장
        [SerializeField] private List<InventoryItem> inventoryItems;
        [field: SerializeField] public int Size { get; private set; } = 36;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;     //인벤토리 업데이트시 딕셔너리 저장

        //인벤토리 생성
        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null, List<UpgradeResult> upgradeResults = null)
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
            InformAboutChange();                    //인벤토리 적용
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null, List<UpgradeResult> upgradeResults = null)
        {
            //item.DefaultParametersList, itemState, item.DefaultUpgradeResults, upgradeResults 확인
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity,
                //아이템이 변화가 없다면, 일반적인 값을 가져오지만, 그렇지 않다면, 변화된 값을 유지한다.
                itemState = new List<ItemParameter>(itemState == null ? item.DefaultParametersList : itemState),
                upgradeResults = new List<UpgradeResult>(upgradeResults == null ? item.DefaultUpgradeResults : upgradeResults)
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    MessageManager.Instance.ItemMessage(item, quantity);
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull()
        {
            //하나라도 isEmpty라면 false, 아니면 true
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

        private int AddStackableItem(ItemSO item, int quantity) //item은 습득한 아이템, quantity는 떨어진 개수
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)              //인벤토리에 빈칸을 찾는다. 
                    continue;
                if (inventoryItems[i].item.ID == item.ID)   //빈칸의 아이템과 습득하려는 아이템이 같다면, 
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake)        //초과로 들어왔을 때, quatity를 깍는다. 20, 10+10
                    {
                        MessageManager.Instance.ItemMessage(item, amountPossibleToTake);
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleToTake;
                    }
                    else                                        //초과가 아니라면, 그냥 표기한다.
                    {
                        MessageManager.Instance.ItemMessage(item, quantity);
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            //그럼에도 남은 게 있다면, 
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
            //인벤토리가 차 있다면, 해당 아이템과 번호를 딕셔너리에 저장하여 전달한다. 즉 인벤토리 저장장치다.
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
            InventoryItem item1;
            item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();
            if (!inventoryItems[itemIndex_1].IsEmpty)
            {
                if (inventoryItems[itemIndex_1].item.ID == inventoryItems[itemIndex_2].item.ID)
                {
                    if (inventoryItems[itemIndex_1].quantity + inventoryItems[itemIndex_2].quantity <= inventoryItems[itemIndex_1].item.MaxStackSize)
                    {
                        //둘의 합이 최대 수량보다 적다면
                        //1을 지우고, 2를 그 수만큼 채운다.
                        inventoryItems[itemIndex_2] = inventoryItems[itemIndex_2].ChangeQuantity(inventoryItems[itemIndex_1].quantity + inventoryItems[itemIndex_2].quantity);
                        inventoryItems[itemIndex_1] = InventoryItem.GetEmptyItem();
                        InformAboutChange();
                    }
                    else
                    {
                        //도착점이 맥스가 아니라면, 도착점을 맥스로 정하고 1에 나머지를 넣는다.
                        int amount = (inventoryItems[itemIndex_2].quantity + inventoryItems[itemIndex_1].quantity) - inventoryItems[itemIndex_2].item.MaxStackSize;
                        inventoryItems[itemIndex_1] = inventoryItems[itemIndex_1].ChangeQuantity(amount);
                        inventoryItems[itemIndex_2] = inventoryItems[itemIndex_2].ChangeQuantity(inventoryItems[itemIndex_1].item.MaxStackSize);
                        InformAboutChange();
                    }
                }
            }
        }

        private void InformAboutChange()
        {
            //아이템이 스왑되었다면, 인벤토리에 변동이 있는 것이기에, 인벤토리 딕셔너리를 저장한다.
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }

    [Serializable]
    //객체배열보다 구조체배열이 좋음 좆까 객체가 더 좋음 썅 ㅋ
    public class InventoryItem                                                 //Inventory Item의 실체
    {
        public int quantity;
        public ItemQuality quality;
        public ItemSO item;
        public List<ItemParameter> itemState;
        public List<UpgradeResult> upgradeResults;
        public bool IsEmpty => item == null;

        public List<UpgradeResult> GetUpgradeResults()
        {
            List<UpgradeResult> a = new List<UpgradeResult>();
            a = upgradeResults;
            return a;
        }

        public void AddParameter(ItemParameter parameter)
        {
            if (FindIndex(parameter.itemParameter.ParameterCode) != -1)
                AddValue(parameter.itemParameter.ParameterCode, parameter.value);
            else
                itemState.Add(parameter);
        }

        public bool AddValue(int code, float value)
        {
            if (FindIndex(code) != -1)
            {
                itemState[FindIndex(code)] = itemState[FindIndex(code)].AddParameterValue(value);
                return true;
            }
            else
                return false;
        }

        public int FindIndex(int code)
        {
            for (int i = 0; i < itemState.Count; i++)
            {
                if (itemState[i].itemParameter.ParameterCode == code)
                    return i;
            }
            return -1;
        }

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity,
                itemState = new List<ItemParameter>(this.itemState),
                upgradeResults = new List<UpgradeResult>(this.upgradeResults)
            };
        }

        public static InventoryItem GetEmptyItem()
            => new InventoryItem
            {
                item = null,
                quantity = 0,
                itemState = new List<ItemParameter>(),
                upgradeResults = new List<UpgradeResult>()
            };
    }
}