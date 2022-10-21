using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Resource health = character.GetComponent<Resource>();
        if (health != null)
            health.AddHP((int)val);
    }
}
