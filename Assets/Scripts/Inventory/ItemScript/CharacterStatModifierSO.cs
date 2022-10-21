using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 소모시 적용될 기능
public abstract class CharacterStatModifierSO : ScriptableObject
{
    public abstract void AffectCharacter(GameObject character, float val);
}
