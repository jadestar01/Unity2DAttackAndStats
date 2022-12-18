using BansheeGz.BGDatabase;
using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SaveDB : MonoBehaviour
{
    //Dictionary를 뜯어서 Save하는 기능
    public DBLoader DB;
    public DB_EquipItem a;

    void Start()
    {
        SaveDataBase();
        //gameObject.GetComponent<BGDataBinderRowGo>().Entity.Delete();
        //gameObject.GetComponent<BGDataBinderRowGo>().Entity;
        var item = gameObject.GetComponent<BGDataBinderDatabaseGo>();
    }

    /*
    public void Init()
    {
        var databaseData = Addressables.LoadAssetAsync<TextAsset>(BGLoaderForRepoCustom.CustomDatabaseGuid).WaitForCompletion().bytes;
        //수동으로 콘텐츠를 로드한다.
        BGRepo.SetDefaultRepoContent(databaseData);
        BGRepo.Load();

        DB_EquipItem.NewEntity();
    }
    */

    public void SaveDataBase()
    {
        SaveEquipItem();
    }

    public void SaveEquipItem()
    {
        Dictionary<int, EquippableItemSO> EquipItems = new Dictionary<int, EquippableItemSO>();
        EquipItems = DB.EquipItemDB;

        foreach (KeyValuePair<int, EquippableItemSO> item in EquipItems)
        {
            EquipItemParsing(item.Value);
        }
    }

    public void EquipItemParsing(EquippableItemSO item)
    {
        DB_EquipItem newItem = DB_EquipItem.NewEntity();
        newItem.f_ID = item.ID;
        //newItem.f_Image.SetSpriteID(item.ItemImage.GetSpriteID());
        newItem.f_Name = item.Name;
        newItem.f_Type = item.Type;
        newItem.f_Quality = item.Quality;
        newItem.f_Description = item.Description;
        //newItem.f_Image.GetComponent<BGFieldUnitySprite>().SetAssetPath(newItem.Index,"Sprite/" + name);
        //newItem.f_Parameters = item.DefaultParametersList;
        //newItem.f_Weapon = item.weapon;
    }

    public void SaveConsumeItem()
    {

    }

    public void SaveUpgradeItem()
    {

    }
}
