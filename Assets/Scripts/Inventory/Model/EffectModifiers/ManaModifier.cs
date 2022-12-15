using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaModifier", menuName = "Modifiers/Mana", order = 1)]
public class ManaModifier : CharacterStatModifierSO
{
    public int ID = 1;

    public override void AffectCharacter(GameObject character, float val, BuffSO buff = null)
    {
        if (character.tag == "Player")
        {
            if (character.GetComponent<Stats>().curMana + val < character.GetComponent<Stats>().mana)
            {
                character.GetComponent<Stats>().curMana += val;
            }
            else if (character.GetComponent<Stats>().curMana + val >= character.GetComponent<Stats>().curMana)
            {
                character.GetComponent<Stats>().curMana += character.GetComponent<Stats>().mana - character.GetComponent<Stats>().curMana;
            }
        }
    }
}
