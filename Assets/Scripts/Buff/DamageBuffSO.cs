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
        Debug.Log(Target + "이 " + Name + "으로 인해 " + Damage + "의 피해를 입었습니다!");
    }
}
