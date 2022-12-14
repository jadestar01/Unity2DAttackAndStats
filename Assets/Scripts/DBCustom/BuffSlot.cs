using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

public class BuffSlot : MonoBehaviour
{
    public BuffSO buff;
    public TMP_Text name;

    public void SetName(string name, BuffSO buff)
    {
        this.name.text = name;
        this.buff = buff;
    }

    public void SetBuff()
    {
        GameObject.Find("DB").GetComponent<DBCustomManager>().BuffAdd(buff);
    }
}
