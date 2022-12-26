using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GetDamageBuff", menuName = "Buff/GetDamageBuff", order = 1)]
public class DamageBuffSO : BuffSO
{
    public SkillDamage skillDamage;

    public override void AffectTarget(GameObject Main, GameObject Target)
    {
            DamageCalculator.DmgCalculator.CauseDamage(Main, Target, skillDamage);
    }
}