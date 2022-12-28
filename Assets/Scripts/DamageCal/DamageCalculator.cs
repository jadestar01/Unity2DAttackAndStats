using BansheeGz.BGDatabase;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DamageCalculator : MonoBehaviour
{
    public void CauseDamage(GameObject attacker, GameObject defender, SkillDamage skillProfile)
    {
        if (defender.tag == "Player")
        {
            if (defender.GetComponent<Stats>().isDead == false)
            {
                Damage damage = DmgCal(attacker, defender, skillProfile);
                defender.GetComponent<Stats>().curHealth -= damage.totalDamage;
                if (damage.PhysicalDmg > 0)
                    DamageMessageManager.Instance.SetMessage(damage.PhysicalDmg, defender.transform.position, "-", "physical");
                if (damage.MagicalDmg > 0)
                    DamageMessageManager.Instance.SetMessage(damage.MagicalDmg, defender.transform.position, "-", "magical");
                if (damage.recovery > 0 && attacker != null)
                    CauseHPup(attacker, damage.recovery);
            }
        }
        else
        {
            if (defender.GetComponent<MobController>().isDead == false)
            {
                Damage damage = DmgCal(attacker, defender, skillProfile);
                defender.GetComponent<MobController>().curHealth -= damage.totalDamage;
                if (damage.PhysicalDmg > 0)
                    DamageMessageManager.Instance.SetMessage(damage.PhysicalDmg, defender.transform.position, "-", "physical");
                if (damage.MagicalDmg > 0)
                    DamageMessageManager.Instance.SetMessage(damage.MagicalDmg, defender.transform.position, "-", "magical");
                if (damage.recovery > 0 && attacker != null)
                    CauseHPup(attacker, damage.recovery);
            }
        }
    }

    public void CauseHPup(GameObject target, int value)
    {
        if (target.tag == "Player")
        {
            if (value != 0)
            {
                float curHealth = target.GetComponent<Stats>().curHealth;
                float maxHealth = target.GetComponent<Stats>().health;
                if (value > 0)                               //+����� ���
                {
                    if (curHealth + value <= maxHealth)      //���� �ʰ������ �ƴ϶��
                    {
                        DamageMessageManager.Instance.SetMessage(value, target.transform.position, "+", "health");
                        target.GetComponent<Stats>().curHealth += value;                  //�������ŭ ����Ѵ�.
                    }
                    else                                            //�ʰ�����̶��,
                    {
                        if (maxHealth - curHealth > 0)
                        {
                            DamageMessageManager.Instance.SetMessage((int)(maxHealth - curHealth), target.transform.position, "+", "health");
                            target.GetComponent<Stats>().curHealth += maxHealth - curHealth;         //�� ������ ��ŭ�� ����Ѵ�.   
                        }
                    }
                }
                if (value < 0)
                {
                    if (curHealth + value >= 0)         //ü�� ���Ұ� �ǵ�, -�� �ƴ϶��, 
                    {
                        DamageMessageManager.Instance.SetMessage((int)(value), target.transform.position, "-", "health");
                        target.GetComponent<Stats>().curHealth += value;
                    }
                    else                                //ü�� ���Ұ� �Ǹ�, -�� �ȴٸ�, curHealth��ŭ ���� 
                    {
                        if (curHealth != 0)
                        {
                            DamageMessageManager.Instance.SetMessage((int)(curHealth), target.transform.position, "-", "health");
                            target.GetComponent<Stats>().curHealth -= curHealth;
                        }
                    }
                }
            }
        }
        else
        {
            if (value != 0)
            {
                float curHealth = target.GetComponent<MobController>().curHealth;
                float maxHealth = target.GetComponent<MobController>().maxHealth;
                if (value > 0)                               //+����� ���
                {
                    if (curHealth + value <= maxHealth)      //���� �ʰ������ �ƴ϶��
                    {
                        DamageMessageManager.Instance.SetMessage(value, target.transform.position, "+", "health");
                        target.GetComponent<MobController>().curHealth += value;                  //�������ŭ ����Ѵ�.
                    }
                    else                                            //�ʰ�����̶��,
                    {
                        if (maxHealth - curHealth > 0)
                        {
                            DamageMessageManager.Instance.SetMessage((int)(maxHealth - curHealth), target.transform.position, "+", "health");
                            target.GetComponent<MobController>().curHealth += maxHealth - curHealth;         //�� ������ ��ŭ�� ����Ѵ�.   
                        }
                    }
                }
                if (value < 0)
                {
                    if (curHealth + value >= 0)         //ü�� ���Ұ� �ǵ�, -�� �ƴ϶��, 
                    {
                        DamageMessageManager.Instance.SetMessage((int)(value), target.transform.position, "-", "health");
                        target.GetComponent<MobController>().curHealth += value;
                    }
                    else                                //ü�� ���Ұ� �Ǹ�, -�� �ȴٸ�, curHealth��ŭ ���� 
                    {
                        if (curHealth != 0)
                        {
                            DamageMessageManager.Instance.SetMessage((int)(curHealth), target.transform.position, "-", "health");
                            target.GetComponent<MobController>().curHealth -= curHealth;
                        }
                    }
                }
            }
        }
    }

    public void CauseMPup(GameObject target, int value)
    {
        if (target.tag == "Player")
        {
            if (value != 0)
            {
                float curMana = target.GetComponent<Stats>().curMana;
                float maxMana = target.GetComponent<Stats>().mana;
                if (value > 0)                               //+����� ���
                {
                    if (curMana + value <= maxMana)      //���� �ʰ������ �ƴ϶��
                    {
                        DamageMessageManager.Instance.SetMessage(value, target.transform.position, "+", "mana");
                        target.GetComponent<Stats>().curMana += value;                  //�������ŭ ����Ѵ�.
                    }
                    else                                            //�ʰ�����̶��,
                    {
                        if (maxMana - curMana > 0)
                        {
                            DamageMessageManager.Instance.SetMessage((int)(maxMana - curMana), target.transform.position, "+", "mana");
                            target.GetComponent<Stats>().curMana += maxMana - curMana;         //�� ������ ��ŭ�� ����Ѵ�.   
                        }
                    }
                }
                if (value < 0)
                {
                    if (curMana + value >= 0)         //ü�� ���Ұ� �ǵ�, -�� �ƴ϶��, 
                    {
                        DamageMessageManager.Instance.SetMessage((int)(value), target.transform.position, "-", "mana");
                        target.GetComponent<Stats>().curMana += value;
                    }
                    else                                //ü�� ���Ұ� �Ǹ�, -�� �ȴٸ�, curMana��ŭ ���� 
                    {
                        if (curMana != 0)
                        {
                            DamageMessageManager.Instance.SetMessage((int)(curMana), target.transform.position, "-", "mana");
                            target.GetComponent<Stats>().curMana -= curMana;
                        }
                    }
                }
            }
        }
    }

    public void CauseSPup(GameObject target, int value)
    {
        if (target.tag == "Player")
        {
            if (value != 0)
            {
                float curStamina = target.GetComponent<Stats>().curStamina;
                float maxStamina = target.GetComponent<Stats>().stamina;
                if (value > 0)                               //+����� ���
                {
                    if (curStamina + value <= maxStamina)      //���� �ʰ������ �ƴ϶��
                    {
                        DamageMessageManager.Instance.SetMessage(value, target.transform.position, "+", "stamina");
                        target.GetComponent<Stats>().curStamina += value;                  //�������ŭ ����Ѵ�.
                    }
                    else                                            //�ʰ�����̶��,
                    {
                        if (maxStamina - curStamina > 0)
                        {
                            DamageMessageManager.Instance.SetMessage((int)(maxStamina - curStamina), target.transform.position, "+", "stamina");
                            target.GetComponent<Stats>().curStamina += maxStamina - curStamina;         //�� ������ ��ŭ�� ����Ѵ�.   
                        }
                    }
                }
                if (value < 0)
                {
                    if (curStamina + value >= 0)         //ü�� ���Ұ� �ǵ�, -�� �ƴ϶��, 
                    {
                        DamageMessageManager.Instance.SetMessage((int)(value), target.transform.position, "-", "stamina");
                        target.GetComponent<Stats>().curStamina += value;
                    }
                    else                                //ü�� ���Ұ� �Ǹ�, -�� �ȴٸ�, curStamina��ŭ ���� 
                    {
                        if (curStamina != 0)
                        {
                            DamageMessageManager.Instance.SetMessage((int)(curStamina), target.transform.position, "-", "stamina");
                            target.GetComponent<Stats>().curStamina -= curStamina;
                        }
                    }
                }
            }
        }
    }

    private static DamageCalculator dmgCalculator;
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

    //���� �氨�� ���ط�, �˹鷮 ����
    //(��ü, ���, ��ų���)

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
                Debug.Log("������ : " + PhysicalDmg);
                Debug.Log("������ : " + MagicalDmg);

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
                Debug.Log("������ : " + PhysicalDmg);
                Debug.Log("������ : " + MagicalDmg);

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
                Debug.Log("ȸ����!");
                return new Damage { isDodge = true, totalDamage = 0, knockback = 0 };
            }

            SkillDamage skill = new SkillDamage();
            skillProfile.DmgCal(main);
            skill = skillProfile.Deepcopy();

            //�⺻������ ���� �� �ִ� ����� // ũ��Ƽ��, ����, ����
            int PhysicalDmg = skill.physicalDmg + skill.extraPhysicalDmg;
            bool isPhysicalCrit = UnityEngine.Random.Range(0, 101) < (attacker.physicalCritRate + skill.extraPhysicalCritRate) ? true : false;
            if (PhysicalDmg != 0)
            {
                if (isPhysicalCrit)      //ũ���� �����ٸ�, 
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
            //Ÿ���� ��/������� �� �˺� ���
            PhysicalDmg = (int)(PhysicalDmg * (1 + ((attacker.physicalPenetration + skill.extraPhysicalPenetration - defender.armor) / 100)));
            MagicalDmg = (int)(MagicalDmg * (1 + ((attacker.magicalPenetration + skill.extraMagicalPenetration - defender.registance) / 100)));

            float knockback = (1 + ((attacker.strike + skill.extraStrike - defender.grit) / 100));

            int vampirism = (int)((PhysicalDmg + MagicalDmg) * ((attacker.vampirism + skill.extraVampirism) / 100));

            Debug.Log("Player -> Mob");
            Debug.Log("ȸ�� : " + false);
            Debug.Log("������ : " + PhysicalDmg);
            Debug.Log("ġ��Ÿ : " + isPhysicalCrit);
            Debug.Log("������ : " + MagicalDmg);
            Debug.Log("�ش�ȭ : " + isMagicalCrit);
            Debug.Log("�˹� : " + knockback);
            Debug.Log("���� : " + vampirism);
                 
            //���ظ� ���Ѵ�.
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
                Debug.Log("ȸ����!");
                return new Damage { isDodge = true, totalDamage = 0, knockback = 0 };
            }

            SkillDamage skill = new SkillDamage();
            skillProfile.DmgCal(main);
            skill = skillProfile.Deepcopy();

            //�⺻������ ���� �� �ִ� ����� // ũ��Ƽ��, ����, ����
            int PhysicalDmg = skill.physicalDmg + skill.extraPhysicalDmg;
            bool isPhysicalCrit = UnityEngine.Random.Range(0, 101) < (attacker.physicalCritRate + skill.extraPhysicalCritRate) ? true : false;
            if (PhysicalDmg != 0)
            {
                if (isPhysicalCrit)      //ũ���� �����ٸ�, 
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
            //Ÿ���� ��/������� �� �˺� ���
            PhysicalDmg = (int)(PhysicalDmg * (1 + ((attacker.physicalPenetration + skill.extraPhysicalPenetration - defender.armor) / 100)));
            MagicalDmg = (int)(MagicalDmg * (1 + ((attacker.magicalPenetration + skill.extraMagicalPenetration - defender.registance) / 100)));

            float knockback = (1 + ((attacker.strike + skill.extraStrike - defender.grit) / 100));

            int vampirism = (int)((PhysicalDmg + MagicalDmg) * ((attacker.vampirism + skill.extraVampirism) / 100));

            Debug.Log("Mob -> Player");
            Debug.Log("ȸ�� : " + false);
            Debug.Log("������ : " + PhysicalDmg);
            Debug.Log("ġ��Ÿ : " + isPhysicalCrit);
            Debug.Log("������ : " + MagicalDmg);
            Debug.Log("�ش�ȭ : " + isMagicalCrit);
            Debug.Log("�˹� : " + knockback);
            Debug.Log("���� : " + vampirism);

            //���ظ� ���Ѵ�.
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
    public bool canDodge = true;                       //ȸ�� ������ ����ΰ�?
    public int physicalDmg;                     //�߰� �������ط��� ���� ����, ��������
    public int magicalDmg;                      //�߰� �������ط��� ���� ����, ��������
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
    public int totalDamage;               //defender�� �Դ� ���� �����
    public bool isDodge;                    //ȸ��
    public int PhysicalDmg;                 //��������
    public bool isPhysicalCrit;             //ġ��Ÿ ����
    public int MagicalDmg;                  //��������
    public bool isMagicalCrit;              //�ش�ȭ ����
    public float knockback;                 //�˹鷮
    public int recovery;                    //ȸ����
}