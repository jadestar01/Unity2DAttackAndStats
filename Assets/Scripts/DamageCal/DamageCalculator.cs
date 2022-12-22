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

    //���� �氨�� ���ط�, �˹鷮 ����
    //(��ü, ���, ��ų���)
    public Damage DmgCal(GameObject main, GameObject target, SkillDamage skill = new SkillDamage())
    {
        if (main.tag == "Player")
        {
            Stats attacker = main.GetComponent<Stats>();
            MobController defender = target.GetComponent<MobController>();

            if (Random.Range(0, 100) < defender.dodge)
            {
                Debug.Log("ȸ����!");
                return new Damage { isDodge = true };
            }

            int PhysicalDmg = 0;
            bool isPhysicalCrit = Random.Range(0, 100) < attacker.physicalCritRate ? true : false;
            if (attacker.physicalMinDmg != 0 && attacker.physicalMaxDmg != 0)
            {
                if (isPhysicalCrit)      //ũ���� �����ٸ�, 
                {
                    PhysicalDmg = (int)((Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * (1 + skill.physicalDmgCoeffiecient / 100) + skill.additionalPhysicalDmg) * (1 + attacker.physicalCritDmg / 100));
                    //Min~Max ������ �̾Ƽ� ġ��Ÿ ������� �����Ų��.
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
            //Ÿ���� ��/������� �� �˺� ���

            PhysicalDmg = (int)(PhysicalDmg * (1 + (-1) * ((defender.armor - attacker.physicalPenetration) / 100)));
            MagicalDmg = (int)(MagicalDmg * (1 + (-1) * ((defender.registance - attacker.magicalPenetration) / 100)));

            float knockback = (1 + (-1) * ((defender.grit - attacker.strike) / 100));

            Debug.Log("Player -> Mob");
            Debug.Log("ȸ�� : " + false);
            Debug.Log("������ : " + PhysicalDmg);
            Debug.Log("ġ��Ÿ : " + isPhysicalCrit);
            Debug.Log("������ : " + MagicalDmg);
            Debug.Log("�ش�ȭ : " + isMagicalCrit);
            Debug.Log("�˹� : " + knockback);
            //���ظ� ���Ѵ�.
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
                Debug.Log("ȸ����!");
                return new Damage { isDodge = true };
            }

            int PhysicalDmg = 0;
            bool isPhysicalCrit = Random.Range(0, 100) < attacker.physicalCritRate ? true : false;
            if (attacker.physicalMinDmg != 0 && attacker.physicalMaxDmg != 0)
            {
                if (isPhysicalCrit)      //ũ���� �����ٸ�, 
                {
                    PhysicalDmg = (int)((Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * (1 + skill.physicalDmgCoeffiecient / 100) + skill.additionalPhysicalDmg) * (1 + attacker.physicalCritDmg / 100));
                    //Min~Max ������ �̾Ƽ� ġ��Ÿ ������� �����Ų��.
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
            //Ÿ���� ��/������� �� �˺� ���

            PhysicalDmg = (int)(PhysicalDmg * (1 + (-1) * ((defender.armor - attacker.physicalPenetration) / 100)));
            MagicalDmg = (int)(MagicalDmg * (1 + (-1) * ((defender.registance - attacker.magicalPenetration) / 100)));

            float knockback = (1 + (-1) * ((defender.grit - attacker.strike) / 100));

            Debug.Log("Mob -> Player");
            Debug.Log("ȸ�� : " + false);
            Debug.Log("������ : " + PhysicalDmg);
            Debug.Log("ġ��Ÿ : " + isPhysicalCrit);
            Debug.Log("������ : " + MagicalDmg);
            Debug.Log("�ش�ȭ : " + isMagicalCrit);
            Debug.Log("�˹� : " + knockback);
            //���ظ� ���Ѵ�.
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
        public float additionalPhysicalDmg;     //�߰� �������ط�
        public float physicalDmgCoeffiecient;   //�������� ���
        public float additionalMagicalDmg;      //�߰� �������ط�
        public float magicalDmgCoeffiecient;    //�������� ���
    }

    public struct Damage
    {
        public bool isDodge;
        public int PhysicalDmg;                 //��������
        public bool isPhysicalCrit;             //ġ��Ÿ ����
        public int MagicalDmg;                  //��������
        public bool isMagicalCrit;              //�ش�ȭ ����
        public float knockback;                 //�˹鷮
    }
}
