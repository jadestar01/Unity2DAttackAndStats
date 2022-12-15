using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Inventory.Model.EquippableItemSO;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [field: SerializeField] public float coolTime;
        public bool canUse = true;
        //얻을 효과를 담는 리스트
        [SerializeField] public List<ModifierData> modifierData = new List<ModifierData>();
        //하단, interface에 대한 구조
        public string ActionName => "Consume";
        [field: SerializeField] public AudioClip actionSFX {get; set;}

        public bool PerformAction(GameObject character, List<ItemParameter> ltemState = null)
        {
            foreach (ModifierData data in modifierData)
            {
                //모디파이어 효과 적용
                data.statModifier.AffectCharacter(character, data.value, data.buff);
            }
            return true;
        }

        public EdibleItemSO DeepCopy()
        {
            EdibleItemSO item = new EdibleItemSO();
            item.InStackable = InStackable;
            item.MaxStackSize = MaxStackSize;
            item.ItemImage = ItemImage;
            item.ID = ID;
            item.Name = Name;
            item.Type = Type;
            item.Quality = Quality;
            item.Description = Description;
            item.DefaultParametersList = new List<ItemParameter>();
            for (int i = 0; i < DefaultParametersList.Count; i++)
            {
                item.DefaultParametersList.Add(DefaultParametersList[i]);
            }
            item.DefaultUpgradeResults = new List<UpgradeResult>();
            for (int i = 0; i < DefaultUpgradeResults.Count; i++)
            {
                item.DefaultUpgradeResults.Add(DefaultUpgradeResults[i]);
            }
            item.actionSFX = actionSFX;
            item.modifierData = new List<ModifierData>();
            for (int i = 0; i < modifierData.Count; i++)
            {
                item.modifierData.Add(new ModifierData
                {
                    statModifier = modifierData[i].statModifier,
                    buff = modifierData[i].buff,
                    value = modifierData[i].value
                });
            }
            item.coolTime = coolTime;
            item.canUse = canUse;

            return item;
        }
    }

    public interface IDestroyableItem
    {
        
    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character, List<ItemParameter> ltemState);
    }

    [Serializable]
    public class ModifierData
    {
        //플레이어가 섭취함으로써 얻을 수 있는 효과
        public CharacterStatModifierSO statModifier;
        public BuffSO buff;
        public float value;

        public ModifierData Copy()
        {
            ModifierData newItem = new ModifierData
            {
                statModifier = this.statModifier,
                buff = this.buff,
                value = this.value
            };

            return newItem;
        }
    }
}