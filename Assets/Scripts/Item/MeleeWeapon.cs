using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Melee Weapon Data", menuName = "Weapon/Melee Weapon Data", order = 2)]
public class MeleeWeapon : ScriptableObject
{
    public List<Melee> meleeWeaponList;

    [System.Serializable]
    public class Melee : Weapon.WeaponClass
    {
        public Melee()
        { }

        //밀리변수
        [SerializeField] protected float physicalCritRate;
        public float PhysicalCritRate { get { return physicalCritRate; } }

        [SerializeField] protected float physicalCritDmg;
        public float PhysicalCritDmg { get { return physicalCritDmg; } }

        [SerializeField] protected float physicalSpeed;
        public float PhysicalSpeed { get { return physicalSpeed; } }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Debug.Log("PhysicalCritRate : " + physicalCritRate + "% PhysicalCritDmg : " + physicalCritDmg + "%");
            Debug.Log("Physical Speed : " + physicalSpeed);
        }
    }
}
