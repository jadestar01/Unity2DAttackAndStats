using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Range Weapon Data", menuName = "Weapon/Range Weapon Data", order = 4)]
public class RangeWeapon : ScriptableObject
{
    public List<Range> rangeWeaponList;

    [System.Serializable]
    public class Range : Weapon.WeaponClass
    {
        //Range Variable
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
