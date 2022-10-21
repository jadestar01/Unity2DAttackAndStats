using Inv.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Inv.UI
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private WeaponManagement weaponPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private Tooltip itemTooltip;
        [SerializeField] private MouseFollower mouseFollower;

        List<WeaponManagement> listOfWeapons = new List<WeaponManagement>();

        public Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
        public Action<int, int> OnSwapItems;

        [SerializeField] private ItemActionPanel actionPanel;

        private int currentlyDraggedItemIndex = -1;

        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            itemTooltip.ResetDescription();
        }

        //인벤토리 공간 확보
        public void InitalizeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                WeaponManagement uiWeapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity);
                uiWeapon.transform.SetParent(contentPanel);
                listOfWeapons.Add(uiWeapon);
                uiWeapon.OnItemClicked += HandleItemSelection;
                uiWeapon.OnItemBeginDrag += HandleBeginDrag;
                uiWeapon.OnItemDroppedOn += HandleSwap;
                uiWeapon.OnItemEndDrag += HandleEndDrag;
                uiWeapon.OnRightMouseBtnClick += HandleShowItemAction;
            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfWeapons.Count > itemIndex)
            {
                listOfWeapons[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleShowItemAction(WeaponManagement inventoryItemUI)
        {
            int index = listOfWeapons.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(WeaponManagement inventoryItemUI)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(WeaponManagement inventoryItemUI)
        {
            int index = listOfWeapons.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(WeaponManagement inventoryItemUI)
        {
            int index = listOfWeapons.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(WeaponManagement inventoryItemUI)
        {
            int index = listOfWeapons.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemTooltip.ResetDescription();
            DeselectAllItems();
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction);
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfWeapons[itemIndex].transform.position;
        }

        private void DeselectAllItems()
        {
            foreach (WeaponManagement item in listOfWeapons)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemTooltip.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfWeapons[itemIndex].Select();
        }

        internal void ResetAllItems()
        {
            foreach (var item in listOfWeapons)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}