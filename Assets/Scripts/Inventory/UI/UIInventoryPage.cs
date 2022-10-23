using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] public Canvas mainCanvas;                         //���� ų ĵ����
        [SerializeField] private UIInventoryItem itemPrefab;                //������ ���� ������
        [SerializeField] RectTransform contentPanel;                        //������ ������ UI
        [SerializeField] UIInventoryDescription itemDescription;            //�������� ������ ���
        [SerializeField] MouseFollower mouseFollower;                       //���콺 ������   
        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();  //������ �����ϴ� ����Ʈ
        private int currentlyDraggedItemIndex = -1;                         //�巡���� �ֱ� �������� List�ε��� -1�� ����
        public event Action<int> OnDescriptionRequested,                    //������ ���� �ε�
                                    OnItemActionRequested,                  //������ �׼�(��Ŭ��) �ε�
                                    OnStartDragging;                        //������ �巡��
        public event Action<int, int> OnSwapItems;                          //������ ����
        [SerializeField] private ItemActionPanel actionPanel;               //�׼��г�

        private void Awake()
        {
            //�κ��丮 ���� ����
            Hide();
            //�巡�� ����� �����ִ� mouseFollower�� Toggle����� �ϴ� ����.
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
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

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            //�������� �������� �ʾҴٸ�, �������� Index�� �߰��Ѵ�.
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            //���콺 ��Ŭ�� �Է½�, �׼ǹ� ȣ��
            actionPanel.Toggle(true);
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)                        //��ĭ�̶��
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            //�巡�� ����� ����� ���� �ֱ� �ε����� �ʱ�ȭ�Ѵ�.
            ResetDraggedItem();
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            //�������� ����� ��
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)                        //��ĭ�̶��
            {
                return;
            }
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

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            //�������� �巡������ۿ� ������.
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            //�������� Ŭ���� ��
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            //�κ��丮�� Ȱ��ȭ�ϸ�, ������ �ʱ�ȭ�Ѵ�.
            //gameObject.SetActive(true);
            mainCanvas.gameObject.SetActive(true);
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
            mainCanvas.enabled = true;
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
            actionPanel.Toggle(false);
            //gameObject.SetActive(false);
            mainCanvas.gameObject.SetActive(false);
            ResetDraggedItem();
        }
    }
}