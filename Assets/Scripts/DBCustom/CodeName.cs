using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeName : MonoBehaviour
{
    public TMP_Text Code;
    public TMP_Text Name;
    public void SetCodeName(int code, string name)
    {
        Code.text = code.ToString();
        Name.text = name;
    }

    public void SetKey()
    {
        GameObject.Find("DB").GetComponent<DBCustomManager>().Key = int.Parse(Code.text);
    }
}
