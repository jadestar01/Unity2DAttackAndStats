using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthModifier", menuName = "Modifiers/Health", order = 0)]
public class HealthModifier : CharacterStatModifierSO
{
    public int ID = 0;
    public override void AffectCharacter(GameObject character, int val, BuffSO buff = null)
    {
        DamageCalculator.DmgCalculator.CauseHPup(character, val);
    }
}
