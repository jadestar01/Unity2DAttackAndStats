using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBuff", menuName = "Buff/StatBuff", order = 2)]
public class StatBuffSO : BuffSO
{
    [field: SerializeField] public ItemParameter[] stat;
    public override void AffectTarget(GameObject Target)
    {
    }
}
