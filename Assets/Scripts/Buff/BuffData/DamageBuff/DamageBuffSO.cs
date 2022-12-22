using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GetDamageBuff", menuName = "Buff/GetDamageBuff", order = 1)]
public class DamageBuffSO : BuffSO
{
    [field: SerializeField] public int Damage;
    public override void AffectTarget(GameObject Target)
    {
        if (Target.gameObject.tag == "Player")
        {
            Target.GetComponent<Stats>().curHealth -= Damage;
        }
        else
        {
            Target.GetComponent<MobController>().curHealth -= Damage;
        }
    }
}
