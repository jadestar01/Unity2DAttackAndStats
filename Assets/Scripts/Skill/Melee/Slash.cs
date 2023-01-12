using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slash", menuName = "Skill/Melee/LB/Slash", order = 1)]
public class Slash : SkillSO
{
    public AudioClip slashSound;
    public int range;
    //hand�� Find�� �������.
    public override void SkillAction(GameObject hand)
    {
        //Weapon��������
        GameObject weapon = null;
        if (hand.transform.childCount == 0)
            return;
        weapon = hand.transform.GetChild(0).gameObject;

        //��ų �����
        WeaponController.Instance.isAttack = true;

        hand.transform.DORotate(new Vector3(0, 0, 360), 2.5f, RotateMode.FastBeyond360)
                     .SetEase(Ease.Linear);

        //��ų ���Ϸ�
        //WeaponController.Instance.isAttack = false;
    }
}
