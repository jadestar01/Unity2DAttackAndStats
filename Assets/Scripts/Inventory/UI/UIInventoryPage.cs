using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ColorPallete;
using Inventory.Model;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] private PlayerLevel playerLevel;
        [SerializeField] private UIInventoryItem itemPrefab;                    //������ ���� ������
        [SerializeField] UIInventoryDescription itemDescription;                //�������� ������ ���
        [SerializeField] MouseFollower mouseFollower;                           //���콺 ������
        [SerializeField] RectTransform contentPanel;                            //������ ������ UI
        public List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();      //������ �����ϴ� ����Ʈ 0~35�� �κ�, 36~45�� ����
        [SerializeField] RectTransform slotPanel;                               //�������
        private int currentlyDraggedItemIndex = -1;                             //�巡���� �ֱ� �������� List�ε��� -1�� ����
        public event Action<int> OnDescriptionRequested,                        //������ ���� �ε�
                                    OnItemActionRequested,                      //������ �׼�(��Ŭ��) �ε�
                                    OnStartDragging;                            //������ �巡��
        public event Action<int, int> OnSwapItems;                              //������ ����
        [SerializeField] public ItemActionPanel actionPanel;                    //�׼��г�
        [SerializeField] private InventorySO inventoryData;                     //�÷��̾��� �κ��丮 �������̴�.
        [SerializeField] public GameObject mainCamera;
        [SerializeField] public InventoryController inventoryController;
        [SerializeField] public ItemUpgrade itemUpgrade; 

        RectTransform dr;
        bool isUpside;

        private void Awake()
        {
            //�κ��丮 ���� ����
            Hide();
            //�巡�� ����� �����ִ� mouseFollower�� Toggle����� �ϴ� ����.
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        private void Update()
        {
            if (itemDescription.isActiveAndEnabled)
            {
                if (isUpside)
                    itemDescription.transform.position = new Vector3(dr.transform.position.x, dr.transform.position.y - 60, 0);
                else
                    itemDescription.transform.position = new Vector3(dr.transform.position.x, dr.transform.position.y + 60 + itemDescription.nameHeight + itemDescription.descriptionHeight, 0);
            }
        }

        internal void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem;
                //inventorySize ��ŭ�� slot�� �����Ͽ�, UI�� �ڽ����� �־��ְ�, UI�� �̸� ���� �����ϸ�, ����Ʈ�� �� ���Ե鿡 ������ �� �ִ� ���� �����Ѵ�.
                if (i <= 35)
                {
                    uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                    uiItem.transform.SetParent(contentPanel);
                    listOfUIItems.Add(uiItem);
                    uiItem.OnItemClicked += HandleItemSelection;
                    uiItem.OnItemBeginDrag += HandleBeginDrag;
                    uiItem.OnItemDroppedOn += HandleSwap;
                    uiItem.OnItemEndDrag += HandleEndDrag;
                    uiItem.OnRightMouseBtnClick += HandleShowItemActions;
                }
                else if(i <= 45)
                {
                    uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                    uiItem.transform.SetParent(slotPanel);
                    listOfUIItems.Add(uiItem);
                    uiItem.OnItemClicked += HandleItemSelection;
                    uiItem.OnItemBeginDrag += HandleBeginDrag;
                    uiItem.OnItemDroppedOn += HandleSwap;
                    uiItem.OnItemEndDrag += HandleEndDrag;
                    uiItem.OnRightMouseBtnClick += HandleShowItemActions;
                }
            }
        }
        /*
        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                //inventorySize ��ŭ�� slot�� �����Ͽ�, UI�� �ڽ����� �־��ְ�, UI�� �̸� ���� �����ϸ�, ����Ʈ�� �� ���Ե鿡 ������ �� �ִ� ���� �����Ѵ�.
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                listOfUIItems.Add(uiItem);

                //������ slot�� ������ �̺�Ʈ�� ����� �� �ֵ���, �̺�Ʈ�� �Լ��� ��Ī�����ش�.
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }
         */

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, int type, ItemQuality quality, string description)
        {
            //�������� ������ ������.
            //��ġ ����
            dr = listOfUIItems[itemIndex].GetComponent<RectTransform>();
            itemDescription.gameObject.SetActive(true);
            itemDescription.SetDescription(itemImage, name, type, quality, description);
            if (itemIndex < 18 || itemIndex > 35)
                isUpside = true;
            else
                isUpside = false;
            //������ ����
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity, ItemQuality quality)
        {
            //�������� �������� �ʾҴٸ�, �������� Index�� �߰��Ѵ�.
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity, quality);
            }
        }
        /*
        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity, ItemQuality quality)
        {
            //�������� �������� �ʾҴٸ�, �������� Index�� �߰��Ѵ�.
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity, quality);
            }
        }
         */

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            //���콺 ��Ŭ�� �Է½�, �׼ǹ� ȣ��
            actionPanel.Toggle(true);
            if(inventoryItemUI.empty)
                actionPanel.Toggle(false);
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)                        //��ĭ�̶��
                return;
            if (index >= 36)                        //���Թٶ�� ��������
                return;
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            //�巡�� ����� ����� ���� �ֱ� �ε����� �ʱ�ȭ�Ѵ�.
            ResetDraggedItem();
            ResetSelection();
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)        //inventoryItemUI = �����ϴ� ������
        {
            //�������� ����� ��
            int index = listOfUIItems.IndexOf(inventoryItemUI);     //index = ���� index, currentlyDraggedItemIndex = ��� index
            InventoryItem inventoryItem = inventoryData.GetItemAt(currentlyDraggedItemIndex);   //�����ġ ������
            InventoryItem destinationItem = inventoryData.GetItemAt(index);                     //������ġ ������

            //�䱸 ���� ����
            bool inventoryItemRequiresLevel = false;
            int inventoryItemRequiersLevelExist = -1;
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
                if (inventoryItem.itemState[i].itemParameter.ParameterCode == 6)
                    inventoryItemRequiersLevelExist = i;
            if (inventoryItemRequiersLevelExist != -1)
                inventoryItemRequiresLevel = playerLevel.lv >= inventoryItem.itemState[inventoryItemRequiersLevelExist].value;
            else                                              
                inventoryItemRequiresLevel = true;
            bool destinationItemRequiresLevel = false;  
            int destinationItemRequiersLevelExist = -1;
            if (!destinationItem.IsEmpty)
            {
                for (int i = 0; i < destinationItem.itemState.Count; i++)
                    if (destinationItem.itemState[i].itemParameter.ParameterCode == 6)
                        destinationItemRequiersLevelExist = i;
                if (destinationItemRequiersLevelExist != -1)          
                    destinationItemRequiresLevel = playerLevel.lv >= destinationItem.itemState[destinationItemRequiersLevelExist].value;
                else                                                
                    destinationItemRequiresLevel = true;
            }
            else
                destinationItemRequiresLevel = true;
            if (index >= 36 && index <= 45 && !inventoryItemRequiresLevel)
                return;
            if (currentlyDraggedItemIndex >= 36 && currentlyDraggedItemIndex <= 45 && !destinationItemRequiresLevel)
                return;

            if (index == currentlyDraggedItemIndex)     //���� ��ҿ� �����̶�� ����Ѵ�.
                return;
            if (index == -1)
                return;
            if (!destinationItem.IsEmpty)
            {
                if (currentlyDraggedItemIndex == 36 && destinationItem.item.Type != ItemSO.ItemType.Melee)
                    return;
                if (currentlyDraggedItemIndex == 37 && destinationItem.item.Type != ItemSO.ItemType.Magic)
                    return;
                if (currentlyDraggedItemIndex == 38 && destinationItem.item.Type != ItemSO.ItemType.Range)
                    return;
                if ((currentlyDraggedItemIndex == 39 || currentlyDraggedItemIndex == 40) && destinationItem.item.Type != ItemSO.ItemType.Potion && destinationItem.item.Type != ItemSO.ItemType.Scroll)
                    return;
                if ((currentlyDraggedItemIndex == 41 || currentlyDraggedItemIndex == 42 || currentlyDraggedItemIndex == 43 || currentlyDraggedItemIndex == 44 || currentlyDraggedItemIndex == 45) && destinationItem.item.Type != ItemSO.ItemType.Trinket)
                    return;
            }
            //36 Melee, 37 Magic, 38 Range, 39~40 Consume, 41~45 Trinket
            if (index == 36 && inventoryItem.item.Type != ItemSO.ItemType.Melee)
                return;
            if (index == 37 && inventoryItem.item.Type != ItemSO.ItemType.Magic)
                return;
            if (index == 38 && inventoryItem.item.Type != ItemSO.ItemType.Range)
                return;
            if ((index == 39 || index == 40) && inventoryItem.item.Type != ItemSO.ItemType.Potion && inventoryItem.item.Type != ItemSO.ItemType.Scroll)
                return;
            if ((index == 41 || index == 42 || index == 43 || index == 44 || index == 45) && inventoryItem.item.Type != ItemSO.ItemType.Trinket)
                return;

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);        //��� ����
            currentlyDraggedItemIndex = -1;     //��ĭ���� ����
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            //�巡�� ���۽�
            //���콺�ȷο��� �Ѱ�, �̹����� ���� �������ش�. (�巡�� �̺�Ʈ)
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)                        //��ĭ �� ���
                return;
            currentlyDraggedItemIndex = index;      //��ĭ�� �ƴ϶��, �ε����� ����
            HandleItemSelection(inventoryItemUI);   //������ ����
            OnStartDragging?.Invoke(index);         //�����۽�ŸƮ�巡�� �̺�Ʈ ����
        }

        public void CreateDraggedItem(Sprite sprite, int quantity, ItemQuality quality)
        {
            //�������� �巡������ۿ� ������.
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity, quality);
        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            //�������� Ŭ���� ��
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            if (inventoryController.isUpgrade)
            {
                itemUpgrade.Upgrade(inventoryController.upgradeMaterialIndex, index);

                inventoryController.isUpgrade = false;
                mainCamera.GetComponent<Mouse>().cursorType = Mouse.CursorType.UI;
            }
            OnDescriptionRequested?.Invoke(index);      //������ ���.
        }

        public void Show()
        {
            //�κ��丮�� Ȱ��ȭ�ϸ�, ������ �ʱ�ȭ�Ѵ�.
            mainCamera.GetComponent<Mouse>().cursorType = Mouse.CursorType.UI;
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction);
        }


        public void ShowItemAction(int itemIndex)
        {
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
        }

        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        public void Hide()
        {
            //�κ��丮�� ��Ȱ��ȭ�Ѵ�.
            mainCamera.GetComponent<Mouse>().cursorType = Mouse.CursorType.Combat;
            inventoryController.isUpgrade = false;
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        internal void ResetDescription()
        {
            itemDescription.ResetDescription();
        }
    }
}