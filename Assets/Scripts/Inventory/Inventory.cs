using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private WeaponManagement weaponPrefab;
    [SerializeField] private RectTransform contentPanel;

    List<WeaponManagement> listOfWeapons = new List<WeaponManagement> ();

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

    private void HandleShowItemAction(WeaponManagement obj)
    {

    }

    private void HandleEndDrag(WeaponManagement obj)
    {

    }

    private void HandleSwap(WeaponManagement obj)
    {

    }

    private void HandleBeginDrag(WeaponManagement obj)
    {

    }

    private void HandleItemSelection(WeaponManagement obj)
    {
        Debug.Log(obj.name);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
