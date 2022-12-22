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
        SetText("�ִ� ü�� : ", player.health);
        SetText("�ִ� ���� : ", player.mana);
        SetText("�ִ� ��� : ", player.stamina);
        SetText("�̵� �ӵ� : ", player.speed, true);
        SetText("���� : ", player.haste, true);
        SetText("��� : ", player.strike);
        SetText("���� : ", player.vampirism);
        SetText("�ּ� ���� ���� : ", player.physicalMinDmg);
        SetText("�ִ� ���� ���� : ", player.physicalMaxDmg);
        SetText("ġ��Ÿ Ȯ�� : ", player.physicalCritRate, true);
        SetText("ġ��Ÿ ���� : ", player.physicalCritDmg, true);
        SetText("���� ����� : ", player.physicalPenetration, true);
        SetText("���� ���� �ӵ� : ", player.physicalAttackSpeed);
        SetText("�ּ� ���� ���� : ", player.magicalMinDmg);
        SetText("�ִ� ���� ���� : ", player.magicalMaxDmg);
        SetText("�ش�ȭ Ȯ�� : ", player.magicalCritRate, true);
        SetText("�ش�ȭ ���� : ", player.magicalCritDmg, true);
        SetText("���� ����� : ", player.magicalPenetration, true);
        SetText("���� ���� �ӵ� : ", player.magicalAttackSpeed);
        SetText("���� : ", player.armor);
        SetText("���׷� : ", player.registance);
        SetText("ȸ�� : ", player.dodge, true);
        SetText("�ټ� : ", player.grit, true);
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
