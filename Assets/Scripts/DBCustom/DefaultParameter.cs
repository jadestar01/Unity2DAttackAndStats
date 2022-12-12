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
                    return "체력";
                }
            case 1:
                {
                    return "마나";
                }
            case 2:
                {
                    return "기력";
                }
            case 3:
                {
                    return "신속";
                }
            case 4:
                {
                    return "가속";
                }
            case 5:
                {
                    return "충격";
                }
            case 6:
                {
                    return "레벨 제한";
                }
            case 7:
                {
                    return "업그레이드 횟수";
                }
            case 8:
                {
                    return "흡혈";
                }
            case 10:
                {
                    return "최소공격력";
                }
            case 11:
                {
                    return "최대공격력";
                }
            case 12:
                {
                    return "치명타확률";
                }
            case 13:
                {
                    return "치명타대미지";
                }
            case 14:
                {
                    return "공격속도";
                }
            case 15:
                {
                    return "방어관통력";
                }
            case 20:
                {
                    return "최소주문력";
                }
            case 21:
                {
                    return "최대주문력";
                }
            case 22:
                {
                    return "극대화확률";
                }
            case 23:
                {
                    return "극대화대미지";
                }
            case 24:
                {
                    return "주문속도";
                }
            case 25:
                {
                    return "저항관통력";
                }
            case 30:
                {
                    return "방어력";
                }
            case 31:
                {
                    return "저항력";
                }
            case 32:
                {
                    return "회피";
                }
            case 33:
                {
                    return "회피";
                }
            case 34:
                {
                    return "근성";
                }
            default:
                {
                    return "";
                }
        }
    }
}
