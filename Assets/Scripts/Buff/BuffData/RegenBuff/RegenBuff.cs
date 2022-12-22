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
                if (HPRegenValue > 0)                               //+재생일 경우
                {
                    if (curHealth + HPRegenValue <= maxHealth)      //만약 초과재생이 아니라면
                    {
                        Target.GetComponent<Stats>().curHealth += HPRegenValue;                  //재생량만큼 재생한다.
                    }
                    else                                            //초과재생이라면,
                        Target.GetComponent<Stats>().curHealth += maxHealth - curHealth;         //그 격차량 만큼만 재생한다.
                }
                else                                                //-재생일 경우
                {
                    if (curHealth + HPRegenValue >= 0)              //빠진량이 0보다 크다면,
                        Target.GetComponent<Stats>().curHealth += HPRegenValue;
                    else
                        Target.GetComponent<Stats>().curHealth += HPRegenValue;
                }
            }

            if (MPRegenValue != 0)
            {
                float curMana = Target.GetComponent<Stats>().curMana;
                float maxMana = Target.GetComponent<Stats>().mana;
                if (MPRegenValue > 0)                               //+재생일 경우
                {
                    if (curMana + MPRegenValue <= maxMana)      //만약 초과재생이 아니라면
                        Target.GetComponent<Stats>().curMana += MPRegenValue;                  //재생량만큼 재생한다.
                    else                                            //초과재생이라면,
                        Target.GetComponent<Stats>().curMana += maxMana - curMana;         //그 격차량 만큼만 재생한다.
                }
                else                                                //-재생일 경우
                {
                    if (curMana + MPRegenValue >= 0)              //빠진량이 0보다 크다면,
                        Target.GetComponent<Stats>().curMana += MPRegenValue;
                    else
                        Target.GetComponent<Stats>().curMana += MPRegenValue;
                }
            }

            if (SPRegenValue != 0)
            {
                float curStamina = Target.GetComponent<Stats>().curStamina;
                float maxStamina = Target.GetComponent<Stats>().stamina;
                if (SPRegenValue > 0)                               //+재생일 경우
                {
                    if (curStamina + SPRegenValue <= maxStamina)      //만약 초과재생이 아니라면
                        Target.GetComponent<Stats>().curStamina += SPRegenValue;                  //재생량만큼 재생한다.
                    else                                            //초과재생이라면,
                        Target.GetComponent<Stats>().curStamina += maxStamina - curStamina;         //그 격차량 만큼만 재생한다.
                }
                else                                                //-재생일 경우
                {
                    if (curStamina + SPRegenValue >= 0)              //빠진량이 0보다 크다면,
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
                if (HPRegenValue > 0)                               //+재생일 경우
                {
                    if (curHealth + HPRegenValue <= maxHealth)      //만약 초과재생이 아니라면
                    {
                        Target.GetComponent<MobController>().curHealth += HPRegenValue;                  //재생량만큼 재생한다.
                    }
                    else                                            //초과재생이라면,
                        Target.GetComponent<MobController>().curHealth += maxHealth - curHealth;         //그 격차량 만큼만 재생한다.
                }
                else                                                //-재생일 경우
                {
                    if (curHealth + HPRegenValue >= 0)              //빠진량이 0보다 크다면,
                        Target.GetComponent<MobController>().curHealth += HPRegenValue;
                    else
                        Target.GetComponent<MobController>().curHealth += HPRegenValue;
                }
            }
        }
    }
}
