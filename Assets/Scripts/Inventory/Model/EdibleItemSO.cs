using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        //���� ȿ���� ��� ����Ʈ
        [SerializeField] List<ModifierData> modifierData = new List<ModifierData>();
        //�ϴ�, interface�� ���� ����
        public string ActionName => "Consume";
        [field: SerializeField] public AudioClip actionSFX {get; private set;}

        public bool PerformAction(GameObject character, List<ItemParameter> ltemState = null)
        {
            foreach (ModifierData data in modifierData)
            {
                //������̾� ȿ�� ����
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
        //�÷��̾ ���������ν� ���� �� �ִ� ȿ��
        public CharacterStatModifierSO statModifier;
        public float value;
    }
}