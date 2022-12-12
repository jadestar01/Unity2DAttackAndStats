using ColorPallete;
using Inventory.Model;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Inventory.Model.ItemSO;

public class DBCustomManager : MonoBehaviour
{
    //O : EquipItem
    //1 : ConsumeItem
    //2 : UpgradeItem
    [HideInInspector] public int DB = 0;
    [HideInInspector] public int currentDB = 0;
    public GameObject item;
    public GameObject listPanel;
    [HideInInspector] public DBLoader loader;
    [HideInInspector] public int currentKey = -1;
    [HideInInspector] public int Key = -1;
    public ItemSO Item;
    public ItemSO currentItem;
    public Sprite defaultSprite;

    [FoldoutGroup("EquipItem")] public GameObject E;
    [FoldoutGroup("EquipItem")] public UnityEngine.UI.Image E_itemImage;
    [FoldoutGroup("EquipItem")] public TMP_InputField E_itemCode;
    [FoldoutGroup("EquipItem")] public TMP_InputField E_itemName;
    [FoldoutGroup("EquipItem")] public TMP_Dropdown E_itemType;
    [FoldoutGroup("EquipItem")] public TMP_Dropdown E_itemQuality;
    [FoldoutGroup("EquipItem")] public TMP_InputField E_itemDescription;
    [FoldoutGroup("EquipItem")] public GameObject E_itemParameterPannel;
    [FoldoutGroup("EquipItem")] public GameObject E_itemParameter;

    [FoldoutGroup("UpgradeItem")] public GameObject U;
    [FoldoutGroup("UpgradeItem")] public UnityEngine.UI.Image U_itemImage;
    [FoldoutGroup("UpgradeItem")] public TMP_InputField U_itemCode;
    [FoldoutGroup("UpgradeItem")] public TMP_InputField U_itemName;
    [FoldoutGroup("UpgradeItem")] public TMP_Dropdown U_itemType;
    [FoldoutGroup("UpgradeItem")] public TMP_Dropdown U_itemQuality;
    [FoldoutGroup("UpgradeItem")] public TMP_InputField U_itemDescription;
    [FoldoutGroup("UpgradeItem")] public TMP_InputField U_itemRate;

    private void Start()
    {
        loader = gameObject.GetComponent<DBLoader>();
        ItemListSetting();
    }

    private void Update()
    {
        DBChanged();
        KeyChanged();
    }

    public void DBChanged()
    {
        if (currentDB != DB)
        {
            Debug.Log("DB Changed!");
            currentDB = DB;

            ItemListSetting();
        }
    }

    public void SetDB(int dbSet)
    {
        DB = dbSet;
    }

    public void ItemListSetting()
    {
        ItemListReset();
        if (currentDB == 0)
        {
            foreach (KeyValuePair<int, EquippableItemSO> DBItem in loader.EquipItemDB)
            {
                GameObject newItem = Instantiate(item, listPanel.transform);
                newItem.GetComponent<CodeName>().SetCodeName(DBItem.Key, DBItem.Value.Name);
            }
        }
        else if (currentDB == 1)
        {
            foreach (KeyValuePair<int, EdibleItemSO> DBItem in loader.ConsumeItemDB)
            {
                GameObject newItem = Instantiate(item, listPanel.transform);
                newItem.GetComponent<CodeName>().SetCodeName(DBItem.Key, DBItem.Value.Name);
            }
        }
        else if (currentDB == 2)
        {
            foreach (KeyValuePair<int, UpgradeItemSO> DBItem in loader.UpgradeItemDB)
            {
                GameObject newItem = Instantiate(item, listPanel.transform);
                newItem.GetComponent<CodeName>().SetCodeName(DBItem.Key, DBItem.Value.Name);
            }
        }
    }

    public void ItemListReset()
    {
        for (int i = 0; i < listPanel.transform.childCount; i++)
        {
            Destroy(listPanel.transform.GetChild(listPanel.transform.childCount - 1 - i).gameObject);
        }
    }

    public void KeyChanged()
    {
        if (currentKey != Key)
        {
            Debug.Log("Key Changed!");
            currentKey = Key;

            ItemSetting();
        }
    }
    public void ItemSetting()
    {
        if (Key == -1)
            return;
        else
        {
            if (DB == 0)
            {
                Item = loader.EquipItemDB[Key].DeepCopy();
                EquipItemSetting();
            }
            else if (DB == 1)
            {
                Item = loader.ConsumeItemDB[Key];
            }
            else if (DB == 2)
            {
                Item = loader.UpgradeItemDB[Key];
            }
        }
    }

