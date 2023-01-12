using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillSO : ScriptableObject
{
    public int skillCode;                                       //스킬 코드
    public SkillKey skillKey;                                   //스킬 키     (LB, RB, Q, E) 
    public string skillName;                                    //스킬 이름
    public string skillDescription;                             //스킬 설명
    public int skillCurLv = 1;                                  //현재 레벨
    public int skillMaxLv = 5;                                  //최대 레벨
    public float skillCooltime;                                 //쿨타임
    public bool coolDown;                                       //사용여부 확인
    public float ReadyTime;                                     //영창시간
    public float attackTime;                                    //공격시간
    public float moveCoeffiecinet;                              //속도 영향
    public List<SkillBuff> skillBuffs = new List<SkillBuff>();  //버프 가하기
    public SkillType skillType;                                 //스킬 타입 (Melee, Magic, Range)
    public SkillDamage skillDamage;                             //스킬 대미지연산식
    public GameObject skillObject;                              //혹시 스킬 오브젝트
    public ParticleSystem skillParticle;                        //스킬 파티클
    public abstract void SkillAction(GameObject weapon);        //스킬 공격

    [Serializable]
    public struct SkillBuff
    {
        [SerializeField] BuffSO buff;                           //걸 버프
        [SerializeField] float rate;                            //버프 확률
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
