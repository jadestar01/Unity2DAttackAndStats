using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slash", menuName = "Skill/Melee/LB/Slash", order = 1)]
public class Slash : SkillSO
{
    public AudioClip slashSound;
    public int range;
    //hand엔 Find를 사용하자.
    public override void SkillAction(GameObject hand)
    {
        //Weapon가져오기
        GameObject weapon = null;
        if (hand.transform.childCount == 0)
            return;
        weapon = hand.transform.GetChild(0).gameObject;

        //스킬 사용중
        WeaponController.Instance.isAttack = true;

        hand.transform.DORotate(new Vector3(0, 0, 360), 2.5f, RotateMode.FastBeyond360)
                     .SetEase(Ease.Linear);

        //스킬 사용완료
        //WeaponController.Instance.isAttack = false;
    }
}