    public void AddItem()
    {
        if (DB == 0)
        {
            int lastCode = 0;
            for (int i = 0; i < loader.EquipItemDB.Count; i++)
            {
                lastCode = i;
            }
            EquippableItemSO newItem = new EquippableItemSO
            {
                ItemImage = defaultSprite,
                Type = ItemType.Melee,
                Quality = ItemQuality.Normal,
                DefaultParametersList = new List<ItemParameter>(),
                DefaultUpgradeResults = new List<EquippableItemSO.UpgradeResult>()
            };
            loader.EquipItemDB.Add(lastCode + 1, newItem);
            Key = lastCode + 1;
            Item = loader.EquipItemDB[Key];
            ItemListSetting();
        }
        else if (DB == 1)
        {
        }
        else if (DB == 2)
        { 
        }
    }

    public void RemoveItem()
    {
        if (DB == 0)
        {
            loader.EquipItemDB.Remove(Key);
        }
        else if (DB == 1)
        {

        }
        else if (DB == 2)
        {
            
        }

        Key = -1;
        ItemListSetting();
    }

    public void CancelItem()
    { 
    }

    public void SaveItem()
    {
        //아이템을 삭제하고
        //그곳에 저장
    }

    public void EquipItemSetting()
    {
        for (int i = 0; i < E_itemParameterPannel.transform.childCount; i++)
        {
            Destroy(E_itemParameterPannel.transform.GetChild(E_itemParameterPannel.transform.childCount - 1 - i).gameObject);
        }
        E_itemImage.sprite = Item.ItemImage;
        E_itemCode.text = Key.ToString();
        E_itemCode.placeholder.GetComponent<TMP_Text>().text = Key.ToString();

        if (Item.Name == null)
        {
            E_itemName.text = "새 아이템";
            E_itemName.placeholder.GetComponent<TMP_Text>().text = "이름";
        }
        else
        {
            E_itemName.text = Item.Name;
            E_itemName.placeholder.GetComponent<TMP_Text>().text = Item.Name;
        }

        E_itemType.value = (int)Item.Type - 3;
        E_itemQuality.value = (int)Item.Quality;

        if (E_itemDescription.text == null)
        {
            E_itemDescription.placeholder.GetComponent<TMP_Text>().text = "아이템 설명";
        }
        else
        {
            E_itemDescription.text = Item.Description;
            E_itemDescription.placeholder.GetComponent<TMP_Text>().text = Item.Description;
        }
        for (int i = 0; i < Item.DefaultParametersList.Count; i++)
        {
            GameObject item = Instantiate(E_itemParameter);
            item.transform.SetParent(E_itemParameterPannel.transform);
            item.GetComponent<DefaultParameter>().SetParameter(Item.DefaultParametersList[i].itemParameter, Item.DefaultParametersList[i].value);
        }
        ItemListSetting();
    }

    public void UpgradeItemSetting()
    {

    }

    public void ParameterController(ItemParameterSO parameter)
    {
        int thereIsParameter = -1;
        for (int i = 0; i < Item.DefaultParametersList.Count; i++)
        {
            if(Item.DefaultParametersList[i].itemParameter.ParameterCode == parameter.ParameterCode)
                thereIsParameter = i;
        }

        if (thereIsParameter != -1)                                         //파라미터가 이미 있다면,
        {
            Item.DefaultParametersList.RemoveAt(thereIsParameter);
        }
        else                                                                //파라미터가 없다면,
        {
            Item.DefaultParametersList.Add(new ItemParameter { itemParameter = parameter, value = 0 });
        }

        if (DB == 0)
        {
            for (int i = 0; i < E_itemParameterPannel.transform.childCount; i++)
            {
                Destroy(E_itemParameterPannel.transform.GetChild(E_itemParameterPannel.transform.childCount - 1 - i).gameObject);
            }
            for (int i = 0; i < Item.DefaultParametersList.Count; i++)
            {
                GameObject item = Instantiate(E_itemParameter);
                item.transform.SetParent(E_itemParameterPannel.transform);
                item.GetComponent<DefaultParameter>().SetParameter(Item.DefaultParametersList[i].itemParameter, Item.DefaultParametersList[i].value);
            }
        }
    }

    public void ChangeItemCode()
    {
        if (DB == 0)
        {
            Item = loader.EquipItemDB[Key];
        }
        else if (DB == 1)
        {
            Item = loader.ConsumeItemDB[Key];
        }
        else if (DB == 2)
        {
            Item = loader.UpgradeItemDB[Key];
        }
    }
}
