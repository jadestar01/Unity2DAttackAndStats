using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorPallete;
using static Inventory.Model.EquippableItemSO;

namespace Inventory.Model
{
    public abstract class ItemSO : ScriptableObject
    {
        public enum ItemType
        { 
            None,               //�⺻����
            Potion,             //�Ҹ�ǰ
            Scroll,             //�Ҹ�ǰ
            Melee,              //��������
            Magic,              //��������
            Range,              //���Ÿ�����
            Trinket,            //��ű�
            NormalUpgrade,      //�Ϲ� ��ȭ ���
            SpecialUpgrade      //Ư�� ��ȭ ���
        };
        //������ ������ ��
        [field: SerializeField] public bool InStackable { get; set; }
        public int ID;  //=> GetInstanceID()
        [field: SerializeField] public int MaxStackSize { get; set; } = 1;
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite ItemImage { get; set; }

        [field: SerializeField] public ItemType Type = ItemType.None;
        [field: SerializeField] public ItemQuality Quality = ItemQuality.None;
        [field: SerializeField][field: TextArea] public string Description { get; set; }
        //������ �Ӽ�
        [field: SerializeField] public List<ItemParameter> DefaultParametersList { get; set; }
        [field: SerializeField] public List<UpgradeResult> DefaultUpgradeResults { get; set; }

        public int GetID()
        {
            return ID;
        }
    }

    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public float value;


        public ItemParameter AddParameterValue(float a)
        {
            return new ItemParameter
            {
                itemParameter = this.itemParameter,
                value = this.value + a
            };
        }

        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }
}
