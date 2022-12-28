using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GetDamageBuff", menuName = "Buff/RegenBuff", order = 3)]
public class RegenBuff : BuffSO
{ 
    [field: SerializeField] public int HPRegenValue = 0;
    [field: SerializeField] public int MPRegenValue = 0;
    [field: SerializeField] public int SPRegenValue = 0;

    public override void AffectTarget(GameObject Main, GameObject Target)
    {
        DamageCalculator.DmgCalculator.CauseHPup(Target, HPRegenValue);
        DamageCalculator.DmgCalculator.CauseMPup(Target, MPRegenValue);
        DamageCalculator.DmgCalculator.CauseSPup(Target, SPRegenValue);
    }
}
