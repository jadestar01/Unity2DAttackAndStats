using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaminaModifier", menuName = "Modifiers/Stamina", order = 2)]
public class StaminaModifier : CharacterStatModifierSO
{
    public int ID = 2;

    public override void AffectCharacter(GameObject character, int val, BuffSO buff = null)
    {
        DamageCalculator.DmgCalculator.CauseSPup(character, val);
    }
}
