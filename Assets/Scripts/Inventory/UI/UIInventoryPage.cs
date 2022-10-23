using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] public Canvas mainCanvas;                         //끄고 킬 캔버스
        [SerializeField] private UIInventoryItem itemPrefab;                //아이템 슬롯 프리펩
        [SerializeField] RectTransform contentPanel;                        //슬롯을 보여줄 UI
        [SerializeField] UIInventoryDescription itemDescription;            //아이템의 설명을 출력
        [SerializeField] MouseFollower mouseFollower;                       //마우스 추적자   
        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();  //슬롯을 관리하는 리스트
        private int currentlyDraggedItemIndex = -1;                         //드래그한 최근 아이템의 List인덱스 -1은 없음
        public event Action<int> OnDescriptionRequested,                    //아이템 설명 로드
                                    OnItemActionRequested,                  //아이템 액션(우클릭) 로드
                                    OnStartDragging;                        //아이템 드래깅
        public event Action<int, int> OnSwapItems;                          //아이템 스왑
        [SerializeField] private ItemActionPanel actionPanel;               //액션패널

        private void Awake()
        {
            //인벤토리 시작 설정
            Hide();
            //드래그 기능을 보여주는 mouseFollower의 Toggle기능을 일단 끈다.
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
                //inventorySize 만큼의 slot을 생성하여, UI에 자식으로 넣어주고, UI는 이를 토대로 정렬하며, 리스트에 이 슬롯들에 접근할 수 있는 값을 저장한다.
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                listOfUIItems.Add(uiItem);

                //생성된 slot에 각각의 이벤트가 수행될 수 있도록, 이벤트와 함수를 매칭시켜준다.
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
            //아이템이 가득차지 않았다면, 아이템을 Index에 추가한다.
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            //마우스 우클릭 입력시, 액션바 호출
            actionPanel.Toggle(true);
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)                        //빈칸이라면
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            //드래그 종료시 토글을 끄고 최근 인덱스를 초기화한다.
            ResetDraggedItem();
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            //아이템을 드랍할 시
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)                        //빈칸이라면
            {
                return;
            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);        //토글 종료
            currentlyDraggedItemIndex = -1;     //빈칸으로 설정
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            //드래그 시작시
            //마우스팔로워를 켜고, 이미지와 값을 복사해준다. (드래그 이벤트)
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)                        //빈칸 시 취소
                return;
            currentlyDraggedItemIndex = index;      //빈칸이 아니라면, 인덱스를 저장
            HandleItemSelection(inventoryItemUI);   //아이템 선택
            OnStartDragging?.Invoke(index);         //아이템스타트드래깅 이벤트 시작
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            //아이템을 드래깅아이템에 생성함.
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            //아이템을 클릭할 시
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            //인벤토리를 활성화하며, 설명을 초기화한다.
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
            //인벤토리를 비활성화한다.
            actionPanel.Toggle(false);
            //gameObject.SetActive(false);
            mainCanvas.gameObject.SetActive(false);
            ResetDraggedItem();
        }
    }
}