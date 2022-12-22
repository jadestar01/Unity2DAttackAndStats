using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GetDamageBuff", menuName = "Buff/RegenBuff", order = 3)]
public class RegenBuff : BuffSO
{ 
    [field: SerializeField] public float HPRegenValue = 0.0f;
    [field: SerializeField] public float MPRegenValue = 0.0f;
    [field: SerializeField] public float SPRegenValue = 0.0f;

    public override void AffectTarget(GameObject Target)
    {
        if (Target.tag == "Player")
        {
            if (HPRegenValue != 0)
            {
                float curHealth = Target.GetComponent<Stats>().curHealth;
                float maxHealth = Target.GetComponent<Stats>().health;
                if (HPRegenValue > 0)                               //+����� ���
                {
                    if (curHealth + HPRegenValue <= maxHealth)      //���� �ʰ������ �ƴ϶��
                    {
                        Target.GetComponent<Stats>().curHealth += HPRegenValue;                  //�������ŭ ����Ѵ�.
                    }
                    else                                            //�ʰ�����̶��,
                        Target.GetComponent<Stats>().curHealth += maxHealth - curHealth;         //�� ������ ��ŭ�� ����Ѵ�.
                }
                else                                                //-����� ���
                {
                    if (curHealth + HPRegenValue >= 0)              //�������� 0���� ũ�ٸ�,
                        Target.GetComponent<Stats>().curHealth += HPRegenValue;
                    else
                        Target.GetComponent<Stats>().curHealth += HPRegenValue;
                }
            }

            if (MPRegenValue != 0)
            {
                float curMana = Target.GetComponent<Stats>().curMana;
                float maxMana = Target.GetComponent<Stats>().mana;
                if (MPRegenValue > 0)                               //+����� ���
                {
                    if (curMana + MPRegenValue <= maxMana)      //���� �ʰ������ �ƴ϶��
                        Target.GetComponent<Stats>().curMana += MPRegenValue;                  //�������ŭ ����Ѵ�.
                    else                                            //�ʰ�����̶��,
                        Target.GetComponent<Stats>().curMana += maxMana - curMana;         //�� ������ ��ŭ�� ����Ѵ�.
                }
                else                                                //-����� ���
                {
                    if (curMana + MPRegenValue >= 0)              //�������� 0���� ũ�ٸ�,
                        Target.GetComponent<Stats>().curMana += MPRegenValue;
                    else
                        Target.GetComponent<Stats>().curMana += MPRegenValue;
                }
            }

            if (SPRegenValue != 0)
            {
                float curStamina = Target.GetComponent<Stats>().curStamina;
                float maxStamina = Target.GetComponent<Stats>().stamina;
                if (SPRegenValue > 0)                               //+����� ���
                {
                    if (curStamina + SPRegenValue <= maxStamina)      //���� �ʰ������ �ƴ϶��
                        Target.GetComponent<Stats>().curStamina += SPRegenValue;                  //�������ŭ ����Ѵ�.
                    else                                            //�ʰ�����̶��,
                        Target.GetComponent<Stats>().curStamina += maxStamina - curStamina;         //�� ������ ��ŭ�� ����Ѵ�.
                }
                else                                                //-����� ���
                {
                    if (curStamina + SPRegenValue >= 0)              //�������� 0���� ũ�ٸ�,
                        Target.GetComponent<Stats>().curStamina += SPRegenValue;
                    else
                        Target.GetComponent<Stats>().curStamina += SPRegenValue;
                }
            }
        }
        else
        {
            if (HPRegenValue != 0)
            {
                float curHealth = Target.GetComponent<MobController>().curHealth;
                float maxHealth = Target.GetComponent<MobController>().maxHealth;
                if (HPRegenValue > 0)                               //+����� ���
                {
                    if (curHealth + HPRegenValue <= maxHealth)      //���� �ʰ������ �ƴ϶��
                    {
                        Target.GetComponent<MobController>().curHealth += HPRegenValue;                  //�������ŭ ����Ѵ�.
                    }
                    else                                            //�ʰ�����̶��,
                        Target.GetComponent<MobController>().curHealth += maxHealth - curHealth;         //�� ������ ��ŭ�� ����Ѵ�.
                }
                else                                                //-����� ���
                {
                    if (curHealth + HPRegenValue >= 0)              //�������� 0���� ũ�ٸ�,
                        Target.GetComponent<MobController>().curHealth += HPRegenValue;
                    else
                        Target.GetComponent<MobController>().curHealth += HPRegenValue;
                }
            }
        }
    }
}
