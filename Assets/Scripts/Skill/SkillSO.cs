using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillSO : ScriptableObject
{
    public int skillCode;                                       //��ų �ڵ�
    public SkillKey skillKey;                                   //��ų Ű     (LB, RB, Q, E) 
    public string skillName;                                    //��ų �̸�
    public string skillDescription;                             //��ų ����
    public int skillCurLv = 1;                                  //���� ����
    public int skillMaxLv = 5;                                  //�ִ� ����
    public float skillCooltime;                                 //��Ÿ��
    public bool coolDown;                                       //��뿩�� Ȯ��
    public float ReadyTime;                                     //��â�ð�
    public float attackTime;                                    //���ݽð�
    public float moveCoeffiecinet;                              //�ӵ� ����
    public List<SkillBuff> skillBuffs = new List<SkillBuff>();  //���� ���ϱ�
    public SkillType skillType;                                 //��ų Ÿ�� (Melee, Magic, Range)
    public SkillDamage skillDamage;                             //��ų ����������
    public GameObject skillObject;                              //Ȥ�� ��ų ������Ʈ
    public ParticleSystem skillParticle;                        //��ų ��ƼŬ
    public abstract void SkillAction(GameObject weapon);        //��ų ����

    [Serializable]
    public struct SkillBuff
    {
        [SerializeField] BuffSO buff;                           //�� ����
        [SerializeField] float rate;                            //���� Ȯ��
    }
}

public enum SkillType
{
    Melee,
    Magic,
    Range
}

public enum SkillKey
{
    LB,
    RB,
    Q,
    E
}
