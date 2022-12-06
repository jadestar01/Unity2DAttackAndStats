using Inventory.UI;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UIElements;
using ColorPallete;
using System.Drawing;
using static UnityEditor.Progress;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;
using UnityEngine.UI;
using Newtonsoft.Json.Bson;
using System.Reflection;
using UnityEditor;
using DG.Tweening.Core;
using DG.Tweening;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private PlayerLevel playerLevel;
        [SerializeField] private Canvas inventoryCanvas;
        [SerializeField] private UIInventoryPage inventoryUI;                   //�κ��丮 UI�� �����Ѵ�.
        [SerializeField] private InventorySO inventoryData;                     //�÷��̾��� �κ��丮 �������̴�.
        public List<InventoryItem> initialItems = new List<InventoryItem>();    //�κ��丮 ������
        [SerializeField] private AudioClip dropClip;                            //������ ����� �Ҹ�
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject mainCamera;
        public bool isUpgrade = false;
        public int upgradeMaterialIndex;
        public Dictionary<int, bool> consumeCooltime = new Dictionary<int, bool>();
        public Dictionary<int, Cooltime> cooltime = new Dictionary<int, Cooltime>();
        public MessageManager messageManager;
        public AgentWeapon agentWeapon;

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

        private void InventorySorting()
        {
            //���� ���
            //�ֹٸ� �����ϸ� �� ��.
            //������ ��, �� ĭ�� ���� ��
        }

        public class Cooltime
        {
            public bool canUse;
            public float time = 0.0f;

            public float CooltimeStart(float duration)
            {
                if (time == 0)
                {
                    canUse = false;
                    DOTween.To(() => time, x => time = x, 1, duration).SetEase(Ease.Linear);
                }
                if (time == 1) { time = 0; canUse = true; }
                return time;
            }

            public IEnumerator CoolReact(float duration)
            {
                yield return new WaitForSeconds(duration);
                time = 0.0f;
            }
        }

        void CooltimeIndicator()
        {
            foreach (KeyValuePair<int, bool> consume in consumeCooltime)
            {
                if (true)
                {
                    for (int i = 0; i < 46; i++)
                    {
                        if (!inventoryData.GetItemAt(i).IsEmpty)
                        {
                            int ID = inventoryData.GetItemAt(i).item.GetID();
                            if (consume.Key == ID)
                            {
                                if (consume.Value)
                                {
                                    if (inventoryUI.gameObject.activeSelf == true)
                                        inventoryUI.listOfUIItems[i].fill = cooltime[ID].time;
                                    if (i == 39)
                                        agentWeapon.listOfHotbar[3].fill = cooltime[ID].time;
                                    else if (i == 40)
                                        agentWeapon.listOfHotbar[4].fill = cooltime[ID].time;
                                }
                                else
                                {
                                    if (inventoryUI.gameObject.activeSelf == true)
                                        inventoryUI.listOfUIItems[i].fill = cooltime[ID].time;
                                    if (i == 39)
                                        agentWeapon.listOfHotbar[3].fill = cooltime[ID].time;
                                    else if (i == 40)
                                        agentWeapon.listOfHotbar[4].fill = cooltime[ID].time;
                                }
                            }
                            else
                            {
                                inventoryUI.listOfUIItems[i].fill = 0.0f;
                            }
                        }
                    }
                }
            }
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
                if (inventoryData.GetItemAt(itemIndex).item.Type == ItemSO.ItemType.NormalUpgrade ||
                   inventoryData.GetItemAt(itemIndex).item.Type == ItemSO.ItemType.SpecialUpgrade)
                {
                }
                else if (inventoryData.GetItemAt(itemIndex).item.Type == ItemSO.ItemType.Potion ||
                         inventoryData.GetItemAt(itemIndex).item.Type == ItemSO.ItemType.Scroll)
                {
                }
                else
                    inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                if (inventoryData.GetItemAt(itemIndex).item.Type == ItemSO.ItemType.NormalUpgrade ||
                   inventoryData.GetItemAt(itemIndex).item.Type == ItemSO.ItemType.SpecialUpgrade)
                {
                    //��ȭ
                    upgradeMaterialIndex = itemIndex;
                    isUpgrade = true;
                    inventoryUI.actionPanel.Toggle(false);
                    mainCamera.GetComponent<Mouse>().cursorType = Mouse.CursorType.Upgrade;
                }
                else if (inventoryData.GetItemAt(itemIndex).item.Type == ItemSO.ItemType.Potion ||
                         inventoryData.GetItemAt(itemIndex).item.Type == ItemSO.ItemType.Scroll)
                {
                    //�Ҹ�
                    EdibleItemSO consume = (EdibleItemSO)inventoryItem.item;
                    if (consumeCooltime.ContainsKey(consume.ID))
                    {
                        //ID�� ������ �ִٸ�,
                        if (consumeCooltime[consume.ID])
                        {
                            //cooltime�� true���,
                            //���
                            cooltime[consume.ID].CooltimeStart(consume.coolTime);
                            StartCoroutine(cooltime[consume.ID].CoolReact(consume.coolTime));
                            StartCoroutine(ConsumeCooltime(consume));
                            itemAction.PerformAction(gameObject, inventoryItem.itemState);
                            inventoryData.RemoveItem(itemIndex, 1);
                        }
                        else
                        {
                            //cooltime�� false���,
                            messageManager.Message("���� ����� �� �����ϴ�.");
                        }
                    }
                    else
                    {
                        consumeCooltime.Add(consume.ID, true);
                        cooltime.Add(consume.ID, new Cooltime { canUse = false, time = 0 });
                        cooltime[consume.ID].CooltimeStart(consume.coolTime);
                        StartCoroutine(cooltime[consume.ID].CoolReact(consume.coolTime));
                        //���
                        StartCoroutine(ConsumeCooltime(consume));
                        itemAction.PerformAction(gameObject, inventoryItem.itemState);
                        inventoryData.RemoveItem(itemIndex, 1);
                    }
                }
                else
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

        IEnumerator ConsumeCooltime(EdibleItemSO consume)
        {
            consumeCooltime[consume.ID] = false;
            yield return new WaitForSeconds(consume.coolTime);
            consumeCooltime[consume.ID] = true;
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
            //���뷹��
            if (FindParameterCode(inventoryItem.itemState, 6) != -1)
            {
                if (playerLevel.lv < inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 6)].value)
                {
                    sb.Append($"�䱸 ���� : " +
                    $"<color=#f2626e>{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 6)].value}</color>");
                    sb.AppendLine();
                }
                else
                {
                    sb.Append($"�䱸 ���� : " +
                    $"<color=#e1f63d>{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 6)].value}</color>");
                    sb.AppendLine();
                }
            }
            if (inventoryItem.item.Type == ItemSO.ItemType.NormalUpgrade || inventoryItem.item.Type == ItemSO.ItemType.SpecialUpgrade)
            {
                UpgradeItemSO upgrade = new UpgradeItemSO();
                upgrade = (UpgradeItemSO) inventoryItem.item;
                sb.Append($"������ : " +
                $"{upgrade.upgradeRate}" +
                $"%");
                sb.AppendLine();
                sb.Append($"������������ȭ�׸񡪡�����");
                sb.AppendLine();
            }
            //��ȭ
            if (FindParameterCode(inventoryItem.itemState, 7) != -1)
            {
                sb.Append($"��ȭ : ");
                for (int i = 0; i < inventoryItem.upgradeResults.Count; i++)
                {
                    if (inventoryItem.upgradeResults[i] == EquippableItemSO.UpgradeResult.Normal)
                        sb.Append($"��");
                    else if (inventoryItem.upgradeResults[i] == EquippableItemSO.UpgradeResult.Special)
                        sb.Append($"��");
                    else if (inventoryItem.upgradeResults[i] == EquippableItemSO.UpgradeResult.Fail)
                        sb.Append($"��");
                }
                for (int i = 0; i < inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 7)].value; i++)
                {
                    sb.Append($"��");
                }
                sb.AppendLine();

            }

                //�����Ķ����
                if (FindParameterCode(inventoryItem.itemState, 10) != -1 ||
               FindParameterCode(inventoryItem.itemState, 11) != -1 ||
               FindParameterCode(inventoryItem.itemState, 12) != -1 ||
               FindParameterCode(inventoryItem.itemState, 13) != -1 ||
               FindParameterCode(inventoryItem.itemState, 14) != -1 ||
               FindParameterCode(inventoryItem.itemState, 15) != -1)
            {
                sb.Append($"<size=10>");
                sb.AppendLine();
                sb.Append($"</size>");
            }
            //���������
            if (FindParameterCode(inventoryItem.itemState, 10) != -1 && FindParameterCode(inventoryItem.itemState, 11) != -1)
            {
                sb.Append($"��������� : " +
                $"{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 10)].value} ~ " +
                $"{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 11)].value}");
                sb.AppendLine();
            }
            //ġ��Ÿ��
            if (FindParameterCode(inventoryItem.itemState, 12) != -1)
            {
                sb.Append($"ġ��Ÿ�� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 12)].value}%");
                sb.AppendLine();
            }
            //ġ��Ÿ�����
            if (FindParameterCode(inventoryItem.itemState, 13) != -1)
            {
                sb.Append($"ġ��Ÿ ����� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 13)].value}%");
                sb.AppendLine();
            }
            //���ݼӵ�
            if (FindParameterCode(inventoryItem.itemState, 14) != -1)
            {
                sb.Append($"���ݼӵ� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 14)].value}%");
                sb.AppendLine();
            }
            //�������
            if (FindParameterCode(inventoryItem.itemState, 15) != -1)
            {
                sb.Append($"������� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 15)].value}");
                sb.AppendLine();
            }

            //�����Ķ����
            if (FindParameterCode(inventoryItem.itemState, 20) != -1 ||
                FindParameterCode(inventoryItem.itemState, 21) != -1 ||
                FindParameterCode(inventoryItem.itemState, 22) != -1 ||
                FindParameterCode(inventoryItem.itemState, 23) != -1 ||
                FindParameterCode(inventoryItem.itemState, 24) != -1 ||
                FindParameterCode(inventoryItem.itemState, 25) != -1)
            {
                sb.Append($"<size=10>");
                sb.AppendLine();
                sb.Append($"</size>");
            }
            //���������
            if (FindParameterCode(inventoryItem.itemState, 20) != -1 && FindParameterCode(inventoryItem.itemState, 21) != -1)
            {
                sb.Append($"��������� : " +
                $"{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 20)].value} ~ " +
                $"{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 21)].value}");
                sb.AppendLine();
            }
            //�ش�ȭ��
            if (FindParameterCode(inventoryItem.itemState, 22) != -1)
            {
                sb.Append($"�ش�ȭ�� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 22)].value}%");
                sb.AppendLine();
            }
            //�ش�ȭ�����
            if (FindParameterCode(inventoryItem.itemState, 23) != -1)
            {
                sb.Append($"�ش�ȭ ����� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 23)].value}%");
                sb.AppendLine();
            }
            //�ֹ��ӵ�
            if (FindParameterCode(inventoryItem.itemState, 24) != -1)
            {
                sb.Append($"�ֹ��ӵ� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 24)].value}%");
                sb.AppendLine();
            }
            //���װ����
            if (FindParameterCode(inventoryItem.itemState, 25) != -1)
            {
                sb.Append($"���װ���� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 25)].value}");
                sb.AppendLine();
            }

            //��ƿ�Ķ����
            if (FindParameterCode(inventoryItem.itemState, 0) != -1 ||
                FindParameterCode(inventoryItem.itemState, 1) != -1 ||
                FindParameterCode(inventoryItem.itemState, 2) != -1 ||
                FindParameterCode(inventoryItem.itemState, 3) != -1 ||
                FindParameterCode(inventoryItem.itemState, 4) != -1 ||
                FindParameterCode(inventoryItem.itemState, 5) != -1)
            {
                sb.Append($"<size=10>");
                sb.AppendLine();
                sb.Append($"</size>");
            }
            //ü��
            if (FindParameterCode(inventoryItem.itemState, 0) != -1)
            {
                sb.Append($"ü�� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 0)].value}");
                sb.AppendLine();
            }
            //����
            if (FindParameterCode(inventoryItem.itemState, 1) != -1)
            {
                sb.Append($"���� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 1)].value}");
                sb.AppendLine();
            }
            //���
            if (FindParameterCode(inventoryItem.itemState, 2) != -1)
            {
                sb.Append($"��� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 2)].value}");
                sb.AppendLine();
            }
            //�ż�
            if (FindParameterCode(inventoryItem.itemState, 3) != -1)
            {
                sb.Append($"�ż� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 3)].value}%");
                sb.AppendLine();
            }
            //����
            if (FindParameterCode(inventoryItem.itemState, 4) != -1)
            {
                sb.Append($"���� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 4)].value}%");
                sb.AppendLine();
            }
            //���
            if (FindParameterCode(inventoryItem.itemState, 5) != -1)
            {
                sb.Append($"��� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 5)].value}");
                sb.AppendLine();
            }
            //����
            if (FindParameterCode(inventoryItem.itemState, 8) != -1)
            {
                sb.Append($"���� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 8)].value}");
                sb.AppendLine();
            }

            //����Ķ����
            if (FindParameterCode(inventoryItem.itemState, 30) != -1 ||
                FindParameterCode(inventoryItem.itemState, 31) != -1 ||
                FindParameterCode(inventoryItem.itemState, 32) != -1 ||
                FindParameterCode(inventoryItem.itemState, 33) != -1 ||
                FindParameterCode(inventoryItem.itemState, 34) != -1)
            {
                sb.Append($"<size=10>");
                sb.AppendLine();
                sb.Append($"</size>");
            }
            //����
            if (FindParameterCode(inventoryItem.itemState, 30) != -1)
            {
                sb.Append($"���� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 30)].value}");
                sb.AppendLine();
            }
            //���׷�
            if (FindParameterCode(inventoryItem.itemState, 31) != -1)
            {
                sb.Append($"���׷� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 31)].value}");
                sb.AppendLine();
            }
            //ȸ��
            if (FindParameterCode(inventoryItem.itemState, 32) != -1)
            {
                sb.Append($"ȸ�� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 32)].value}%");
                sb.AppendLine();
            }
            //�ټ�
            if (FindParameterCode(inventoryItem.itemState, 33) != -1)
            {
                sb.Append($"�ټ� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 33)].value}");
                sb.AppendLine();
            }
            //����
            if (FindParameterCode(inventoryItem.itemState, 34) != -1)
            {
                sb.Append($"���� : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 34)].value}%");
                sb.AppendLine();
            }


            sb.Append($"<size=10>");
            sb.AppendLine();
            sb.Append($"</size>");
            sb.Append(inventoryItem.item.Description);
            return sb.ToString();
        }

        private int FindParameterCode(List<ItemParameter> list, int code)    //code�� index���� ���´�.
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
            foreach (KeyValuePair<int, Cooltime> aa in cooltime)
            {
                Debug.Log(aa.Key + ", " + aa.Value.time);
            }
            CooltimeIndicator();
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