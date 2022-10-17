using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Weapon/Weapon Data", order = 1)]
public class Weapon : ScriptableObject
{  
    [System.Serializable]
    public class WeaponClass
    {
        public enum WeaponType
        {
            None,
            Melee,
            Magic,
            Range
        }
        public enum QualityName
        {
            None,
            Normal,
            Rare,
            Epic,
            Unique,
            Legendary
        }
        public enum TypeName
        {
            None,
            Sword,
            Club,
            Wand,
            Staff,
            Bow,
            Crossbow
        }

        [SerializeField] protected WeaponType weapon;
        public WeaponType Weapon { get { return weapon; } }

        [SerializeField] protected int id;
        public int ID { get { return id; } }

        [SerializeField] protected GameObject itemObject;
        public GameObject ItemObject { get { return itemObject; } }

        [SerializeField] protected Sprite icon;
        public Sprite Icon { get { return icon; } }

        [SerializeField] protected string name;
        public string Name { get { return name; } }

        [SerializeField] protected TypeName type;
        public TypeName Type { get { return type; } }

        [SerializeField] protected QualityName quality;
        public QualityName Quality { get { return quality; } }

        [SerializeField] protected string description;
        public string Description { get { return description; } }

        [SerializeField] protected int minDmg;
        public int MinDmg { get { return minDmg; } }

        [SerializeField] private int maxDmg;
        public int MaxDmg { get { return maxDmg; } }

        [SerializeField] protected int Str;
        public int STR { get { return Str; } }

        [SerializeField] protected int Int;
        public int INT { get { return Int; } }

        [SerializeField] protected int Dex;
        public int DEX { get { return Dex; } }

        public virtual void PrintInfo()
        {
            Debug.Log("==========");
            Debug.Log("Weapon Type : " + weapon);
            Debug.Log("ID : " + id);
            Debug.Log("Name : " + name);
            Debug.Log("Type : " + type);
            Debug.Log("Quality : " + quality);
            Debug.Log("Description : " + description);
            Debug.Log("DMG : " + minDmg + "~" + maxDmg);
            Debug.Log("STR : " + Str + " INT : " + Int + " DEX : " + Dex);
        }
    }
}