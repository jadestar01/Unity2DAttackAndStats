using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MeleeWeapon;

[CreateAssetMenu(fileName = "Magic Weapon Data", menuName = "Weapon/Magic Weapon Data", order = 3)]
public class MagicWeapon : ScriptableObject
{
    public List<Magic> magicWeaponList;

    [System.Serializable]
    public class Magic : Weapon.WeaponClass
    {
        //마법 변수
        [SerializeField] protected float magicalCritRate;
        public float MagicalCritRate { get { return magicalCritRate; } }

        [SerializeField] protected float magicalCritDmg;
        public float MagicalCritDmg { get { return magicalCritDmg; } }

        [SerializeField] protected float magicalSpeed;
        public float MagicalSpeed { get { return magicalSpeed; } }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Debug.Log("MagicalCritRate : " + magicalCritRate + "% MagicalCritDmg : " + magicalCritDmg + "%");
            Debug.Log("Magical Speed : " + magicalSpeed);
        }
    }
}
