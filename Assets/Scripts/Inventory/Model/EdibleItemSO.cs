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
        //���� ȿ���� ��� ����Ʈ
        [SerializeField] public List<ModifierData> modifierData = new List<ModifierData>();
        //�ϴ�, interface�� ���� ����
        public string ActionName => "Consume";
        [field: SerializeField] public AudioClip actionSFX {get; set;}

        public bool PerformAction(GameObject character, List<ItemParameter> ltemState = null)
        {
            foreach (ModifierData data in modifierData)
            {
                //������̾� ȿ�� ����
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
        //�÷��̾ ���������ν� ���� �� �ִ� ȿ��
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