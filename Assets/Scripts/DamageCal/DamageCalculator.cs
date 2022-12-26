using BansheeGz.BGDatabase;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DamageCalculator : MonoBehaviour
{
    private static DamageCalculator dmgCalculator;

    public void CauseDamage(GameObject attacker, GameObject defender, SkillDamage skillProfile)
    {
        if (defender.tag == "Player")
        {
            Debug.Log("attacker" + attacker);
            Debug.Log("defender" + defender);
            Debug.Log("skillProfile" + skillProfile);
            defender.GetComponent<Stats>().curHealth -= DmgCal(attacker, defender, skillProfile).totalDamage;
            //대미지 출력 to defender
        }
        else
        {
            defender.GetComponent<MobController>().curHealth -= DmgCal(attacker, defender, skillProfile).totalDamage;
        }
    }

    public static DamageCalculator DmgCalculator
    {
        get
        {
            if (null == dmgCalculator)
            {
                return null;
            }
            return dmgCalculator;
        }
    }

    private void Awake()
    {
        if (dmgCalculator == null)
        {
            dmgCalculator = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //방어도에 경감된 피해량, 넉백량 리턴
    //(주체, 대상, 스킬계수)

    public Damage DmgCal(GameObject main, GameObject target, SkillDamage skillProfile)
    {
        if (main == null)
        {
            if (target.tag == "Player")
            {
                Stats defender = target.GetComponent<Stats>();
                int PhysicalDmg = skillProfile.physicalDmg;
                int MagicalDmg = skillProfile.magicalDmg;

                if (PhysicalDmg != 0)
                    PhysicalDmg = (int)(PhysicalDmg * (1 + (-1) * (defender.armor) / 100));
                if (MagicalDmg != 0)
                    MagicalDmg = (int)(MagicalDmg * (1 + (-1) * (defender.registance) / 100));

                Debug.Log("None -> Player");
                Debug.Log("물리댐 : " + PhysicalDmg);
                Debug.Log("마법댐 : " + MagicalDmg);

                return new Damage
                {
                    totalDamage = PhysicalDmg + MagicalDmg,
                    isDodge = false,
                    PhysicalDmg = PhysicalDmg,
                    isPhysicalCrit = false,
                    MagicalDmg = MagicalDmg,
                    isMagicalCrit = false,
                    knockback = 1
                };
            }
            else
            {
                MobController defender = target.GetComponent<MobController>();
                int PhysicalDmg = skillProfile.physicalDmg;
                int MagicalDmg = skillProfile.magicalDmg;

                if (PhysicalDmg != 0)
                    PhysicalDmg = (int)(PhysicalDmg * (1 + (-1) * (defender.armor) / 100));
                if (MagicalDmg != 0)
                    MagicalDmg = (int)(MagicalDmg * (1 + (-1) * (defender.registance) / 100));

                Debug.Log("None -> Mob");
                Debug.Log("물리댐 : " + PhysicalDmg);
                Debug.Log("마법댐 : " + MagicalDmg);

                return new Damage
                {
                    totalDamage = PhysicalDmg + MagicalDmg,
                    isDodge = false,
                    PhysicalDmg = PhysicalDmg,
                    isPhysicalCrit = false,
                    MagicalDmg = MagicalDmg,
                    isMagicalCrit = false,
                    knockback = 1
                };
            }
        }

        if (main.tag == "Player")
        {
            Stats attacker = main.GetComponent<Stats>();
            MobController defender = target.GetComponent<MobController>();

            if (UnityEngine.Random.Range(0, 101) < defender.dodge && skillProfile.canDodge == true)
            {
                Debug.Log("회피함!");
                return new Damage { isDodge = true, totalDamage = 0, knockback = 0 };
            }

            SkillDamage skill = new SkillDamage();
            skillProfile.DmgCal(main);
            skill = skillProfile.Deepcopy();

            //기본적으로 입힐 수 있는 대미지 // 크리티컬, 방어력, 흡혈
            int PhysicalDmg = skill.physicalDmg + skill.extraPhysicalDmg;
            bool isPhysicalCrit = UnityEngine.Random.Range(0, 101) < (attacker.physicalCritRate + skill.extraPhysicalCritRate) ? true : false;
            if (PhysicalDmg != 0)
            {
                if (isPhysicalCrit)      //크리가 터졌다면, 
                {
                    PhysicalDmg = (int)(PhysicalDmg * (1 + (attacker.physicalCritDmg + skill.extraPhysicalCritDmg) / 100));
                }
            }

            int MagicalDmg = skill.magicalDmg + skill.extraMagicalDmg;
            bool isMagicalCrit = UnityEngine.Random.Range(0, 101) < (attacker.magicalCritRate + skill.extraMagicalCritRate) ? true : false;
            if (MagicalDmg != 0)
            {
                if (isMagicalCrit)
                {
                    MagicalDmg = (int)(MagicalDmg * (1 + (attacker.magicalCritDmg + skill.extraMagicalCritDmg) / 100));
                }
            }
            //타깃의 방어도/방어도관통력 와 넉벡 계산
            PhysicalDmg = (int)(PhysicalDmg * (1 + ((attacker.physicalPenetration + skill.extraPhysicalPenetration - defender.armor) / 100)));
            MagicalDmg = (int)(MagicalDmg * (1 + ((attacker.magicalPenetration + skill.extraMagicalPenetration - defender.registance) / 100)));

            float knockback = (1 + ((attacker.strike + skill.extraStrike - defender.grit) / 100));

            int vampirism = (int)((PhysicalDmg + MagicalDmg) * ((attacker.vampirism + skill.extraVampirism) / 100));

            Debug.Log("Player -> Mob");
            Debug.Log("회피 : " + false);
            Debug.Log("물리댐 : " + PhysicalDmg);
            Debug.Log("치명타 : " + isPhysicalCrit);
            Debug.Log("마법댐 : " + MagicalDmg);
            Debug.Log("극대화 : " + isMagicalCrit);
            Debug.Log("넉백 : " + knockback);
            Debug.Log("흡혈 : " + vampirism);
                 
            //피해를 가한다.
            return new Damage
            {
                totalDamage = PhysicalDmg + MagicalDmg,
                isDodge = false,
                PhysicalDmg = PhysicalDmg,
                isPhysicalCrit = isPhysicalCrit,
                MagicalDmg = MagicalDmg,
                isMagicalCrit = isMagicalCrit,
                knockback = knockback,
                recovery = vampirism
            };
        }
        else
        {
            MobController attacker = main.GetComponent<MobController>();
            Stats defender = target.GetComponent<Stats>();

            if (UnityEngine.Random.Range(0, 101) < defender.dodge && skillProfile.canDodge == true)
            {
                Debug.Log("회피함!");
                return new Damage { isDodge = true, totalDamage = 0, knockback = 0 };
            }

            SkillDamage skill = new SkillDamage();
            skillProfile.DmgCal(main);
            skill = skillProfile.Deepcopy();

            //기본적으로 입힐 수 있는 대미지 // 크리티컬, 방어력, 흡혈
            int PhysicalDmg = skill.physicalDmg + skill.extraPhysicalDmg;
            bool isPhysicalCrit = UnityEngine.Random.Range(0, 101) < (attacker.physicalCritRate + skill.extraPhysicalCritRate) ? true : false;
            if (PhysicalDmg != 0)
            {
                if (isPhysicalCrit)      //크리가 터졌다면, 
                {
                    PhysicalDmg = (int)(PhysicalDmg * (1 + (attacker.physicalCritDmg + skill.extraPhysicalCritDmg) / 100));
                }
            }

            int MagicalDmg = skill.magicalDmg + skill.extraMagicalDmg;
            bool isMagicalCrit = UnityEngine.Random.Range(0, 101) < (attacker.magicalCritRate + skill.extraMagicalCritRate) ? true : false;
            if (MagicalDmg != 0)
            {
                if (isMagicalCrit)
                {
                    MagicalDmg = (int)(MagicalDmg * (1 + (attacker.magicalCritDmg + skill.extraMagicalCritDmg) / 100));
                }
            }
            //타깃의 방어도/방어도관통력 와 넉벡 계산
            PhysicalDmg = (int)(PhysicalDmg * (1 + ((attacker.physicalPenetration + skill.extraPhysicalPenetration - defender.armor) / 100)));
            MagicalDmg = (int)(MagicalDmg * (1 + ((attacker.magicalPenetration + skill.extraMagicalPenetration - defender.registance) / 100)));

            float knockback = (1 + ((attacker.strike + skill.extraStrike - defender.grit) / 100));

            int vampirism = (int)((PhysicalDmg + MagicalDmg) * ((attacker.vampirism + skill.extraVampirism) / 100));

            Debug.Log("Mob -> Player");
            Debug.Log("회피 : " + false);
            Debug.Log("물리댐 : " + PhysicalDmg);
            Debug.Log("치명타 : " + isPhysicalCrit);
            Debug.Log("마법댐 : " + MagicalDmg);
            Debug.Log("극대화 : " + isMagicalCrit);
            Debug.Log("넉백 : " + knockback);
            Debug.Log("흡혈 : " + vampirism);

            //피해를 가한다.
            return new Damage
            {
                totalDamage = PhysicalDmg + MagicalDmg,
                isDodge = false,
                PhysicalDmg = PhysicalDmg,
                isPhysicalCrit = isPhysicalCrit,
                MagicalDmg = MagicalDmg,
                isMagicalCrit = isMagicalCrit,
                knockback = knockback,
                recovery = vampirism
            };
        }
    }
}

[Serializable]
public class SkillDamage
{
    public bool canDodge = true;                       //회피 가능한 기술인가?
    public int physicalDmg;                     //추가 물리피해량이 있을 때만, 물리피해
    public int magicalDmg;                      //추가 마법피해량이 있을 때만, 마법피해
    public int extraPhysicalDmg;
    public int extraMagicalDmg;
    public int extraVampirism;
    public int extraStrike;
    public int extraPhysicalCritRate;
    public int extraPhysicalCritDmg;
    public int extraPhysicalPenetration;
    public int extraMagicalCritRate;
    public int extraMagicalCritDmg;
    public int extraMagicalPenetration;

    public DamageApplication physical = new DamageApplication();
    public DamageApplication magical = new DamageApplication();

    public void DmgCal(GameObject subject)
    {
        extraPhysicalDmg = 0;
        extraMagicalDmg = 0;
        if (subject.tag == "Player")
        {
            Stats attacker = subject.GetComponent<Stats>();

            if (physical.health != 0)
                extraPhysicalDmg += (int)(attacker.health * physical.health);
            if (physical.mana != 0)
                extraPhysicalDmg += (int)(attacker.mana * physical.mana);
            if (physical.stamina != 0)
                extraPhysicalDmg += (int)(attacker.stamina * physical.stamina);
            if (physical.strike != 0)
                extraPhysicalDmg += (int)(attacker.strike * physical.strike);
            if (physical.physicalDmg != 0)
                extraPhysicalDmg += (int)((UnityEngine.Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * physical.physicalDmg));
            if (physical.physicalAttackSpeed != 0)
                extraPhysicalDmg += (int)(attacker.physicalAttackSpeed * physical.physicalAttackSpeed);
            if (physical.physicalPenetration != 0)
                extraPhysicalDmg += (int)(attacker.physicalPenetration * physical.physicalPenetration);
            if (physical.magicalDmg != 0)
                extraPhysicalDmg += (int)((UnityEngine.Random.Range(attacker.magicalMinDmg, attacker.magicalMaxDmg + 1) * physical.magicalDmg));
            if (physical.magicalAttackSpeed != 0)
                extraPhysicalDmg += (int)(attacker.magicalAttackSpeed * physical.magicalAttackSpeed);
            if (physical.magicalPenetration != 0)
                extraPhysicalDmg += (int)(attacker.magicalPenetration * physical.magicalPenetration);
            if (physical.armor != 0)
                extraPhysicalDmg += (int)(attacker.armor * physical.armor);
            if (physical.registance != 0)
                extraPhysicalDmg += (int)(attacker.registance * physical.registance);
            if (physical.grit != 0)
                extraPhysicalDmg += (int)(attacker.grit * physical.grit);

            if (magical.health != 0)
                extraMagicalDmg += (int)(attacker.health * magical.health);
            if (magical.mana != 0)
                extraMagicalDmg += (int)(attacker.mana * magical.mana);
            if (magical.stamina != 0)
                extraMagicalDmg += (int)(attacker.stamina * magical.stamina);
            if (magical.strike != 0)
                extraMagicalDmg += (int)(attacker.strike * magical.strike);
            if (magical.physicalDmg != 0)
                extraMagicalDmg += (int)((UnityEngine.Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * magical.physicalDmg));
            if (magical.physicalAttackSpeed != 0)
                extraMagicalDmg += (int)(attacker.physicalAttackSpeed * magical.physicalAttackSpeed);
            if (magical.physicalPenetration != 0)
                extraMagicalDmg += (int)(attacker.physicalPenetration * magical.physicalPenetration);
            if (magical.magicalDmg != 0)
                extraMagicalDmg += (int)((UnityEngine.Random.Range(attacker.magicalMinDmg, attacker.magicalMaxDmg + 1) * magical.magicalDmg));
            if (magical.magicalAttackSpeed != 0)
                extraMagicalDmg += (int)(attacker.magicalAttackSpeed * magical.magicalAttackSpeed);
            if (magical.magicalPenetration != 0)
                extraMagicalDmg += (int)(attacker.magicalPenetration * magical.magicalPenetration);
            if (magical.armor != 0)
                extraMagicalDmg += (int)(attacker.armor * magical.armor);
            if (magical.registance != 0)
                extraMagicalDmg += (int)(attacker.registance * magical.registance);
            if (magical.grit != 0)
                extraMagicalDmg += (int)(attacker.grit * magical.grit);
        }
        else
        {
            MobController attacker = subject.GetComponent<MobController>();

            if (physical.health != 0)
                extraPhysicalDmg += (int)(attacker.maxHealth * physical.health);
            if (physical.strike != 0)
                extraPhysicalDmg += (int)(attacker.strike * physical.strike);
            if (physical.physicalDmg != 0)
                extraPhysicalDmg += (int)((UnityEngine.Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * physical.physicalDmg));
            if (physical.physicalAttackSpeed != 0)
                extraPhysicalDmg += (int)(attacker.physicalAttackSpeed * physical.physicalAttackSpeed);
            if (physical.physicalPenetration != 0)
                extraPhysicalDmg += (int)(attacker.physicalPenetration * physical.physicalPenetration);
            if (physical.magicalDmg != 0)
                extraPhysicalDmg += (int)((UnityEngine.Random.Range(attacker.magicalMinDmg, attacker.magicalMaxDmg + 1) * physical.magicalDmg));
            if (physical.magicalAttackSpeed != 0)
                extraPhysicalDmg += (int)(attacker.magicalAttackSpeed * physical.magicalAttackSpeed);
            if (physical.magicalPenetration != 0)
                extraPhysicalDmg += (int)(attacker.magicalPenetration * physical.magicalPenetration);
            if (physical.armor != 0)
                extraPhysicalDmg += (int)(attacker.armor * physical.armor);
            if (physical.registance != 0)
                extraPhysicalDmg += (int)(attacker.registance * physical.registance);
            if (physical.grit != 0)
                extraPhysicalDmg += (int)(attacker.grit * physical.grit);

            if (magical.health != 0)
                extraMagicalDmg += (int)(attacker.maxHealth * magical.health);
            if (magical.strike != 0)
                extraMagicalDmg += (int)(attacker.strike * magical.strike);
            if (magical.physicalDmg != 0)
                extraMagicalDmg += (int)((UnityEngine.Random.Range(attacker.physicalMinDmg, attacker.physicalMaxDmg + 1) * magical.physicalDmg));
            if (magical.physicalAttackSpeed != 0)
                extraMagicalDmg += (int)(attacker.physicalAttackSpeed * magical.physicalAttackSpeed);
            if (magical.physicalPenetration != 0)
                extraMagicalDmg += (int)(attacker.physicalPenetration * magical.physicalPenetration);
            if (magical.magicalDmg != 0)
                extraMagicalDmg += (int)((UnityEngine.Random.Range(attacker.magicalMinDmg, attacker.magicalMaxDmg + 1) * magical.magicalDmg));
            if (magical.magicalAttackSpeed != 0)
                extraMagicalDmg += (int)(attacker.magicalAttackSpeed * magical.magicalAttackSpeed);
            if (magical.magicalPenetration != 0)
                extraMagicalDmg += (int)(attacker.magicalPenetration * magical.magicalPenetration);
            if (magical.armor != 0)
                extraMagicalDmg += (int)(attacker.armor * magical.armor);
            if (magical.registance != 0)
                extraMagicalDmg += (int)(attacker.registance * magical.registance);
            if (magical.grit != 0)
                extraMagicalDmg += (int)(attacker.grit * magical.grit);
        }
    }

    public SkillDamage Deepcopy()
    {
        SkillDamage damage = new SkillDamage
        {
            physicalDmg = physicalDmg,
            extraPhysicalDmg = extraPhysicalDmg,
            magicalDmg = magicalDmg,
            extraMagicalDmg = extraMagicalDmg,
            extraVampirism = extraVampirism,
            extraStrike = extraStrike,
            extraMagicalPenetration = extraMagicalPenetration,
            extraPhysicalPenetration = extraPhysicalPenetration,
            extraMagicalCritDmg = extraMagicalCritDmg,
            extraMagicalCritRate = extraMagicalCritRate,
            extraPhysicalCritDmg = extraPhysicalCritDmg,
            extraPhysicalCritRate = extraPhysicalCritRate
        };

        return damage;
    }
}


[Serializable]
public class DamageApplication
{
    public float health;                        //0.3   30%
    public float mana;
    public float stamina;
    public float strike;
    public float physicalDmg;                   //0.5   50%
    public float physicalAttackSpeed;
    public float physicalPenetration;
    public float magicalDmg;
    public float magicalAttackSpeed;
    public float magicalPenetration;
    public float armor;
    public float registance;
    public float grit;
}

public struct Damage
{
    public float totalDamage;               //defender가 입는 최종 대미지
    public bool isDodge;                    //회피
    public int PhysicalDmg;                 //물리피해
    public bool isPhysicalCrit;             //치명타 여부
    public int MagicalDmg;                  //마법피해
    public bool isMagicalCrit;              //극대화 여부
    public float knockback;                 //넉백량
    public int recovery;                    //회복량
}