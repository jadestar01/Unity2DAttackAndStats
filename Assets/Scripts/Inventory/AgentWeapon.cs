using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField] private EquippableItemSO weapon;                   //장착 무기
    [SerializeField] private InventorySO inventoryData;                 //인벤토리 데이터
    [SerializeField] private WeaponController weaponController;         //선택 웨폰 확인
    [SerializeField] private List<ItemParameter> parametersToModify;    //파라미터
    [SerializeField] private List<ItemParameter> itemCurrentState;      //최신상태
    [SerializeField] private GameObject Melee;
    [SerializeField] private GameObject Magic;
    [SerializeField] private GameObject Range;
    [SerializeField] private List<UIInventoryItem> listOfHotbar = new List<UIInventoryItem>();

    private EquippableItemSO currentMelee;
    private EquippableItemSO equippedMelee;

    private EquippableItemSO currentMagic;
    private EquippableItemSO equippedMagic;

    private EquippableItemSO currentRange;
    private EquippableItemSO equippedRange;

    private int currentWeapon = -1;

    private ItemSO melee;           //근접
    private ItemSO magic;           //마법
    private ItemSO range;           //원거리
    private ItemSO consume01;       //소모1
    private ItemSO consume02;       //소모2
    private ItemSO trinket01;       //장신구1
    private ItemSO trinket02;       //장신구2
    private ItemSO trinket03;       //장신구3
    private ItemSO trinket04;       //장신구4
    private ItemSO trinket05;       //장신구5

    private void Start()
    {
        currentMelee = null; equippedMelee = (EquippableItemSO)inventoryData.GetItemAt(36).item; if (Melee.transform.childCount != 0) Destroy(Melee.transform.GetChild(0).gameObject);
        currentMagic = null; equippedMagic = (EquippableItemSO)inventoryData.GetItemAt(37).item; if (Magic.transform.childCount != 0) Destroy(Magic.transform.GetChild(0).gameObject);
        currentRange = null; equippedRange = (EquippableItemSO)inventoryData.GetItemAt(38).item; if (Range.transform.childCount != 0) Destroy(Range.transform.GetChild(0).gameObject);
    }

    private void Update()
    {
        DataSet();
        HotbarSet();
        WeaponSet();
    }

    public void HotbarSet()
    {
        int weaponActive = weaponController.weaponActive;
        if (currentWeapon != weaponActive)
        {
            if(currentWeapon != -1)
                listOfHotbar[currentWeapon].Deselect();
            currentWeapon = weaponActive;
            listOfHotbar[weaponActive].Select();
        }
        for (int i = 0; i < 5; i++)
        {
            InventoryItem item = inventoryData.GetItemAt(i + 36);
            if (item.IsEmpty)
                listOfHotbar[i].ResetData();
            else
                listOfHotbar[i].SetData(item.item.ItemImage, item.quantity, item.item.Quality);
        }
    }

    public void DataSet()
    {
        //0~35 = inventory, 36 = melee, 37 = magic, 38 = range, 39~40 = consume, 41~45 = Trinket
        melee = inventoryData.GetItemAt(36).item;
        magic = inventoryData.GetItemAt(37).item;
        range = inventoryData.GetItemAt(38).item;
        consume01 = inventoryData.GetItemAt(39).item;
        consume02 = inventoryData.GetItemAt(40).item;
        trinket01 = inventoryData.GetItemAt(41).item;
        trinket02 = inventoryData.GetItemAt(42).item;
        trinket03 = inventoryData.GetItemAt(43).item;
        trinket04 = inventoryData.GetItemAt(44).item;
        trinket05 = inventoryData.GetItemAt(45).item;
    }

    public void WeaponSet()
    {
        if (melee == null)
            equippedMelee = null;
        else
            equippedMelee = (EquippableItemSO)melee;

        if (currentMelee != equippedMelee)      //착용장비와 현재장비가 다를 때,
        {
            currentMelee = equippedMelee;
            if (equippedMelee == null)           //착용장비가 null이라면
                Destroy(Melee.transform.GetChild(0).gameObject);    //장비를 해제한다.
            else                                 //착용장비가 null이 아니라면,
            {
                if(Melee.transform.childCount != 0 && Melee.transform.childCount != 0)
                    Destroy(Melee.transform.GetChild(0).gameObject);    //장비를 해제하고,
                GameObject item = Instantiate(equippedMelee.weapon, Melee.transform.position, Quaternion.identity);
                item.transform.SetParent(Melee.transform);
            }
        }

        if (magic == null)
            equippedMagic = null;
        else
            equippedMagic = (EquippableItemSO)magic;

        if (currentMagic != equippedMagic)      //착용장비와 현재장비가 다를 때,
        {
            currentMagic = equippedMagic;
            if (equippedMagic == null && Magic.transform.childCount != 0)           //착용장비가 null이라면
                Destroy(Magic.transform.GetChild(0).gameObject);    //장비를 해제한다.
            else                                 //착용장비가 null이 아니라면,
            {
                if (Magic.transform.childCount != 0)
                    Destroy(Magic.transform.GetChild(0).gameObject);    //장비를 해제하고,
                GameObject item = Instantiate(equippedMagic.weapon, Magic.transform.position, Quaternion.identity);
                item.transform.SetParent(Magic.transform);
            }
        }

        if (range == null)
            equippedRange = null;
        else
            equippedRange = (EquippableItemSO)range;

        if (currentRange != equippedRange)      //착용장비와 현재장비가 다를 때,
        {
            currentRange = equippedRange;
            if (equippedRange == null)           //착용장비가 null이라면
                Destroy(Range.transform.GetChild(0).gameObject);    //장비를 해제한다.
            else                                 //착용장비가 null이 아니라면,
            {
                if (Range.transform.childCount != 0 && Range.transform.childCount != 0)
                    Destroy(Range.transform.GetChild(0).gameObject);    //장비를 해제하고,
                GameObject item = Instantiate(equippedRange.weapon, Range.transform.position, Quaternion.identity);
                item.transform.SetParent(Range.transform);
            }
        }
    }

    /*
    private GameObject currentMelee;
    private GameObject equippedMelee;

    private GameObject currentMagic;
    private EquippableItemSO magicItem;
    private GameObject equippedMagic;

    private GameObject currentRange;
    private EquippableItemSO rangeItem;
    private GameObject equippedRange;


    public void WeaponSet2()
    {
        EquippableItemSO meleeItem = (EquippableItemSO)inventoryData.GetItemAt(36).item;
        equippedMelee = meleeItem.weapon;
        if (currentMelee != equippedMelee)
        {
            currentMelee = equippedMelee;
            Destroy(Melee.transform.GetChild(0).gameObject);
            if (inventoryData.GetItemAt(36).IsEmpty)
                equippedMelee = null;
            else
            {
                GameObject item = Instantiate(equippedMelee, gameObject.transform.position, Quaternion.identity);
                item.transform.SetParent(Melee.transform);
            }
        }
        if(equippedMelee == null) Destroy(Melee.transform.GetChild(0).gameObject);

        EquippableItemSO magicItem = (EquippableItemSO)inventoryData.GetItemAt(37).item;
        equippedMagic = magicItem.weapon;
        if (currentMagic != equippedMagic)
        {
            currentMagic = equippedMagic;
            Destroy(Magic.transform.GetChild(0).gameObject);
            if (inventoryData.GetItemAt(37).IsEmpty)
                equippedMagic = null;
            else
            {
                GameObject item = Instantiate(equippedMagic, gameObject.transform.position, Quaternion.identity);
                item.transform.SetParent(Magic.transform);
            }
        }
        if (currentMagic == null) Destroy(Magic.transform.GetChild(0).gameObject);

        EquippableItemSO rangeItem = (EquippableItemSO)inventoryData.GetItemAt(38).item;
        equippedRange = rangeItem.weapon;
        if (currentRange != equippedRange)
        {
            currentRange = equippedRange;
            Destroy(Range.transform.GetChild(0).gameObject);
            if (inventoryData.GetItemAt(38).IsEmpty)
                equippedRange = null;
            else
            {
                GameObject item = Instantiate(equippedRange, gameObject.transform.position, Quaternion.identity);
                item.transform.SetParent(Range.transform);
            }
        }
        if (currentRange == null) Destroy(Magic.transform.GetChild(0).gameObject);
    }
    */

    public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        if(weapon != null)
        {
            inventoryData.AddItem(weapon, 1, itemCurrentState);
        }

        this.weapon = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }

    private void ModifyParameters()
    {
        //파라미터 변경
        foreach (var parameter in parametersToModify)
        {
            if (itemCurrentState.Contains(parameter))
            {
                int index = itemCurrentState.IndexOf(parameter);
                float newValue = itemCurrentState[index].value + parameter.value;
                itemCurrentState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue
                };
            }
        }
    }
}
