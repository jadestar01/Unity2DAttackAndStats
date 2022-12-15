using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthModifier", menuName = "Modifiers/Health", order = 0)]
public class HealthModifier : CharacterStatModifierSO
{
    public int ID = 0;
    public override void AffectCharacter(GameObject character, float val, BuffSO buff = null)
    {
        if (character.tag == "Player")
        {
            if (character.GetComponent<Stats>().curHealth + val < character.GetComponent<Stats>().health)
            {
                character.GetComponent<Stats>().curHealth += val;
            }
            else if (character.GetComponent<Stats>().curHealth + val >= character.GetComponent<Stats>().curHealth)
            {
                character.GetComponent<Stats>().curHealth += character.GetComponent<Stats>().health - character.GetComponent<Stats>().curHealth;
            }
        }
    }
}
