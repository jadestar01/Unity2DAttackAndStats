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
    [SerializeField] private EquippableItemSO weapon;                   //���� ����
    [SerializeField] private InventorySO inventoryData;                 //�κ��丮 ������
    [SerializeField] private WeaponController weaponController;         //���� ���� Ȯ��
    [SerializeField] private List<ItemParameter> parametersToModify;    //�Ķ����
    [SerializeField] private List<ItemParameter> itemCurrentState;      //�ֽŻ���
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

    private ItemSO melee;           //����
    private ItemSO magic;           //����
    private ItemSO range;           //���Ÿ�
    private ItemSO consume01;       //�Ҹ�1
    private ItemSO consume02;       //�Ҹ�2
    private ItemSO trinket01;       //��ű�1
    private ItemSO trinket02;       //��ű�2
    private ItemSO trinket03;       //��ű�3
    private ItemSO trinket04;       //��ű�4
    private ItemSO trinket05;       //��ű�5

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

        if (currentMelee != equippedMelee)      //�������� ������� �ٸ� ��,
        {
            currentMelee = equippedMelee;
            if (equippedMelee == null)           //������� null�̶��
                Destroy(Melee.transform.GetChild(0).gameObject);    //��� �����Ѵ�.
            else                                 //������� null�� �ƴ϶��,
            {
                if(Melee.transform.childCount != 0 && Melee.transform.childCount != 0)
                    Destroy(Melee.transform.GetChild(0).gameObject);    //��� �����ϰ�,
                GameObject item = Instantiate(equippedMelee.weapon, Melee.transform.position, Quaternion.identity);
                item.transform.SetParent(Melee.transform);
            }
        }

        if (magic == null)
            equippedMagic = null;
        else
            equippedMagic = (EquippableItemSO)magic;

        if (currentMagic != equippedMagic)      //�������� ������� �ٸ� ��,
        {
            currentMagic = equippedMagic;
            if (equippedMagic == null && Magic.transform.childCount != 0)           //������� null�̶��
                Destroy(Magic.transform.GetChild(0).gameObject);    //��� �����Ѵ�.
            else                                 //������� null�� �ƴ϶��,
            {
                if (Magic.transform.childCount != 0)
                    Destroy(Magic.transform.GetChild(0).gameObject);    //��� �����ϰ�,
                GameObject item = Instantiate(equippedMagic.weapon, Magic.transform.position, Quaternion.identity);
                item.transform.SetParent(Magic.transform);
            }
        }

        if (range == null)
            equippedRange = null;
        else
            equippedRange = (EquippableItemSO)range;

        if (currentRange != equippedRange)      //�������� ������� �ٸ� ��,
        {
            currentRange = equippedRange;
            if (equippedRange == null)           //������� null�̶��
                Destroy(Range.transform.GetChild(0).gameObject);    //��� �����Ѵ�.
            else                                 //������� null�� �ƴ϶��,
            {
                if (Range.transform.childCount != 0 && Range.transform.childCount != 0)
                    Destroy(Range.transform.GetChild(0).gameObject);    //��� �����ϰ�,
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
        //�Ķ���� ����
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
