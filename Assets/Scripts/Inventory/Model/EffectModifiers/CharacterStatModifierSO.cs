using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStatModifierSO : ScriptableObject
{
    //�÷��̾ ���� ȿ��
    public abstract void AffectCharacter(GameObject character, int val, BuffSO buff = null);
}
