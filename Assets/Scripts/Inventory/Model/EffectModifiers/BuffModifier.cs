using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffModifier", menuName = "Modifiers/Buff", order = 3)]
public class BuffModifier : CharacterStatModifierSO
{
    public int ID = 3;
    public override void AffectCharacter(GameObject character, int val, BuffSO buff = null)
    {
        if (character.tag == "Player" && buff != null)
        {
            character.GetComponent<BuffManagement>().AddBuff(buff);
        }
    }
}