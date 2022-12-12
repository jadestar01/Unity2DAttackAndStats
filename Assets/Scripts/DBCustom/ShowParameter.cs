using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ShowParameter : MonoBehaviour
{
    public TMP_Dropdown parameterSelect;

    public List<ItemParameterSO> Utility = new List<ItemParameterSO>();
    public List<ItemParameterSO> Physical = new List<ItemParameterSO>();
    public List<ItemParameterSO> Magical = new List<ItemParameterSO>();
    public List<ItemParameterSO> Defense = new List<ItemParameterSO>();

    public GameObject parameterPanel;
    public GameObject parameter;

    public int currentValue;
    public int value;

    private void Start()
    {
        value = parameterSelect.value;
        currentValue = value;
        SetParameter();
    }

    private void Update()
    {
        value = parameterSelect.value;
        ValueChanged();
    }

    public void ValueChanged()
    {
        if (currentValue != value)
        {
            Debug.Log("Parameter Selector Changed!");
            currentValue = value;

            ResetParameter();
            SetParameter();
        }
    }

    public void ResetParameter()
    {
        for (int i = 0; i < parameterPanel.transform.childCount; i++)
        {
            Destroy(parameterPanel.transform.GetChild(parameterPanel.transform.childCount - 1 - i).gameObject);
        }
    }

    public void SetParameter()
    {
        if (value == 0)
        {
            for (int i = 0; i < Utility.Count; i++)
            {
                GameObject item = Instantiate(parameter);
                item.transform.SetParent(parameterPanel.transform);
                item.GetComponent<Parameter>().SetButton(Utility[i], ParameterName(Utility[i].ParameterCode));
            }
        }
        else if (value == 1)
        {
            for (int i = 0; i < Physical.Count; i++)
            {
                GameObject item = Instantiate(parameter);
                item.transform.SetParent(parameterPanel.transform);
                item.GetComponent<Parameter>().SetButton(Physical[i], ParameterName(Physical[i].ParameterCode));
            }
        }
        else if (value == 2)
        {
            for (int i = 0; i < Magical.Count; i++)
            {
                GameObject item = Instantiate(parameter);
                item.transform.SetParent(parameterPanel.transform);
                item.GetComponent<Parameter>().SetButton(Magical[i], ParameterName(Magical[i].ParameterCode));
            }
        }
        else if (value == 3)
        {
            for (int i = 0; i < Defense.Count; i++)
            {
                GameObject item = Instantiate(parameter);
                item.transform.SetParent(parameterPanel.transform);
                item.GetComponent<Parameter>().SetButton(Defense[i], ParameterName(Defense[i].ParameterCode));
            }
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
