using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;

public class MeleeSkill : MonoBehaviour
{
    public GameObject Melee ;
    static public GameObject melee;
    static BoxCollider2D boxCollider;
    static TrailRenderer trailRenderer;
    Vector2 mouse;
    Vector2 position;
    static int dir;
    static float deg;
    static float angle;

    public Slash slash;

    void Start()
    {
        melee = Melee;
        boxCollider = melee.transform.GetChild(0).GetComponent<BoxCollider2D>(); boxCollider.enabled = false;
        trailRenderer = melee.transform.GetChild(0).GetComponent<TrailRenderer>(); trailRenderer.enabled = false;
    }

    void Update()
    {
        mouse = GetComponent<WeaponController>().mouse;
        position = GetComponent<WeaponController>().position;
        angle = GetComponent<WeaponController>().angle;
        deg = Mathf.Atan2(mouse.y - melee.transform.position.y, mouse.x - melee.transform.position.x) * Mathf.Rad2Deg;
        if (mouse.x - melee.transform.GetChild(0).transform.position.x >= 0) dir = 1; else dir = -1;

        slash.UseSkill();
    }

    [System.Serializable]
    public class Slash : SuperSkill.Skill
    {
        float direction = dir;
        public float attackDeg;
        Slash() { }
        public override void UseSkill()
        {
            if (Input.GetKeyDown(skillKey) && !GetisAttack() && CheckMP() && CheckFP())
            {
                Debug.Log("Slash Start");
                CostMP(); CostFP();
                //Ready
                Sequence SlashSeq = DOTween.Sequence();
                SlashSeq.AppendCallback(() => SetisAttack(true));
                SlashSeq.Append(melee.transform.DORotate(new Vector3(0, 0, dir * attackDeg), readyTime, RotateMode.LocalAxisAdd));
                //Attack
                SlashSeq.AppendCallback(() => boxCollider.enabled = true);
                SlashSeq.AppendCallback(() => trailRenderer.enabled = true);
                SlashSeq.Append(melee.transform.DORotate(new Vector3(0, 0, -1 * dir * attackDeg * 2), attackTime, RotateMode.WorldAxisAdd));
                //Attack Done
                SlashSeq.Append(melee.transform.DORotate(Vector3.zero, 0));
                SlashSeq.AppendCallback(() => boxCollider.enabled = false);
                SlashSeq.AppendCallback(() => trailRenderer.enabled = false);
                SlashSeq.AppendCallback(() => SetisAttack(false));
                SlashSeq.Play();
            }
        }
    }

    [System.Serializable]
    public class UpperSlash : SuperSkill.Skill
    {
        public float attackDeg;
        public override void UseSkill()
        {
        }
    }

    [System.Serializable]
    public class Piercing : SuperSkill.Skill
    {
        public float piercingDistance;
        public override void UseSkill()
        {
        }
    }
}
