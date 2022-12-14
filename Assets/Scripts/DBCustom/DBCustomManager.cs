using ColorPallete;
using Inventory.Model;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
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
    public int DB = 0;
    public int currentDB = 0;
    public GameObject item;
    public GameObject listPanel;
    [HideInInspector] public DBLoader loader;
    [HideInInspector] public int currentKey = -1;
    [HideInInspector] public int Key = -1;
    public EquippableItemSO equipItem;
    public EdibleItemSO consumeItem;
    public UpgradeItemSO upgradeItem;

    public EquippableItemSO currentEquipItem;
    public EdibleItemSO currentConsumeItem;
    public UpgradeItemSO currentUpgradeItem;

    public Sprite defaultSprite;

    [FoldoutGroup("EquipItem")] public GameObject E;
    [FoldoutGroup("EquipItem")] public UnityEngine.UI.Image E_itemImage;
    [FoldoutGroup("EquipItem")] public TMP_InputField E_itemCode;
    [FoldoutGroup("EquipItem")] public TMP_InputField E_itemName;
    [FoldoutGroup("EquipItem")] public TMP_Text E_itemWeapon;
    [FoldoutGroup("EquipItem")] public GameObject E_weapon;
    [FoldoutGroup("EquipItem")] public TMP_Dropdown E_itemType;
    [FoldoutGroup("EquipItem")] public TMP_Dropdown E_itemQuality;
    [FoldoutGroup("EquipItem")] public TMP_InputField E_itemDescription;
    [FoldoutGroup("EquipItem")] public GameObject E_itemParameterPannel;
    [FoldoutGroup("EquipItem")] public GameObject E_itemParameter;

    [FoldoutGroup("ConsumeItem")] public GameObject C;
    [FoldoutGroup("ConsumeItem")] public UnityEngine.UI.Image C_itemImage;
    [FoldoutGroup("ConsumeItem")] public TMP_InputField C_itemCode;
    [FoldoutGroup("ConsumeItem")] public TMP_InputField C_itemName;
    [FoldoutGroup("ConsumeItem")] public TMP_Dropdown C_itemType;
    [FoldoutGroup("ConsumeItem")] public TMP_Dropdown C_itemQuality;
    [FoldoutGroup("ConsumeItem")] public TMP_InputField C_itemDescription;
    [FoldoutGroup("ConsumeItem")] public TMP_InputField C_itemCooltime;
    [FoldoutGroup("ConsumeItem")] public GameObject C_itemModifierPannel;
    [FoldoutGroup("ConsumeItem")] public GameObject buffSlot;
    [FoldoutGroup("ConsumeItem")] public GameObject statSlot;

    [FoldoutGroup("UpgradeItem")] public GameObject U;
    [FoldoutGroup("UpgradeItem")] public UnityEngine.UI.Image U_itemImage;
    [FoldoutGroup("UpgradeItem")] public TMP_InputField U_itemCode;
    [FoldoutGroup("UpgradeItem")] public TMP_InputField U_itemName;
    [FoldoutGroup("UpgradeItem")] public TMP_Dropdown U_itemType;
    [FoldoutGroup("UpgradeItem")] public TMP_Dropdown U_itemQuality;
    [FoldoutGroup("UpgradeItem")] public TMP_InputField U_itemDescription;
    [FoldoutGroup("UpgradeItem")] public TMP_InputField U_itemRate;
    [FoldoutGroup("UpgradeItem")] public GameObject U_itemParameterPannel;
    [FoldoutGroup("UpgradeItem")] public GameObject U_itemParameter;

    [FoldoutGroup("SpriteAdder")] public GameObject spriteList;
    [FoldoutGroup("SpriteAdder")] public GameObject spriteButton;
    [FoldoutGroup("SpriteAdder")] public Sprite[] sprites;

    [FoldoutGroup("WeaponAdder")] public GameObject weaponList;
    [FoldoutGroup("WeaponAdder")] public GameObject weaponButton;
    [FoldoutGroup("WeaponAdder")] public GameObject[] weapons;

    private void Start()
    {
        loader = gameObject.GetComponent<DBLoader>();
        sprites = Resources.LoadAll<Sprite>("Sprite");
        weapons = Resources.LoadAll<GameObject>("Weapon");
        SpriteLoad();
        WeaponLoad();
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

            if (DB == 0)
            {
                E.SetActive(true); C.SetActive(false); U.SetActive(false);
            }
            else if (DB == 1)
            {
                E.SetActive(false); C.SetActive(true); U.SetActive(false);
            }
            else if (DB == 2)
            {
                E.SetActive(false); C.SetActive(false); U.SetActive(true);
            }

            ItemListSetting();
        }
    }

    public void SetDB(int db)
    {
        DB = db;
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
                equipItem = loader.EquipItemDB[Key].DeepCopy();
                currentEquipItem = loader.EquipItemDB[Key].DeepCopy();
                EquipItemSetting();
            }
            else if (DB == 1)
            {
                consumeItem = loader.ConsumeItemDB[Key].DeepCopy();
                currentConsumeItem = loader.ConsumeItemDB[Key].DeepCopy();
                ConsumeItemSetting();
            }
            else if (DB == 2)
            {
                upgradeItem = loader.UpgradeItemDB[Key].DeepCopy();
                currentUpgradeItem = loader.UpgradeItemDB[Key].DeepCopy();
                UpgradeItemSetting();
            }
        }
    }

    public void AddItem()
    {
        if (DB == 0)
        {
            if (loader.EquipItemDB.Count == 0)
                Key = 0;
            else
                Key = loader.EquipItemDB.Aggregate((x, y) => x.Key > y.Key ? x : y).Key + 1;
            EquippableItemSO newItem = new EquippableItemSO
            {
                ItemImage = defaultSprite,
                Type = ItemType.Melee,
                Quality = ItemQuality.Normal,
                DefaultParametersList = new List<ItemParameter>(),
                DefaultUpgradeResults = new List<EquippableItemSO.UpgradeResult>(),
                weapon = null
            };
            loader.EquipItemDB.Add(Key, newItem);
            equipItem = loader.EquipItemDB[Key];
            currentEquipItem = loader.EquipItemDB[Key];
            ItemListSetting();
        }
        else if (DB == 1)
        {
            if (loader.ConsumeItemDB.Count == 0)
                Key = 0;
            else
                Key = loader.ConsumeItemDB.Aggregate((x, y) => x.Key > y.Key ? x : y).Key + 1;
            EdibleItemSO newItem = new EdibleItemSO
            {
                ItemImage = defaultSprite,
                Type = ItemType.Potion,
                Quality = ItemQuality.Normal,
                DefaultParametersList = new List<ItemParameter>(),
                DefaultUpgradeResults = new List<EquippableItemSO.UpgradeResult>(),
                modifierData = new List<ModifierData>()
        };
            loader.ConsumeItemDB.Add(Key, newItem);
            consumeItem = loader.ConsumeItemDB[Key];
            currentConsumeItem = loader.ConsumeItemDB[Key];
            ItemListSetting();
        }
        else if (DB == 2)
        {
            if (loader.UpgradeItemDB.Count == 0)
                Key = 0;
            else
                Key = loader.UpgradeItemDB.Aggregate((x, y) => x.Key > y.Key ? x : y).Key + 1;
            UpgradeItemSO newItem = new UpgradeItemSO
            {
                ItemImage = defaultSprite,
                Type = ItemType.Melee,
                Quality = ItemQuality.Normal,
                DefaultParametersList = new List<ItemParameter>(),
                DefaultUpgradeResults = new List<EquippableItemSO.UpgradeResult>(),
                upgradeType = UpgradeItemSO.UpgradeType.Normal,
                upgradeRate = 0.0f
            };
            loader.UpgradeItemDB.Add(Key, newItem);
            upgradeItem = loader.UpgradeItemDB[Key];
            currentUpgradeItem = loader.UpgradeItemDB[Key];
            ItemListSetting();
        }
    }

    public void RemoveItem()
    {
        if (DB == 0)
        {
            loader.EquipItemDB.Remove(Key);
            if (loader.EquipItemDB.Count != 0)
                Key = loader.EquipItemDB.Keys.Last();
            else
                Key = -1;
        }
        else if (DB == 1)
        {
            loader.ConsumeItemDB.Remove(Key);
            if (loader.ConsumeItemDB.Count != 0)
                Key = loader.ConsumeItemDB.Keys.Last();
            else
                Key = -1;
        }
        else if (DB == 2)
        {
            loader.UpgradeItemDB.Remove(Key);
            if (loader.UpgradeItemDB.Count != 0)
                Key = loader.UpgradeItemDB.Keys.Last();
            else
                Key = -1;
        }

        ItemListSetting();
    }

    public void CancelItem()
    {
        if (DB == 0)
        {
            loader.EquipItemDB.Remove(Key);
            loader.EquipItemDB.Add(Key, currentEquipItem);
            ItemListSetting();
            ItemSetting();
        }
        else if (DB == 1)
        {
            loader.ConsumeItemDB.Remove(Key);
            loader.ConsumeItemDB.Add(Key, currentConsumeItem);
            ItemListSetting();
            ItemSetting();
        }
        else if (DB == 2)
        {
            loader.UpgradeItemDB.Remove(Key);
            loader.UpgradeItemDB.Add(Key, currentUpgradeItem);
            ItemListSetting();
            ItemSetting();
        }
    }

    public void SaveItem()
    {
        if (DB == 0)
        {
            equipItem.ID = int.Parse(E_itemCode.text);
            equipItem.Name = E_itemName.text;
            equipItem.Description = E_itemDescription.text;
            //equipItem.weapon = E_weapon;
            equipItem.Type = (ItemType)(E_itemType.value + 3);
            equipItem.Quality = (ItemQuality)(E_itemQuality.value + 1);
            for (int i = 0; i < equipItem.DefaultParametersList.Count; i++)
            {
                equipItem.DefaultParametersList[i] = equipItem.DefaultParametersList[i].SetParameterValue(E_itemParameterPannel.transform.GetChild(i).GetComponent<DefaultParameter>().value);
            }

            Key = equipItem.ID;
            loader.EquipItemDB.Remove(Key);
            loader.EquipItemDB.Add(Key, equipItem);
            ItemListSetting();
            ItemSetting();
        }
        else if (DB == 1)
        {
            consumeItem.ID = int.Parse(C_itemCode.text);
            consumeItem.Name = C_itemName.text;
            consumeItem.Description = C_itemDescription.text;
            consumeItem.Type = (ItemType)(C_itemType.value + 1);
            consumeItem.Quality = (ItemQuality)(C_itemQuality.value + 1);
            consumeItem.coolTime = float.Parse(C_itemCooltime.text);
            for (int i = 0; i < consumeItem.modifierData.Count; i++)    //??????, Defalut Modifier?? ????
            {
                if (consumeItem.modifierData[i].buff != null)
                {
                    consumeItem.modifierData[i] = new ModifierData
                    {
                        statModifier = new BuffModifier(),
                        buff = consumeItem.modifierData[i].buff,
                        value = 0
                    };
                }
                else
                {
                    consumeItem.modifierData[i] = new ModifierData
                    {
                        statModifier = consumeItem.modifierData[i].statModifier,
                        buff = null,
                        value = int.Parse(C_itemModifierPannel.transform.GetChild(i).GetComponent<Modifier>().value.text)
                    };
                }
            }

            Key = consumeItem.ID;
            loader.ConsumeItemDB.Remove(Key);
            loader.ConsumeItemDB.Add(Key, consumeItem);
            ItemListSetting();
            ItemSetting();
        }
        else if (DB == 2)
        {
            upgradeItem.ID = int.Parse(U_itemCode.text);
            upgradeItem.Name = U_itemName.text;
            upgradeItem.Description = U_itemDescription.text;
            upgradeItem.Type = (ItemType)(U_itemType.value + 7);
            upgradeItem.Quality = (ItemQuality)(U_itemQuality.value + 1);
            upgradeItem.upgradeType = (UpgradeItemSO.UpgradeType)(U_itemType.value + 1);
            upgradeItem.upgradeRate = float.Parse(U_itemRate.text);
            for (int i = 0; i < upgradeItem.DefaultParametersList.Count; i++)
            {
                upgradeItem.DefaultParametersList[i] = upgradeItem.DefaultParametersList[i].SetParameterValue(U_itemParameterPannel.transform.GetChild(i).GetComponent<DefaultParameter>().value);
            }

            Key = upgradeItem.ID;
            loader.UpgradeItemDB.Remove(Key);
            loader.UpgradeItemDB.Add(Key, upgradeItem);
            ItemListSetting();
            ItemSetting();
        }
    }

    public void SortItem()
    {
        if (DB == 0)
        {
            loader.EquipItemDB = loader.EquipItemDB.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            ItemListSetting();
        }
        else if (DB == 1)
        {
            loader.ConsumeItemDB = loader.ConsumeItemDB.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            ItemListSetting();
        }
        else if (DB == 2)
        {
            loader.UpgradeItemDB = loader.UpgradeItemDB.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            ItemListSetting();
        }
    }

    public void EquipItemSetting()
    {
        for (int i = 0; i < E_itemParameterPannel.transform.childCount; i++)
        {
            Destroy(E_itemParameterPannel.transform.GetChild(E_itemParameterPannel.transform.childCount - 1 - i).gameObject);
        }
        E_itemImage.sprite = equipItem.ItemImage;
        E_itemCode.text = Key.ToString();
        E_itemCode.placeholder.GetComponent<TMP_Text>().text = Key.ToString();

        if (equipItem.Name == null)
        {
            E_itemName.text = "?? ??????";
            E_itemName.placeholder.GetComponent<TMP_Text>().text = "????";
        }
        else
        {
            E_itemName.text = equipItem.Name;
            E_itemName.placeholder.GetComponent<TMP_Text>().text = equipItem.Name;
        }

        if (equipItem.weapon == null)
        {
            E_weapon = equipItem.weapon;
            E_itemWeapon.text = "null";
        }
        else
        {
            E_weapon = equipItem.weapon;
            E_itemWeapon.text = equipItem.weapon.name.ToString();
        }

        E_itemType.value = (int)equipItem.Type - 3;
        E_itemQuality.value = (int)equipItem.Quality - 1;

        if (equipItem.Description == null)
        {
            E_itemDescription.text = "";
            E_itemDescription.placeholder.GetComponent<TMP_Text>().text = "?????? ????";
        }
        else
        {
            E_itemDescription.text = equipItem.Description;
            E_itemDescription.placeholder.GetComponent<TMP_Text>().text = equipItem.Description;
        }
        for (int i = 0; i < equipItem.DefaultParametersList.Count; i++)
        {
            GameObject item = Instantiate(E_itemParameter);
            item.transform.SetParent(E_itemParameterPannel.transform);
            item.GetComponent<DefaultParameter>().SetParameter(equipItem.DefaultParametersList[i].itemParameter, equipItem.DefaultParametersList[i].value);
        }
        ItemListSetting();
    }

    private void ConsumeItemSetting()
    {
        for (int i = 0; i < C_itemModifierPannel.transform.childCount; i++)
        {
            Destroy(C_itemModifierPannel.transform.GetChild(C_itemModifierPannel.transform.childCount - 1 - i).gameObject);
        }
        C_itemImage.sprite = consumeItem.ItemImage;
        C_itemCode.text = Key.ToString();
        C_itemCode.placeholder.GetComponent<TMP_Text>().text = Key.ToString();

        if (consumeItem.Name == null)
        {
            C_itemName.text = "?? ??????";
            C_itemName.placeholder.GetComponent<TMP_Text>().text = "????";
        }
        else
        {
            C_itemName.text = consumeItem.Name;
            C_itemName.placeholder.GetComponent<TMP_Text>().text = consumeItem.Name;
        }

        C_itemType.value = (int)consumeItem.Type - 1;
        C_itemQuality.value = (int)consumeItem.Quality - 1;

        if (consumeItem.Description == null)
        {
            C_itemDescription.text = "";
            C_itemDescription.placeholder.GetComponent<TMP_Text>().text = "?????? ????";
        }
        else
        {
            C_itemDescription.text = consumeItem.Description;
            C_itemDescription.placeholder.GetComponent<TMP_Text>().text = consumeItem.Description;
        }

        C_itemCooltime.text = consumeItem.coolTime.ToString();

        for (int i = 0; i < consumeItem.modifierData.Count; i++)
        {
            if (consumeItem.modifierData[i].buff != null)
            {
                GameObject item = Instantiate(buffSlot, Vector2.zero, Quaternion.identity);
                item.transform.SetParent(C_itemModifierPannel.transform);
                item.GetComponent<BuffSlot>().SetName(consumeItem.modifierData[i].buff.Name, consumeItem.modifierData[i].buff);
            }
            else
            {
                GameObject item = Instantiate(statSlot, Vector2.zero, Quaternion.identity);
                string Name = "";
                if (consumeItem.modifierData[i].statModifier.GetType() == typeof(HealthModifier))
                    Name = "???? ????";
                else if (consumeItem.modifierData[i].statModifier.GetType() == typeof(ManaModifier))
                    Name = "???? ????";
                else if (consumeItem.modifierData[i].statModifier.GetType() == typeof(StaminaModifier))
                    Name = "???? ????";
                item.transform.SetParent(C_itemModifierPannel.transform);
                item.GetComponent<Modifier>().SetModifier(Name, consumeItem.modifierData[i].value);
            }
        }
        ItemListSetting();
    }

    public void UpgradeItemSetting()
    {
        for (int i = 0; i < U_itemParameterPannel.transform.childCount; i++)
        {
            Destroy(U_itemParameterPannel.transform.GetChild(U_itemParameterPannel.transform.childCount - 1 - i).gameObject);
        }
        U_itemImage.sprite = upgradeItem.ItemImage;
        U_itemCode.text = Key.ToString();
        U_itemCode.placeholder.GetComponent<TMP_Text>().text = Key.ToString();

        if (upgradeItem.Name == null)
        {
            U_itemName.text = "?? ??????";
            U_itemName.placeholder.GetComponent<TMP_Text>().text = "????";
        }
        else
        {
            U_itemName.text = upgradeItem.Name;
            U_itemName.placeholder.GetComponent<TMP_Text>().text = upgradeItem.Name;
        }

        U_itemType.value = (int)upgradeItem.Type - 7;
        U_itemQuality.value = (int)upgradeItem.Quality - 1;

        if (upgradeItem.Description == null)
        {
            U_itemDescription.text = "";
            U_itemDescription.placeholder.GetComponent<TMP_Text>().text = "?????? ????";
        }
        else
        {
            U_itemDescription.text = upgradeItem.Description;
            U_itemDescription.placeholder.GetComponent<TMP_Text>().text = upgradeItem.Description;
        }
        for (int i = 0; i < upgradeItem.DefaultParametersList.Count; i++)
        {
            GameObject item = Instantiate(U_itemParameter);
            item.transform.SetParent(U_itemParameterPannel.transform);
            item.GetComponent<DefaultParameter>().SetParameter(upgradeItem.DefaultParametersList[i].itemParameter, upgradeItem.DefaultParametersList[i].value);
        }
        U_itemRate.text = upgradeItem.upgradeRate.ToString();

        ItemListSetting();
    }

    public void ParameterController(ItemParameterSO parameter)
    {
        if (DB == 0)
        {
            int thereIsParameter = -1;
            for (int i = 0; i < equipItem.DefaultParametersList.Count; i++)
            {
                if (equipItem.DefaultParametersList[i].itemParameter.ParameterCode == parameter.ParameterCode)
                    thereIsParameter = i;
            }

            if (thereIsParameter != -1)                                         //?????????? ???? ??????,
            {
                equipItem.DefaultParametersList.RemoveAt(thereIsParameter);
            }
            else                                                                //?????????? ??????,
            {
                equipItem.DefaultParametersList.Add(new ItemParameter { itemParameter = parameter, value = 0 });
            }
            //???????? ?????? ????
            for (int i = 0; i < E_itemParameterPannel.transform.childCount; i++)
            {
                Destroy(E_itemParameterPannel.transform.GetChild(E_itemParameterPannel.transform.childCount - 1 - i).gameObject);
            }
            //???????? ?????? ????
            for (int i = 0; i < equipItem.DefaultParametersList.Count; i++)
            {
                GameObject item = Instantiate(E_itemParameter);
                item.transform.SetParent(E_itemParameterPannel.transform);
                item.GetComponent<DefaultParameter>().SetParameter(equipItem.DefaultParametersList[i].itemParameter, equipItem.DefaultParametersList[i].value);
            }
        }
        else if (DB == 2)
        {
            int thereIsParameter = -1;
            for (int i = 0; i < upgradeItem.DefaultParametersList.Count; i++)
            {
                if (upgradeItem.DefaultParametersList[i].itemParameter.ParameterCode == parameter.ParameterCode)
                    thereIsParameter = i;
            }

            if (thereIsParameter != -1)                                         //?????????? ???? ??????,
            {
                upgradeItem.DefaultParametersList.RemoveAt(thereIsParameter);
            }
            else                                                                //?????????? ??????,
            {
                upgradeItem.DefaultParametersList.Add(new ItemParameter { itemParameter = parameter, value = 0 });
            }
            //???????? ?????? ????
            for (int i = 0; i < U_itemParameterPannel.transform.childCount; i++)
            {
                Destroy(U_itemParameterPannel.transform.GetChild(U_itemParameterPannel.transform.childCount - 1 - i).gameObject);
            }
            //???????? ?????? ????
            for (int i = 0; i < upgradeItem.DefaultParametersList.Count; i++)
            {
                GameObject item = Instantiate(U_itemParameter);
                item.transform.SetParent(U_itemParameterPannel.transform);
                item.GetComponent<DefaultParameter>().SetParameter(upgradeItem.DefaultParametersList[i].itemParameter, upgradeItem.DefaultParametersList[i].value);
            }
        }
    }

    public void ChangeItemCode()
    {
        if (DB == 0)
        {
            equipItem = loader.EquipItemDB[Key];
        }
        else if (DB == 1)
        {
            consumeItem = loader.ConsumeItemDB[Key];
        }
        else if (DB == 2)
        {
            upgradeItem = loader.UpgradeItemDB[Key];
        }
    }

    public void BuffAdd(BuffSO buff)
    {
        int isThereBuff = -1;

        for (int i = 0; i < consumeItem.modifierData.Count; i++)
        {
            if (consumeItem.modifierData[i].buff != null)
            {
                if (consumeItem.modifierData[i].buff.BuffCode == buff.BuffCode)
                    isThereBuff = i;
            }
        }

        if (isThereBuff != -1)
        {
            consumeItem.modifierData.RemoveAt(isThereBuff);
        }
        else
        {
            consumeItem.modifierData.Add(new ModifierData
            {
                statModifier = new BuffModifier(),
                buff = buff,
                value = 0
            });
        }

        ModifierLoad();
    }

    public void HealthAdd()
    {
        int index = -1;

        for (int i = 0; i < consumeItem.modifierData.Count; i++)
        {
            if (consumeItem.modifierData[i].statModifier.GetType() == typeof(HealthModifier))
            {
                index = i;
            }
        }

        if (index != -1)
        {
            consumeItem.modifierData.RemoveAt(index);
        }
        else
        {
            consumeItem.modifierData.Add(new ModifierData
            {
                statModifier = new HealthModifier(),
                buff = null,
                value = 0
            });
        }

        ModifierLoad();
    }


    public void ManaAdd()
    {
        int index = -1;

        for (int i = 0; i < consumeItem.modifierData.Count; i++)
        {
            if (consumeItem.modifierData[i].statModifier.GetType() == typeof(ManaModifier))
            {
                index = i;
            }
        }

        if (index != -1)
        {
            consumeItem.modifierData.RemoveAt(index);
        }
        else
        {
            consumeItem.modifierData.Add(new ModifierData
            {
                statModifier = new ManaModifier(),
                buff = null,
                value = 0
            });
        }

        ModifierLoad();
    }

    public void StaminaAdd()
    {
        int index = -1;

        for (int i = 0; i < consumeItem.modifierData.Count; i++)
        {
            if (consumeItem.modifierData[i].statModifier.GetType() == typeof(StaminaModifier))
            {
                index = i;
            }
        }

        if (index != -1)
        {
            consumeItem.modifierData.RemoveAt(index);
        }
        else
        {
            consumeItem.modifierData.Add(new ModifierData
            {
                statModifier = new StaminaModifier(),
                buff = null,
                value = 0
            });
        }

        ModifierLoad();
    }

    public void ModifierLoad()
    {
        for (int i = 0; i < C_itemModifierPannel.transform.childCount; i++)
        {
            Destroy(C_itemModifierPannel.transform.GetChild(C_itemModifierPannel.transform.childCount - 1 - i).gameObject);
        }
        for (int i = 0; i < consumeItem.modifierData.Count; i++)
        {
            if (consumeItem.modifierData[i].buff != null)
            {
                GameObject item = Instantiate(buffSlot, Vector2.zero, Quaternion.identity);
                item.transform.SetParent(C_itemModifierPannel.transform);
                item.GetComponent<BuffSlot>().SetName(consumeItem.modifierData[i].buff.Name, consumeItem.modifierData[i].buff);
            }
            else
            {
                GameObject item = Instantiate(statSlot, Vector2.zero, Quaternion.identity);
                string Name = "";
                if (consumeItem.modifierData[i].statModifier.GetType() == typeof(HealthModifier))
                    Name = "???? ????";
                else if (consumeItem.modifierData[i].statModifier.GetType() == typeof(ManaModifier))
                    Name = "???? ????";
                else if (consumeItem.modifierData[i].statModifier.GetType() == typeof(StaminaModifier))
                    Name = "???? ????";
                item.transform.SetParent(C_itemModifierPannel.transform);
                item.GetComponent<Modifier>().SetModifier(Name, consumeItem.modifierData[i].value);
            }
        }
    }

    public void SpriteLoad()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject sprite = Instantiate(spriteButton);
            sprite.transform.SetParent(spriteList.transform);
            sprite.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = sprites[i];
            sprite.transform.GetChild(0).GetComponent<SpriteAdder>().sprite = sprites[i];
        }
    }

    public void WeaponLoad()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            GameObject weapon = Instantiate(weaponButton);
            weapon.transform.SetParent(weaponList.transform);
            weapon.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = weapons[i].transform.GetComponent<SpriteRenderer>().sprite;
            weapon.transform.GetChild(0).GetComponent<WeaponAdder>().weapon = weapons[i];
            weapon.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = weapons[i].name;
        }
    }
}
