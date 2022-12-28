using BansheeGz.BGDatabase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffList : MonoBehaviour
{
    public BuffSO[] buffs;
    public GameObject buffPanel;
    public GameObject buffSlot;

    private void Start()
    {
        buffs = Resources.LoadAll<BuffSO>("Buffs");
        BuffListSetting();
    }

    void BuffListSetting()
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            GameObject item = Instantiate(buffSlot, Vector2.zero, Quaternion.identity);
            item.transform.SetParent(buffPanel.transform);
            item.GetComponent<BuffSlot>().SetName(buffs[i].Name, buffs[i]);
        }
    }
}
