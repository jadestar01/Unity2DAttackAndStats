using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Trinket             //��ű�
        };

        public enum ItemQuality
        {
            None,
            Normal,
            Rare,
            Epic,
            Unique,
            Legendary
        };
        //������ ������ ��
        [field: SerializeField] public bool InStackable { get; set; }
        public int ID => GetInstanceID();
        [field: SerializeField] public int MaxStackSize { get; set; } = 1;
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite ItemImage { get; set; }

        [field: SerializeField] public ItemType Type = ItemType.None;
        [field: SerializeField] public ItemQuality Quality = ItemQuality.None;
        [field: SerializeField][field: TextArea] public string Description { get; set; }
        //������ �Ӽ�
        [field: SerializeField] public List<ItemParameter> DefaultParametersList { get; set; }
    }

    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public float value;

        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }
}
