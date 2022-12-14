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
                    return "????";
                }
            case 1:
                {
                    return "????";
                }
            case 2:
                {
                    return "????";
                }
            case 3:
                {
                    return "????";
                }
            case 4:
                {
                    return "????";
                }
            case 5:
                {
                    return "????";
                }
            case 6:
                {
                    return "???? ????";
                }
            case 7:
                {
                    return "?????????? ????";
                }
            case 8:
                {
                    return "????";
                }
            case 10:
                {
                    return "??????????";
                }
            case 11:
                {
                    return "??????????";
                }
            case 12:
                {
                    return "??????????";
                }
            case 13:
                {
                    return "????????????";
                }
            case 14:
                {
                    return "????????";
                }
            case 15:
                {
                    return "??????????";
                }
            case 20:
                {
                    return "??????????";
                }
            case 21:
                {
                    return "??????????";
                }
            case 22:
                {
                    return "??????????";
                }
            case 23:
                {
                    return "????????????";
                }
            case 24:
                {
                    return "????????";
                }
            case 25:
                {
                    return "??????????";
                }
            case 30:
                {
                    return "??????";
                }
            case 31:
                {
                    return "??????";
                }
            case 32:
                {
                    return "????";
                }
            case 33:
                {
                    return "????";
                }
            case 34:
                {
                    return "????";
                }
            default:
                {
                    return "";
                }
        }
    }
}
