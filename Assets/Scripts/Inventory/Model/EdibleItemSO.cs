using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        //얻을 효과를 담는 리스트
        [SerializeField] List<ModifierData> modifierData = new List<ModifierData>();
        //하단, interface에 대한 구조
        public string ActionName => "Consume";
        [field: SerializeField] public AudioClip actionSFX {get; private set;}

        public bool PerformAction(GameObject character, List<ItemParameter> ltemState = null)
        {
            foreach (ModifierData data in modifierData)
            {
                //모디파이어 효과 적용
                data.statModifier.AffectCharacter(character, data.value);
            }
            return true;
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
        public float value;
    }
}