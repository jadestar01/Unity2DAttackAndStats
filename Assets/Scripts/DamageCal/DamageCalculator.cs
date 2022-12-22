using BansheeGz.BGDatabase;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    public GameObject player;
    public GameObject target;
    [ShowInInspector] public  SkillDamage skill = new SkillDamage();

    [Button]
    public void cal()
    {
        DmgCal(player, target, skill);
    }

    //방어도에 경감된 피해량, 넉백량 리턴
    //(주체, 대상, 스킬계수)
    public Damage DmgCal(GameObject main, GameObject target, SkillDamage skill = new SkillDamage())
    {
        if (main.tag == "Player")
        {
            Stats attacker = main.GetComponent<Stats>();
            MobController defender = target.GetComponent<MobController>();

            if (Random.Range(0, 100) < defender.dodge)
            {
                Debug.Log("회피함!");
                return new Damage { isDodge = true };
            }

            int PhysicalDmg = 0;
            bool isPhysicalCrit = Random.Range(0, 100) < attacker.physicalCritRate ? true : false;
            if (attacker.physicalMinDmg != 0 && attacker.physicalMaxDmg != 0)
            {
                if (isPhysicalCrit)      //크리가 터졌다면, 
                {
                    PhysicalDmg = (int)((Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * (1 + skill.physicalDmgCoeffiecient / 100) + skill.additionalPhysicalDmg) * (1 + attacker.physicalCritDmg / 100));
                    //Min~Max 댐지를 뽑아서 치명타 대미지를 적용시킨다.
                }
                else
                {
                    PhysicalDmg = (int)(Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * (1 + skill.physicalDmgCoeffiecient / 100) + skill.additionalPhysicalDmg);
                }
            }

            int MagicalDmg = 0;
            bool isMagicalCrit = Random.Range(0, 100) < attacker.magicalCritRate ? true : false;
            if (attacker.magicalMinDmg != 0 && attacker.magicalMaxDmg != 0)
            {
                if (isMagicalCrit)
                {
                    MagicalDmg = (int)((Random.Range(attacker.magicalMinDmg, attacker.magicalMaxDmg + 1) * (1 + skill.magicalDmgCoeffiecient / 100) + skill.additionalMagicalDmg) * (1 + attacker.magicalCritDmg / 100));
                }
                else
                {
                    MagicalDmg = (int)(Random.Range(attacker.magicalMinDmg, attacker.magicalMaxDmg + 1) * (1 + skill.magicalDmgCoeffiecient / 100) + skill.additionalMagicalDmg);
                }
            }
            //타깃의 방어도/방어도관통력 와 넉벡 계산

            PhysicalDmg = (int)(PhysicalDmg * (1 + (-1) * ((defender.armor - attacker.physicalPenetration) / 100)));
            MagicalDmg = (int)(MagicalDmg * (1 + (-1) * ((defender.registance - attacker.magicalPenetration) / 100)));

            float knockback = (1 + (-1) * ((defender.grit - attacker.strike) / 100));

            Debug.Log("Player -> Mob");
            Debug.Log("회피 : " + false);
            Debug.Log("물리댐 : " + PhysicalDmg);
            Debug.Log("치명타 : " + isPhysicalCrit);
            Debug.Log("마법댐 : " + MagicalDmg);
            Debug.Log("극대화 : " + isMagicalCrit);
            Debug.Log("넉백 : " + knockback);
            //피해를 가한다.
            return new Damage
            {
                isDodge = false,
                PhysicalDmg = PhysicalDmg,
                isPhysicalCrit = isPhysicalCrit,
                MagicalDmg = MagicalDmg,
                isMagicalCrit = isMagicalCrit,
                knockback = knockback
            };
        }
        else
        {
            MobController attacker = main.GetComponent<MobController>();
            Stats defender = target.GetComponent<Stats>();

            if (Random.Range(0, 100) < defender.dodge)
            {
                Debug.Log("회피함!");
                return new Damage { isDodge = true };
            }

            int PhysicalDmg = 0;
            bool isPhysicalCrit = Random.Range(0, 100) < attacker.physicalCritRate ? true : false;
            if (attacker.physicalMinDmg != 0 && attacker.physicalMaxDmg != 0)
            {
                if (isPhysicalCrit)      //크리가 터졌다면, 
                {
                    PhysicalDmg = (int)((Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * (1 + skill.physicalDmgCoeffiecient / 100) + skill.additionalPhysicalDmg) * (1 + attacker.physicalCritDmg / 100));
                    //Min~Max 댐지를 뽑아서 치명타 대미지를 적용시킨다.
                }
                else
                {
                    PhysicalDmg = (int)(Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * (1 + skill.physicalDmgCoeffiecient / 100) + skill.additionalPhysicalDmg);
                }
            }

            int MagicalDmg = 0;
            bool isMagicalCrit = Random.Range(0, 100) < attacker.magicalCritRate ? true : false;
            if (attacker.magicalMinDmg != 0 && attacker.magicalMaxDmg != 0)
            {
                if (isMagicalCrit)
                {
                    MagicalDmg = (int)((Random.Range(attacker.magicalMinDmg, attacker.magicalMaxDmg + 1) * (1 + skill.magicalDmgCoeffiecient / 100) + skill.additionalMagicalDmg) * (1 + attacker.magicalCritDmg / 100));
                }
                else
                {
                    MagicalDmg = (int)(Random.Range(attacker.magicalMinDmg, attacker.magicalMaxDmg + 1) * (1 + skill.magicalDmgCoeffiecient / 100) + skill.additionalMagicalDmg);
                }
            }
            //타깃의 방어도/방어도관통력 와 넉벡 계산

            PhysicalDmg = (int)(PhysicalDmg * (1 + (-1) * ((defender.armor - attacker.physicalPenetration) / 100)));
            MagicalDmg = (int)(MagicalDmg * (1 + (-1) * ((defender.registance - attacker.magicalPenetration) / 100)));

            float knockback = (1 + (-1) * ((defender.grit - attacker.strike) / 100));

            Debug.Log("Mob -> Player");
            Debug.Log("회피 : " + false);
            Debug.Log("물리댐 : " + PhysicalDmg);
            Debug.Log("치명타 : " + isPhysicalCrit);
            Debug.Log("마법댐 : " + MagicalDmg);
            Debug.Log("극대화 : " + isMagicalCrit);
            Debug.Log("넉백 : " + knockback);
            //피해를 가한다.
            return new Damage
            {
                isDodge = false,
                PhysicalDmg = PhysicalDmg,
                isPhysicalCrit = isPhysicalCrit,
                MagicalDmg = MagicalDmg,
                isMagicalCrit = isMagicalCrit,
                knockback = knockback
            };
        }
    }

    public struct SkillDamage
    {
        public float additionalPhysicalDmg;     //추가 물리피해량
        public float physicalDmgCoeffiecient;   //물리피해 계수
        public float additionalMagicalDmg;      //추가 마법피해량
        public float magicalDmgCoeffiecient;    //마법피해 계수
    }

    public struct Damage
    {
        public bool isDodge;
        public int PhysicalDmg;                 //물리피해
        public bool isPhysicalCrit;             //치명타 여부
        public int MagicalDmg;                  //마법피해
        public bool isMagicalCrit;              //극대화 여부
        public float knockback;                 //넉백량
    }
}
