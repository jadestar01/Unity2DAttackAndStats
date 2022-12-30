using BansheeGz.BGDatabase;
using Inventory.Model;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DBLoader : MonoBehaviour
{
    [ShowInInspector] public Dictionary<int, EquippableItemSO> EquipItemDB = new Dictionary<int, EquippableItemSO>();
    [ShowInInspector] public Dictionary<int, EdibleItemSO> ConsumeItemDB = new Dictionary<int, EdibleItemSO>();
    [ShowInInspector] public Dictionary<int, UpgradeItemSO> UpgradeItemDB = new Dictionary<int, UpgradeItemSO>();
    void Awake()
    {
        Init();
        EqiupItemParsing();
        ConsumeItemParsing();
        UpgradeItemParsing();
    }

    private void Init()
    {
        //Addressable에서 Data를 받아와 Byte[] 변수에 할당한다.
        var databaseData = Addressables.LoadAssetAsync<TextAsset>(BGLoaderForRepoCustom.CustomDatabaseGuid).WaitForCompletion().bytes;
        //수동으로 콘텐츠를 로드한다.
        BGRepo.SetDefaultRepoContent(databaseData);
        BGRepo.Load();
    }

    private void EqiupItemParsing()
    {
        Debug.Log("Start EquipItemParsing");

        List<BGEntity> entities = BGRepo.I["EquipItem"].EntitiesToList();
        for (int i = 0; i < entities.Count; i++)
        {
            List<ItemParameter> itemParameters = new List<ItemParameter>();
            List<BGEntity> parameters = entities[i].Get<List<BGEntity>>("Parameters");
            for (int j = 0; j < parameters.Count; j++)
            {
                ItemParameter itemParameter = new ItemParameter
                {
                    itemParameter = Resources.Load<ItemParameterSO>("Parameters/" + parameters[j].Get<string>("Parameter")),
                    value = parameters[j].Get<float>("Value")
                };
                itemParameters.Add(itemParameter);
            }

            EquippableItemSO item = new EquippableItemSO
            {
                ID = entities[i].Get<int>("ID"),
                ItemImage = Resources.Load<Sprite>("Sprite/" + entities[i].Get<string>("Image")),
                Name = entities[i].Get<string>("Name"),
                Type = DB_EquipItem.GetEntity(i).f_Type,
                Quality = DB_EquipItem.GetEntity(i).f_Quality,
                Description = entities[i].Get<string>("Description"),
                DefaultParametersList = itemParameters,
                weapon = Resources.Load<GameObject>("Weapon/" + entities[i].Get<string>("Weapon")),
                InStackable = false,
                MaxStackSize = 1,
                DefaultUpgradeResults = new List<EquippableItemSO.UpgradeResult>(),
                actionSFX = null
            };
            EquipItemDB.Add(entities[i].Get<int>("ID"), item);
        }
    }

    private void ConsumeItemParsing()
    {
        Debug.Log("Start ConsumeItemParsing");

        List<BGEntity> entities = BGRepo.I["ConsumeItem"].EntitiesToList();
        for (int i = 0; i < entities.Count; i++)
        {
            List<ModifierData> itemModifiers = new List<ModifierData>();
            List<BGEntity> modifiers = entities[i].Get<List<BGEntity>>("Modifiers");
            for (int j = 0; j < modifiers.Count; j++)
            {
                ModifierData modifier = new ModifierData
                {
                    statModifier = Resources.Load<CharacterStatModifierSO>("Modifiers/" + modifiers[j].Get<string>("Modifier")),
                    buff = null,
                    value = modifiers[j].Get<int>("Value")
                };
                itemModifiers.Add(modifier);
            }
            List<BGEntity> buffs = entities[i].Get<List<BGEntity>>("BuffModifier");
            for (int k = 0; k < buffs.Count; k++)
            {
                ModifierData modifier = new ModifierData
                {
                    statModifier = new BuffModifier(),
                    buff = Resources.Load<BuffSO>("Buffs/" + buffs[k].Get<string>("Buff")),
                    value = 0
                };
                itemModifiers.Add(modifier);
            }

            EdibleItemSO item = new EdibleItemSO
            {
                ID = entities[i].Get<int>("ID"),
                ItemImage = Resources.Load<Sprite>("Sprite/" + entities[i].Get<string>("Image")),
                Name = entities[i].Get<string>("Name"),
                Type = DB_ConsumeItem.GetEntity(i).f_Type,
                Quality = DB_ConsumeItem.GetEntity(i).f_Quality,
                Description = entities[i].Get<string>("Description"),
                DefaultParametersList = new List<ItemParameter>(),
                modifierData = itemModifiers,
                coolTime = entities[i].Get<float>("Cooltime"),
                InStackable = true,
                MaxStackSize = 30,
                DefaultUpgradeResults = new List<EquippableItemSO.UpgradeResult>(),
                actionSFX = null
            };
            ConsumeItemDB.Add(entities[i].Get<int>("ID"), item);
        }
    }

    private void UpgradeItemParsing()
    {
        Debug.Log("Start UpgradeItemParsing");

        List<BGEntity> entities = BGRepo.I["UpgradeItem"].EntitiesToList();
        for (int i = 0; i < entities.Count; i++)
        {
            List<ItemParameter> itemParameters = new List<ItemParameter>();
            List<BGEntity> parameters = entities[i].Get<List<BGEntity>>("UParameters");
            for (int j = 0; j < parameters.Count; j++)
            {
                ItemParameter itemParameter = new ItemParameter
                {
                    itemParameter = Resources.Load<ItemParameterSO>("Parameters/" + parameters[j].Get<string>("Parameter")),
                    value = parameters[j].Get<float>("Value")
                };
                itemParameters.Add(itemParameter);
            }

            UpgradeItemSO item = new UpgradeItemSO
            {
                ID = entities[i].Get<int>("ID"),
                ItemImage = Resources.Load<Sprite>("Sprite/" + entities[i].Get<string>("Image")),
                Name = entities[i].Get<string>("Name"),
                Type = DB_UpgradeItem.GetEntity(i).f_Type,
                Quality = DB_UpgradeItem.GetEntity(i).f_Quality,
                Description = entities[i].Get<string>("Description"),
                DefaultParametersList = itemParameters,
                InStackable = true,
                MaxStackSize = 10,
                actionSFX = null,
                DefaultUpgradeResults = new List<EquippableItemSO.UpgradeResult>(),
                upgradeType = DB_UpgradeItem.GetEntity(i).f_UpgradeType,
                upgradeRate = entities[i].Get<float>("UpgradeRate")
            };
            UpgradeItemDB.Add(entities[i].Get<int>("ID"), item);
        }
    }
}