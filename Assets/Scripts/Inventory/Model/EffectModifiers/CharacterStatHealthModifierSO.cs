using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        if (character.tag == "Player")
        {
            if (character.GetComponent<Stats>().curHealth + val < character.GetComponent<Stats>().health)
            {
                character.GetComponent<Stats>().curHealth += val;
            }
            else if(character.GetComponent<Stats>().curHealth + val >= character.GetComponent<Stats>().curHealth)
            {
                character.GetComponent<Stats>().curHealth += character.GetComponent<Stats>().health - character.GetComponent<Stats>().curHealth;
            }
        }
    }
}
