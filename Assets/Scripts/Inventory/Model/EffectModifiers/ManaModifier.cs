using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaModifier", menuName = "Modifiers/Mana", order = 1)]
public class ManaModifier : CharacterStatModifierSO
{
    public int ID = 1;

    public override void AffectCharacter(GameObject character, int val, BuffSO buff = null)
    {
        DamageCalculator.DmgCalculator.CauseMPup(character, val);
    }
}
