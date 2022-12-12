using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class DefaultParameter : MonoBehaviour
{
    public ItemParameterSO parameter;
    public TMP_Text name;
    public TMP_InputField inputField;
    public float value;

    public void SetParameter(ItemParameterSO parameter, float value)
    {
        this.parameter = parameter;
        name.text = ParameterName(this.parameter.ParameterCode);
        inputField.text = value.ToString();
        inputField.placeholder.GetComponent<TMP_Text>().text = value.ToString();
        this.value = value;
    }

    public void SetValue(TMP_InputField a)
    {
        if (float.TryParse(a.text, out value))
        {

        }
        else
        {
            inputField.text = 0.ToString();
            value = 0;
        }
    }

    public string ParameterName(int code)
    {
        switch (code)
        {
            case 0:
                {
                    return "ü��";
                }
            case 1:
                {
                    return "����";
                }
            case 2:
                {
                    return "���";
                }
            case 3:
                {
                    return "�ż�";
                }
            case 4:
                {
                    return "����";
                }
            case 5:
                {
                    return "���";
                }
            case 6:
                {
                    return "���� ����";
                }
            case 7:
                {
                    return "���׷��̵� Ƚ��";
                }
            case 8:
                {
                    return "����";
                }
            case 10:
                {
                    return "�ּҰ��ݷ�";
                }
            case 11:
                {
                    return "�ִ���ݷ�";
                }
            case 12:
                {
                    return "ġ��ŸȮ��";
                }
            case 13:
                {
                    return "ġ��Ÿ�����";
                }
            case 14:
                {
                    return "���ݼӵ�";
                }
            case 15:
                {
                    return "�������";
                }
            case 20:
                {
                    return "�ּ��ֹ���";
                }
            case 21:
                {
                    return "�ִ��ֹ���";
                }
            case 22:
                {
                    return "�ش�ȭȮ��";
                }
            case 23:
                {
                    return "�ش�ȭ�����";
                }
            case 24:
                {
                    return "�ֹ��ӵ�";
                }
            case 25:
                {
                    return "���װ����";
                }
            case 30:
                {
                    return "����";
                }
            case 31:
                {
                    return "���׷�";
                }
            case 32:
                {
                    return "ȸ��";
                }
            case 33:
                {
                    return "ȸ��";
                }
            case 34:
                {
                    return "�ټ�";
                }
            default:
                {
                    return "";
                }
        }
    }
}
