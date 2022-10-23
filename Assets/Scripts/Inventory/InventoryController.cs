using Inventory.UI;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventoryPage inventoryUI;       //�κ��丮 UI�� �����Ѵ�.
        [SerializeField] private InventorySO inventoryData;         //�÷��̾��� �κ��丮 �������̴�.
        public List<InventoryItem> initialItems = new List<InventoryItem>();    //�κ��丮 ������

        private void Start()
        {
            //�κ��丮 ����
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();                             //�� InventoryItem ����ü�� �κ��丮�� �����Ѵ�.
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)            //�������� �ҷ��´�.
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            //�κ��丮�� ������Ʈ �Ǿ��ٸ�, �κ��丮 ��ųʸ��� ����, �ٽ� Ȯ�� �� �����Ѵ�.
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);  //�κ��丮 ������ �����, List�� �����Ѵ�.
                                                                    //�̺�Ʈ ���
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            //��Ŭ�� �Ҹ�, ��Ŭ�� ����, ��� �� �̺�Ʈ ����
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            //������ �Ҹ�
            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }
            //������ ���
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
            }
        }

        private void HandleDragging(int itemIndex)
        {
            //�巡���� ���� �� ĭ�� �ƴ϶��, �巡�� �̹��� ����
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            //�������� �ִ� ��ġ�� �˾Ƴ� ��, �������� ������ ������Ʈ�ϸ�, �������� �����Ѵ�.
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                //�� ���� Ŭ���ϸ�, ������ �ʱ�ȭ�Ѵ�.
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            //�����߰�
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName}" +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
            }
            return sb.ToString();
        }

        public void Update()
        {
            //�κ��丮 ���ݱ�
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())  //�κ��丮 ������ ����� ��ųʸ��� ��ȸ�ϸ�, ���� ��´�.
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                        //int index, Sprite sprite, int quantity //int Key, InventoryItem Value
                    }
                }
                else
                {
                    inventoryUI.Hide();
                }
            }
        }
    }
}