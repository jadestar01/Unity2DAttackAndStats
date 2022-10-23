using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Resource HP = character.GetComponent<Resource>();
        if (HP != null)
            HP.AddHP((int)val);
    }
}
