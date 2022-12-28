using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStatModifierSO : ScriptableObject
{
    //플레이어가 얻을 효과
    public abstract void AffectCharacter(GameObject character, int val, BuffSO buff = null);
}
