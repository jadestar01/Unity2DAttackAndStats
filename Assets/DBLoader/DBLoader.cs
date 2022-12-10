using BansheeGz.BGDatabase;
using ColorPallete;
using Inventory.Model;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static BansheeGz.BGDatabase.BGJsonRepoModel;
using static Inventory.Model.ItemSO;

public class DBLoader : MonoBehaviour
{
    [ShowInInspector] Dictionary<int, EquippableItemSO> EquippableItemDB = new Dictionary<int, EquippableItemSO>();
    [ShowInInspector] Dictionary<int, EdibleItemSO> EdibleItemDB = new Dictionary<int, EdibleItemSO>();
    [ShowInInspector] Dictionary<int, UpgradeItemSO> UpgradeItemDB = new Dictionary<int, UpgradeItemSO>();
    void Awake()
    {
        Init();
        EqiupItemParsing();
    }

    private void Init()
    {
        //Addressable에서 Data를 받아와 Byte[] 변수에 할당한다.
        var databaseData = Addressables.LoadAssetAsync<TextAsset>(BGLoaderForRepoCustom.CustomDatabaseGuid).WaitForCompletion().bytes;
        //수동으로 콘텐츠를 로드한다.
        BGRepo.SetDefaultRepoContent(databaseData);
        BGRepo.Load();
        /*
        //DB에 접근한 뒤 데이터를 불러와서 쓴다.
        //Resource Loader와 동일하다.
        BGMetaEntity table = BGRepo.I["EquipItem"];            //Table(Sheet) 선택
        BGEntity entity = table.GetEntity(index);                   //행 가져오기
        Debug.Log("ID : " + entity.Get<int>("ID") + ", Name : " + entity.Get<string>("Name"));
        for (int i = 0; i < entity.Get<List<BGEntity>>("Parameters").Count; i++)
        {
            Debug.Log(entity.Get<List<BGEntity>>("Parameters")[i].Get<ScriptableObject>("Parameter"));
            Debug.Log(entity.Get<List<BGEntity>>("Parameters")[i].Get<float>("Value"));
        }
        */
    }

    private void EqiupItemParsing()
    {
        Debug.Log("Start EquipItemParsing");

        ItemType a = DB_EquipItem.GetEntity(0).f_Type;

        List<BGEntity> entities = BGRepo.I["EquipItem"].EntitiesToList();
        for (int i = 0; i < entities.Count; i++)
        {
            List<ItemParameter> itemParameters = new List<ItemParameter>();
            List<BGEntity> parameters = entities[i].Get<List<BGEntity>>("Parameters");
            for (int j = 0; j < parameters.Count; j++)
            {
                ItemParameter itemParameter = new ItemParameter
                {
                    itemParameter = (ItemParameterSO)parameters[j].Get<ScriptableObject>("Parameter"),
                    value = parameters[j].Get<float>("Value")
                };
                itemParameters.Add(itemParameter);
            }

            EquippableItemSO item = new EquippableItemSO
            {
                ID = entities[i].Get<int>("ID"),
                ItemImage = entities[i].Get<Sprite>("Image"),
                Name = entities[i].Get<string>("Name"),
                Type = DB_EquipItem.GetEntity(i).f_Type,
                Quality = DB_EquipItem.GetEntity(i).f_Quality,
                Description = entities[i].Get<string>("Description"),
                DefaultParametersList = itemParameters,
                weapon = entities[i].Get<GameObject>("Weapon"),
                InStackable = false,
                MaxStackSize = 1,
                actionSFX = null
            };
            EquippableItemDB.Add(entities[i].Get<int>("ID"), item);
        }

        /*
        foreach (BGEntity entity in entities)
        {

            List<ItemParameter> itemParameters = new List<ItemParameter>();
            List<BGEntity> parameters = entity.Get<List<BGEntity>>("Parameters");
            foreach (BGEntity parameter in parameters)
            {
                ItemParameter itemParameter = new ItemParameter
                {
                    itemParameter = (ItemParameterSO)parameter.Get<ScriptableObject>("Parameter"),
                    value = parameter.Get<float>("Value")
                };
                itemParameters.Add(itemParameter);
            }

            EquippableItemSO item = new EquippableItemSO {
                ID = entity.Get<int>("ID"),
                ItemImage = entity.Get<Sprite>("Image"),
                Name = entity.Get<string>("Name"),
                Type = entity.Get<ItemType> ("Type"),
                Quality = entity.Get<ItemQuality>("Quality"),
                Description = entity.Get<string>("Description"),
                DefaultParametersList = itemParameters,
                weapon = entity.Get<GameObject>("Weapon"),
                InStackable = false,
                MaxStackSize = 1,
                actionSFX = null
            };

            EquippableItemDB.Add(entity.Get<int>("ID"), item);
        }
        */
    }
}