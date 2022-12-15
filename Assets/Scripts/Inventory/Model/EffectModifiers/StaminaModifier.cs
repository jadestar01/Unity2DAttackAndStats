using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaminaModifier", menuName = "Modifiers/Stamina", order = 2)]
public class StaminaModifier : CharacterStatModifierSO
{
    public int ID = 2;

    public override void AffectCharacter(GameObject character, float val, BuffSO buff = null)
    {
        if (character.tag == "Player")
        {
            if (character.GetComponent<Stats>().curStamina + val < character.GetComponent<Stats>().stamina)
            {
                character.GetComponent<Stats>().curStamina += val;
            }
            else if (character.GetComponent<Stats>().curStamina + val >= character.GetComponent<Stats>().curStamina)
            {
                character.GetComponent<Stats>().curStamina += character.GetComponent<Stats>().stamina - character.GetComponent<Stats>().curStamina;
            }
        }
    }
}
