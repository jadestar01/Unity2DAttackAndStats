using Inventory.Model;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Parameter : MonoBehaviour
{
    public TMP_Text Name;
    public ItemParameterSO parameter;

    public void SetButton(ItemParameterSO p, string n)
    {
        parameter = p;
        Name.text = n;
    }

    public void ParameterTranslate()
    {
        GameObject.Find("DB").GetComponent<DBCustomManager>().ParameterController(parameter);
    }
}
