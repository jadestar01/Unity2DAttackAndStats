using BansheeGz.BGDatabase;
using ColorPallete;
using Inventory.Model;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        List<BGEntity> entitys = BGRepo.I["EquipItem"].EntitiesToList();
        foreach (BGEntity entity in entitys)
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
    }
}