using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    public Stats player;
    public GameObject textBox;
    public GameObject textPannel;


    public void SetStatUI()
    {
        for (int i = 0; i < textPannel.transform.childCount; i++)
        {
            Destroy(textPannel.transform.GetChild(textPannel.transform.childCount - i - 1).gameObject);
        }
        SetText("최대 체력 : ", player.health);
        SetText("최대 마나 : ", player.mana);
        SetText("최대 기력 : ", player.stamina);
        SetText("이동 속도 : ", player.speed, true);
        SetText("가속 : ", player.haste, true);
        SetText("충격 : ", player.strike);
        SetText("흡혈 : ", player.vampirism);
        SetText("최소 물리 피해 : ", player.physicalMinDmg);
        SetText("최대 물리 피해 : ", player.physicalMaxDmg);
        SetText("치명타 확률 : ", player.physicalCritRate, true);
        SetText("치명타 배율 : ", player.physicalCritDmg, true);
        SetText("물리 관통력 : ", player.physicalPenetration, true);
        SetText("물리 공격 속도 : ", player.physicalAttackSpeed);
        SetText("최소 마법 피해 : ", player.magicalMinDmg);
        SetText("최대 마법 피해 : ", player.magicalMaxDmg);
        SetText("극대화 확률 : ", player.magicalCritRate, true);
        SetText("극대화 배율 : ", player.magicalCritDmg, true);
        SetText("마법 관통력 : ", player.magicalPenetration, true);
        SetText("마법 공격 속도 : ", player.magicalAttackSpeed);
        SetText("방어력 : ", player.armor);
        SetText("저항력 : ", player.registance);
        SetText("회피 : ", player.dodge, true);
        SetText("근성 : ", player.grit, true);
    }

    void SetText(string name, float value, bool isPercentage = false)
    {
        GameObject text = Instantiate(textBox);
        text.transform.SetParent(textPannel.transform);
        if (isPercentage)
        {
            text.GetComponent<TMP_Text>().text = name + " : " + value + "%";
        }
        else
        {
            text.GetComponent<TMP_Text>().text = name + " : " + value;
        }
    }
}
