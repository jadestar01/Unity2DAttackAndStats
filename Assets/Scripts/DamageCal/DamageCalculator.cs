using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    [SerializeField] private Stats playerStats;
    public float initPhysicalCritDmg = 1.5f;
    public float initMagicalCritDmg = 1.5f;
    Damage DmgCal(float additionalPhysicalDmg, float physicalDmgCoeffiecient, float additionalMagicalDmg, float magicalDmgCoeffiecient)
    {
        float physicalDmg = 0;
        bool isPhysicalCrit = false;
        if (playerStats.physicalMinDmg != 0 && playerStats.physicalMaxDmg != 0)
        {
            physicalDmg = Random.Range(playerStats.physicalMinDmg, playerStats.physicalMaxDmg) * (physicalDmg/100) + additionalPhysicalDmg;
            float physicalCritRate = Random.Range(0, 100);
            if (physicalCritRate <= playerStats.physicalCritRate)
            {
                physicalDmg *= (initPhysicalCritDmg + (playerStats.physicalCritDmg / 100));
                isPhysicalCrit = true;
            }
            else
            {
            }
        }

        float magicalDmg = 0;
        bool isMagicalCrit = false;
        if (playerStats.magicalMinDmg != 0 && playerStats.magicalMaxDmg != 0)
        {
            magicalDmg = Random.Range(playerStats.magicalMinDmg, playerStats.magicalMaxDmg) * (magicalDmg / 100) + additionalMagicalDmg;
            float magicalCritRate = Random.Range(0, 100);
            if (magicalCritRate <= playerStats.magicalCritRate)
            {
                magicalDmg *= (initMagicalCritDmg + (playerStats.magicalCritDmg / 100));
                isMagicalCrit = true;
            }
            else
            {
            }
        }

        return new Damage { PhysicalDmg = physicalDmg, isPhysicalCrit = isPhysicalCrit, MagicalDmg = magicalDmg, isMagicalCrit = isMagicalCrit };
    }

    public struct Damage
    {
        public float PhysicalDmg;
        public bool isPhysicalCrit;
        public float MagicalDmg;
        public bool isMagicalCrit;
    }
}
