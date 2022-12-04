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
        [SerializeField] private UIInventoryItem itemPrefab;                    //아이템 슬롯 프리펩
        [SerializeField] UIInventoryDescription itemDescription;                //아이템의 설명을 출력
        [SerializeField] MouseFollower mouseFollower;                           //마우스 추적자
        [SerializeField] RectTransform contentPanel;                            //슬롯을 보여줄 UI
        public List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();      //슬롯을 관리하는 리스트 0~35는 인벤, 36~45는 슬롯
        [SerializeField] RectTransform slotPanel;                               //슬롯페널
        private int currentlyDraggedItemIndex = -1;                             //드래그한 최근 아이템의 List인덱스 -1은 없음
        public event Action<int> OnDescriptionRequested,                        //아이템 설명 로드
                                    OnItemActionRequested,                      //아이템 액션(우클릭) 로드
                                    OnStartDragging;                            //아이템 드래깅
        public event Action<int, int> OnSwapItems;                              //아이템 스왑
        [SerializeField] public ItemActionPanel actionPanel;                    //액션패널
        [SerializeField] private InventorySO inventoryData;                     //플레이어의 인벤토리 데이터이다.
        [SerializeField] public GameObject mainCamera;
        [SerializeField] public InventoryController inventoryController;
        [SerializeField] public ItemUpgrade itemUpgrade; 

        RectTransform dr;
        bool isUpside;

        private void Awake()
        {
            //인벤토리 시작 설정
            Hide();
            //드래그 기능을 보여주는 mouseFollower의 Toggle기능을 일단 끈다.
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
                //inventorySize 만큼의 slot을 생성하여, UI에 자식으로 넣어주고, UI는 이를 토대로 정렬하며, 리스트에 이 슬롯들에 접근할 수 있는 값을 저장한다.
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
         */

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, int type, ItemQuality quality, string description)
        {
            //아이템의 설명을 설정함.
            //위치 설정
            dr = listOfUIItems[itemIndex].GetComponent<RectTransform>();
            itemDescription.gameObject.SetActive(true);
            itemDescription.SetDescription(itemImage, name, type, quality, description);
            if (itemIndex < 18 || itemIndex > 35)
                isUpside = true;
            else
                isUpside = false;
            //아이템 선택
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity, ItemQuality quality)
        {
            //아이템이 가득차지 않았다면, 아이템을 Index에 추가한다.
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity, quality);
            }
        }
        /*
        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity, ItemQuality quality)
        {
            //아이템이 가득차지 않았다면, 아이템을 Index에 추가한다.
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity, quality);
            }
        }
         */

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            //마우스 우클릭 입력시, 액션바 호출
            actionPanel.Toggle(true);
            if(inventoryItemUI.empty)
                actionPanel.Toggle(false);
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)                        //빈칸이라면
                return;
            if (index >= 36)                        //슬롯바라면 실행종료
                return;
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            //드래그 종료시 토글을 끄고 최근 인덱스를 초기화한다.
            ResetDraggedItem();
            ResetSelection();
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)        //inventoryItemUI = 도착하는 아이템
        {
            //아이템을 드랍할 시
            int index = listOfUIItems.IndexOf(inventoryItemUI);     //index = 도착 index, currentlyDraggedItemIndex = 출발 index
            InventoryItem inventoryItem = inventoryData.GetItemAt(currentlyDraggedItemIndex);   //출발위치 아이템
            InventoryItem destinationItem = inventoryData.GetItemAt(index);                     //도착위치 아이템

            //요구 레벨 제한
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

            if (index == currentlyDraggedItemIndex)     //같은 장소에 스왑이라면 취소한다.
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

        public void CreateDraggedItem(Sprite sprite, int quantity, ItemQuality quality)
        {
            //아이템을 드래깅아이템에 생성함.
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity, quality);
        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            //아이템을 클릭할 시
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
            OnDescriptionRequested?.Invoke(index);      //설명을 띄움.
        }

        public void Show()
        {
            //인벤토리를 활성화하며, 설명을 초기화한다.
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
            //인벤토리를 비활성화한다.
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