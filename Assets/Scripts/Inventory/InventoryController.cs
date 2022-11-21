using Inventory.UI;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UIElements;
using ColorPallete;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Canvas inventoryCanvas;
        [SerializeField] private UIInventoryPage inventoryUI;                   //�κ��丮 UI�� �����Ѵ�.
        [SerializeField] private InventorySO inventoryData;                     //�÷��̾��� �κ��丮 �������̴�.
        public List<InventoryItem> initialItems = new List<InventoryItem>();    //�κ��丮 ������
        [SerializeField] private AudioClip dropClip;                            //������ ����� �Ҹ�
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            inventoryCanvas.gameObject.SetActive(false);
        }

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
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity, item.Value.quality);
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
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                //������ ����
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }

        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                audioSource.PlayOneShot(itemAction.actionSFX);
                if (inventoryItem.item.Type == ItemSO.ItemType.Melee || inventoryItem.item.Type == ItemSO.ItemType.Magic || inventoryItem.item.Type == ItemSO.ItemType.Range)
                    inventoryUI.actionPanel.Toggle(false);
                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                {
                    inventoryUI.ResetSelection();
                }
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }

        private void HandleDragging(int itemIndex)
        {
            //�巡�׸� ���� ���� �� ĭ�� �ƴ϶��, �巡�� �̹��� ����
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.ResetSelection();   //11
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity, inventoryItem.quality);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            //�������� �ִ� ��ġ�� �˾Ƴ� ��, �������� ������ ������Ʈ�ϸ�, �������� �����Ѵ�.
            inventoryUI.ResetDescription();
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                //�� ���� Ŭ���ϸ�, ������ �ʱ�ȭ�Ѵ�.
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.Name, (int)item.Type, item.Quality, description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();

            //�����Ķ����
            if (FindParamterCode(inventoryItem.itemState, 10) != -1 ||
               FindParamterCode(inventoryItem.itemState, 11) != -1 ||
               FindParamterCode(inventoryItem.itemState, 12) != -1 ||
               FindParamterCode(inventoryItem.itemState, 13) != -1 ||
               FindParamterCode(inventoryItem.itemState, 14) != -1 ||
               FindParamterCode(inventoryItem.itemState, 15) != -1)
            {
                sb.Append($"<size=1>");
                sb.AppendLine();
                sb.Append($"</size>");
            }
            //���������
            if (FindParamterCode(inventoryItem.itemState, 10) != -1 && FindParamterCode(inventoryItem.itemState, 11) != -1)
            {
                sb.Append($"��������� : " +
                $"{inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 10)].value} ~ " +
                $"{inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 11)].value}");
                sb.AppendLine();
            }
            //ġ��Ÿ��
            if (FindParamterCode(inventoryItem.itemState, 12) != -1)
            {
                sb.Append($"ġ��Ÿ�� : " +
                $"{inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 12)].value}%");
                sb.AppendLine();
            }
            //ġ��Ÿ�����
            if (FindParamterCode(inventoryItem.itemState, 13) != -1)
            {
                sb.Append($"ġ��Ÿ ����� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 13)].value}%");
                sb.AppendLine();
            }
            //���ݼӵ�
            if (FindParamterCode(inventoryItem.itemState, 14) != -1)
            {
                sb.Append($"���ݼӵ� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 14)].value}%");
                sb.AppendLine();
            }
            //�������
            if (FindParamterCode(inventoryItem.itemState, 15) != -1)
            {
                sb.Append($"������� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 14)].value}");
                sb.AppendLine();
            }

            //�����Ķ����
            if (FindParamterCode(inventoryItem.itemState, 20) != -1 ||
                FindParamterCode(inventoryItem.itemState, 21) != -1 ||
                FindParamterCode(inventoryItem.itemState, 22) != -1 ||
                FindParamterCode(inventoryItem.itemState, 23) != -1 ||
                FindParamterCode(inventoryItem.itemState, 24) != -1 ||
                FindParamterCode(inventoryItem.itemState, 25) != -1)
            {
                sb.Append($"<size=10>");
                sb.AppendLine();
                sb.Append($"</size>");
            }
            //���������
            if (FindParamterCode(inventoryItem.itemState, 20) != -1 && FindParamterCode(inventoryItem.itemState, 21) != -1)
            {
                sb.Append($"��������� : " +
                $"{inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 20)].value} ~ " +
                $"{inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 21)].value}");
                sb.AppendLine();
            }
            //�ش�ȭ��
            if (FindParamterCode(inventoryItem.itemState, 22) != -1)
            {
                sb.Append($"�ش�ȭ�� : " +
                $"{inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 22)].value}%");
                sb.AppendLine();
            }
            //�ش�ȭ�����
            if (FindParamterCode(inventoryItem.itemState, 23) != -1)
            {
                sb.Append($"�ش�ȭ ����� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 23)].value}%");
                sb.AppendLine();
            }
            //�ֹ��ӵ�
            if (FindParamterCode(inventoryItem.itemState, 24) != -1)
            {
                sb.Append($"�ֹ��ӵ� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 23)].value}%");
                sb.AppendLine();
            }
            //���װ����
            if (FindParamterCode(inventoryItem.itemState, 25) != -1)
            {
                sb.Append($"���װ���� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 23)].value}");
                sb.AppendLine();
            }

            //��ƿ�Ķ����
            if (FindParamterCode(inventoryItem.itemState, 0) != -1 ||
                FindParamterCode(inventoryItem.itemState, 1) != -1 ||
                FindParamterCode(inventoryItem.itemState, 2) != -1 ||
                FindParamterCode(inventoryItem.itemState, 3) != -1 ||
                FindParamterCode(inventoryItem.itemState, 4) != -1 ||
                FindParamterCode(inventoryItem.itemState, 5) != -1)
            {
                sb.Append($"<size=10>");
                sb.AppendLine();
                sb.Append($"</size>");
            }
            //ü��
            if (FindParamterCode(inventoryItem.itemState, 0) != -1)
            {
                sb.Append($"ü�� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 0)].value}");
                sb.AppendLine();
            }
            //����
            if (FindParamterCode(inventoryItem.itemState, 1) != -1)
            {
                sb.Append($"���� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 1)].value}");
                sb.AppendLine();
            }
            //���
            if (FindParamterCode(inventoryItem.itemState, 2) != -1)
            {
                sb.Append($"��� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 2)].value}");
                sb.AppendLine();
            }
            //�ż�
            if (FindParamterCode(inventoryItem.itemState, 3) != -1)
            {
                sb.Append($"�ż� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 3)].value}%");
                sb.AppendLine();
            }
            //����
            if (FindParamterCode(inventoryItem.itemState, 4) != -1)
            {
                sb.Append($"���� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 4)].value}%");
                sb.AppendLine();
            }
            //���
            if (FindParamterCode(inventoryItem.itemState, 5) != -1)
            {
                sb.Append($"��� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 5)].value}");
                sb.AppendLine();
            }

            //����Ķ����
            if (FindParamterCode(inventoryItem.itemState, 30) != -1 ||
                FindParamterCode(inventoryItem.itemState, 31) != -1 ||
                FindParamterCode(inventoryItem.itemState, 32) != -1 ||
                FindParamterCode(inventoryItem.itemState, 33) != -1 ||
                FindParamterCode(inventoryItem.itemState, 34) != -1)
            {
                sb.Append($"<size=10>");
                sb.AppendLine();
                sb.Append($"</size>");
            }
            //����
            if (FindParamterCode(inventoryItem.itemState, 30) != -1)
            {
                sb.Append($"���� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 0)].value}");
                sb.AppendLine();
            }
            //���׷�
            if (FindParamterCode(inventoryItem.itemState, 31) != -1)
            {
                sb.Append($"���׷� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 0)].value}");
                sb.AppendLine();
            }
            //ȸ��
            if (FindParamterCode(inventoryItem.itemState, 32) != -1)
            {
                sb.Append($"ȸ�� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 0)].value}%");
                sb.AppendLine();
            }
            //�ټ�
            if (FindParamterCode(inventoryItem.itemState, 33) != -1)
            {
                sb.Append($"�ټ� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 33)].value}");
                sb.AppendLine();
            }
            //����
            if (FindParamterCode(inventoryItem.itemState, 34) != -1)
            {
                sb.Append($"���� : " +
                $"+ {inventoryItem.itemState[FindParamterCode(inventoryItem.itemState, 0)].value}%");
                sb.AppendLine();
            }


            sb.Append($"<size=10>");
            sb.AppendLine();
            sb.Append($"</size>");
            sb.Append(inventoryItem.item.Description);
            return sb.ToString();
        }

        private int FindParamterCode(List<ItemParameter> list, int code)    //code�� index���� ���´�.
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].itemParameter.ParameterCode == code)
                    return i;
            return -1;
        }
        /*
        private string PrepareDescription(InventoryItem inventoryItem)
        {
            //�����߰�
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName}" +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            sb.Append(inventoryItem.item.Description);
            return sb.ToString();
        }
         */

        public void Update()
        {
            foreach (var item in inventoryData.GetCurrentInventoryState())
            {
                inventoryUI.UpdateData(
                    item.Key,
                    item.Value.item.ItemImage,
                    item.Value.quantity,
                    item.Value.item.Quality);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryCanvas.gameObject.SetActive(true);
                    inventoryUI.Show();
                    inventoryUI.ResetSelection();
                }
                else
                {
                    inventoryCanvas.gameObject.SetActive(false);
                    inventoryUI.Hide();
                    inventoryUI.ResetSelection();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && inventoryUI.isActiveAndEnabled == true)
            {
                inventoryCanvas.gameObject.SetActive(false);
                inventoryUI.Hide();
                inventoryUI.ResetSelection();
            }
        }
    }
}