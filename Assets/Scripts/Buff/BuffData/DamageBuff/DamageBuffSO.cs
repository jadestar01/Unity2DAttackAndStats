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
        Debug.Log(Target + "�� " + Name + "���� ���� " + Damage + "�� ���ظ� �Ծ����ϴ�!");
    }
}