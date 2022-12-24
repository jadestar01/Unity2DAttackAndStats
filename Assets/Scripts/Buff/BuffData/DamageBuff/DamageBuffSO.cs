using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using dmg;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GetDamageBuff", menuName = "Buff/GetDamageBuff", order = 1)]
public class DamageBuffSO : BuffSO
{
    [ShowInInspector] public DamageCalculator damageCalculator;
    [ShowInInspector] public int physicalDmg;
    [ShowInInspector] public int magicalDmg;

    private void Awake()
    {
        damageCalculator = GameObject.Find("DamageManager").GetComponent<DamageCalculator>();
    }

    public override void AffectTarget(GameObject Target)
    {
        if (Target.gameObject.tag == "Player")
        {
            Target.GetComponent<Stats>().curHealth -= damageCalculator.DmgCal(null, Target, new SkillDamage { physicalDmg = physicalDmg, magicalDmg = magicalDmg }).totalDamage;
        }
        else
        {
            Target.GetComponent<MobController>().curHealth -= damageCalculator.DmgCal(null, Target, new SkillDamage { physicalDmg = physicalDmg, magicalDmg = magicalDmg }).totalDamage;
        }
    }
}
