using BansheeGz.BGDatabase;
using Inventory.Model;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEditor.Progress;

public class SaveDB : MonoBehaviour
{
    //Dictionary를 뜯어서 Save하는 기능
    public DBLoader DB;

    public void DBSave()
    {
        Debug.Log("Save the current DataBase");
        ResetDataBase();
        SaveDataBase();
        //byte[] bytes = BGRepo.I.Addons.Get<BGAddonSaveLoad>().Save();         //데이터를 bytes로 뽑아오는 것 버전이나 저장 위치 선정 가능.
    }

    public void ResetDataBase()
    {
        List<BGEntity> equipItems = BGRepo.I["EquipItem"].EntitiesToList();
        for (int i = 0; i < equipItems.Count; i++)
        {
            equipItems[i].Delete();
        }
        List<BGEntity> consumeItems = BGRepo.I["ConsumeItem"].EntitiesToList();
        for (int i = 0; i < consumeItems.Count; i++)
        {
            consumeItems[i].Delete();
        }
        List<BGEntity> upgradeItems = BGRepo.I["UpgradeItem"].EntitiesToList();
        for (int i = 0; i < upgradeItems.Count; i++)
        {
            upgradeItems[i].Delete();
        }
    }

    public void SaveDataBase()
    {
        foreach (KeyValuePair<int, EquippableItemSO> item in DB.EquipItemDB)
        {
            EquipItemParsing(item.Value);
        }

        foreach (KeyValuePair<int, EdibleItemSO> item in DB.ConsumeItemDB)
        {
            ConsumeItemParsing(item.Value);
        }

        foreach (KeyValuePair<int, UpgradeItemSO> item in DB.UpgradeItemDB)
        {
            UpgradeItemParsing(item.Value);
        }
    }

    public void EquipItemParsing(EquippableItemSO item)
    {
        DB_EquipItem newItem = DB_EquipItem.NewEntity();
        newItem.f_ID = item.ID;
        newItem.f_Image = item.ItemImage.name;
        newItem.f_Name = item.Name;
        newItem.f_Type = item.Type;
        newItem.f_Quality = item.Quality;
        newItem.f_Description = item.Description;
        for (int i = 0; i < item.DefaultParametersList.Count; i++)
        {
            DB_Parameters newParameter = DB_Parameters.NewEntity(newItem);
            newParameter.f_Parameter = item.DefaultParametersList[i].itemParameter.name;
            newParameter.f_Value = item.DefaultParametersList[i].value;
        }
        newItem.f_Weapon = item.weapon.name;
    }

    public void ConsumeItemParsing(EdibleItemSO item)
    {
        DB_ConsumeItem newItem = DB_ConsumeItem.NewEntity();
        newItem.f_ID = item.ID;
        newItem.f_Image = item.ItemImage.name;
        newItem.f_Name = item.Name;
        newItem.f_Type = item.Type;
        newItem.f_Quality = item.Quality;
        newItem.f_Description = item.Description;
        for (int i = 0; i < item.modifierData.Count; i++)
        {
            if (item.modifierData[i].buff != null)          //버프 데이터라면,
            {
                DB_BuffModifier newBuff = DB_BuffModifier.NewEntity(newItem);
                newBuff.f_Buff = item.modifierData[i].buff.name;
            }
            else
            {
                DB_Modifiers newModifier = DB_Modifiers.NewEntity(newItem);
                newModifier.f_Modifier = item.modifierData[i].statModifier.name;
                newModifier.f_Value = item.modifierData[i].value;
            }
        }
        newItem.f_Cooltime = item.coolTime;
    }

    public void UpgradeItemParsing(UpgradeItemSO item)
    {
        DB_UpgradeItem newItem = DB_UpgradeItem.NewEntity();
        newItem.f_ID = item.ID;
        newItem.f_Image = item.ItemImage.name;
        newItem.f_Name = item.Name;
        newItem.f_Type = item.Type;
        newItem.f_Quality = item.Quality;
        newItem.f_Description = item.Description;
        for (int i = 0; i < item.DefaultParametersList.Count; i++)
        {
            DB_UParameters newParameter = DB_UParameters.NewEntity(newItem);
            newParameter.f_Parameter = item.DefaultParametersList[i].itemParameter.name;
            newParameter.f_Value = item.DefaultParametersList[i].value;
        }
        newItem.f_UpgradeRate = item.upgradeRate;
        newItem.f_UpgradeType = item.upgradeType;
    }
}
