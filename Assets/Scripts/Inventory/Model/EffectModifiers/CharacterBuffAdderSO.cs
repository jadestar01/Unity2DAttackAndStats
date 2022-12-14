using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class CharacterBuffAdderSO : CharacterStatModifierSO
{
    [ShowInInspector] public BuffSO buff;
    public override void AffectCharacter(GameObject character, float val, BuffSO buff = null)
    {
        if (character.tag == "Player" && buff != null)
        {
            Debug.Log("There is no problem");
            character.GetComponent<BuffManagement>().AddBuff(buff, character);
        }
    }
}
