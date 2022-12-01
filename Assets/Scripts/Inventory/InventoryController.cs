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

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private PlayerLevel playerLevel;
        [SerializeField] private Canvas inventoryCanvas;
        [SerializeField] private UIInventoryPage inventoryUI;                   //인벤토리 UI에 접근한다.
        [SerializeField] private InventorySO inventoryData;                     //플레이어의 인벤토리 데이터이다.
        public List<InventoryItem> initialItems = new List<InventoryItem>();    //인벤토리 시작템
        [SerializeField] private AudioClip dropClip;                            //아이템 드랍시 소리
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject mainCamera;
        public bool isUpgrade = false;
        public int upgradeMaterialIndex;

        private void Awake()
        {
            inventoryCanvas.gameObject.SetActive(false);
        }

        private void Start()
        {
            //인벤토리 생성
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();                             //빈 InventoryItem 구조체로 인벤토리를 연동한다.
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)            //시작템을 불러온다.
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            //인벤토리가 업데이트 되었다면, 인벤토리 딕셔너리를 비우고, 다시 확인 후 저장한다.
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity, item.Value.quality);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);  //인벤토리 슬롯을 만들고, List와 연동한다.
                                                                    //이벤트 기능
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
                //아이템 장착
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
                    //강화
                    upgradeMaterialIndex = itemIndex;
                    isUpgrade = true;
                    inventoryUI.actionPanel.Toggle(false);
                    mainCamera.GetComponent<Mouse>().cursorType = Mouse.CursorType.Upgrade;
                }
                else
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
            //드래그를 시작 곳이 빈 칸이 아니라면, 드래그 이미지 생성
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
            //아이템이 있는 위치를 알아낸 후, 아이템의 설명을 업데이트하며, 아이템을 선택한다.
            inventoryUI.ResetDescription();
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                //빈 곳을 클릭하면, 설명을 초기화한다.
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
            //착용레벨
            if (FindParameterCode(inventoryItem.itemState, 6) != -1)
            {
                if (playerLevel.lv < inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 6)].value)
                {
                    sb.Append($"요구 레벨 : " +
                    $"<color=#f2626e>{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 6)].value}</color>");
                    sb.AppendLine();
                }
                else
                {
                    sb.Append($"요구 레벨 : " +
                    $"<color=#e1f63d>{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 6)].value}</color>");
                    sb.AppendLine();
                }
            }
            //강화
            if (FindParameterCode(inventoryItem.itemState, 7) != -1)
            {
                sb.Append($"강화 : ");
                for (int i = 0; i < inventoryItem.item.DefaultParametersList[FindParameterCode(inventoryItem.itemState, 7)].value - inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 7)].value; i++)
                {
                    //강화
                    sb.Append($"◈");
                }
                for (int i = 0; i < inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 7)].value; i++)
                {
                    sb.Append($"◇");
                }
                sb.AppendLine();

            }

                //물리파라미터
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
            //물리대미지
            if (FindParameterCode(inventoryItem.itemState, 10) != -1 && FindParameterCode(inventoryItem.itemState, 11) != -1)
            {
                sb.Append($"물리대미지 : " +
                $"{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 10)].value} ~ " +
                $"{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 11)].value}");
                sb.AppendLine();
            }
            //치명타율
            if (FindParameterCode(inventoryItem.itemState, 12) != -1)
            {
                sb.Append($"치명타율 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 12)].value}%");
                sb.AppendLine();
            }
            //치명타대미지
            if (FindParameterCode(inventoryItem.itemState, 13) != -1)
            {
                sb.Append($"치명타 대미지 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 13)].value}%");
                sb.AppendLine();
            }
            //공격속도
            if (FindParameterCode(inventoryItem.itemState, 14) != -1)
            {
                sb.Append($"공격속도 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 14)].value}%");
                sb.AppendLine();
            }
            //방어관통력
            if (FindParameterCode(inventoryItem.itemState, 15) != -1)
            {
                sb.Append($"방어관통력 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 15)].value}");
                sb.AppendLine();
            }

            //마법파라미터
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
            //마법대미지
            if (FindParameterCode(inventoryItem.itemState, 20) != -1 && FindParameterCode(inventoryItem.itemState, 21) != -1)
            {
                sb.Append($"마법대미지 : " +
                $"{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 20)].value} ~ " +
                $"{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 21)].value}");
                sb.AppendLine();
            }
            //극대화율
            if (FindParameterCode(inventoryItem.itemState, 22) != -1)
            {
                sb.Append($"극대화율 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 22)].value}%");
                sb.AppendLine();
            }
            //극대화대미지
            if (FindParameterCode(inventoryItem.itemState, 23) != -1)
            {
                sb.Append($"극대화 대미지 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 23)].value}%");
                sb.AppendLine();
            }
            //주문속도
            if (FindParameterCode(inventoryItem.itemState, 24) != -1)
            {
                sb.Append($"주문속도 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 24)].value}%");
                sb.AppendLine();
            }
            //저항관통력
            if (FindParameterCode(inventoryItem.itemState, 25) != -1)
            {
                sb.Append($"저항관통력 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 25)].value}");
                sb.AppendLine();
            }

            //유틸파라미터
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
            //체력
            if (FindParameterCode(inventoryItem.itemState, 0) != -1)
            {
                sb.Append($"체력 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 0)].value}");
                sb.AppendLine();
            }
            //마나
            if (FindParameterCode(inventoryItem.itemState, 1) != -1)
            {
                sb.Append($"마나 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 1)].value}");
                sb.AppendLine();
            }
            //기력
            if (FindParameterCode(inventoryItem.itemState, 2) != -1)
            {
                sb.Append($"기력 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 2)].value}");
                sb.AppendLine();
            }
            //신속
            if (FindParameterCode(inventoryItem.itemState, 3) != -1)
            {
                sb.Append($"신속 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 3)].value}%");
                sb.AppendLine();
            }
            //가속
            if (FindParameterCode(inventoryItem.itemState, 4) != -1)
            {
                sb.Append($"가속 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 4)].value}%");
                sb.AppendLine();
            }
            //충격
            if (FindParameterCode(inventoryItem.itemState, 5) != -1)
            {
                sb.Append($"충격 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 5)].value}");
                sb.AppendLine();
            }

            //방어파라미터
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
            //방어력
            if (FindParameterCode(inventoryItem.itemState, 30) != -1)
            {
                sb.Append($"방어력 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 0)].value}");
                sb.AppendLine();
            }
            //저항력
            if (FindParameterCode(inventoryItem.itemState, 31) != -1)
            {
                sb.Append($"저항력 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 0)].value}");
                sb.AppendLine();
            }
            //회피
            if (FindParameterCode(inventoryItem.itemState, 32) != -1)
            {
                sb.Append($"회피 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 0)].value}%");
                sb.AppendLine();
            }
            //근성
            if (FindParameterCode(inventoryItem.itemState, 33) != -1)
            {
                sb.Append($"근성 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 33)].value}");
                sb.AppendLine();
            }
            //감쇄
            if (FindParameterCode(inventoryItem.itemState, 34) != -1)
            {
                sb.Append($"감쇄 : " +
                $"+{inventoryItem.itemState[FindParameterCode(inventoryItem.itemState, 0)].value}%");
                sb.AppendLine();
            }


            sb.Append($"<size=10>");
            sb.AppendLine();
            sb.Append($"</size>");
            sb.Append(inventoryItem.item.Description);
            return sb.ToString();
        }

        private int FindParameterCode(List<ItemParameter> list, int code)    //code의 index값을 얻어온다.
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].itemParameter.ParameterCode == code)
                    return i;
            return -1;
        }
        /*
        private string PrepareDescription(InventoryItem inventoryItem)
        {
            //설명추가
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