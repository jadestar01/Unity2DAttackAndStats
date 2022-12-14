using BansheeGz.BGDatabase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffList : MonoBehaviour
{
    public List<BuffSO> buffList = new List<BuffSO>();
    public GameObject buffPanel;
    public GameObject buffSlot;

    private void Start()
    {
        BuffListSetting();
    }

    void BuffListSetting()
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            GameObject item = Instantiate(buffSlot, Vector2.zero, Quaternion.identity);
            item.transform.SetParent(buffPanel.transform);
            item.GetComponent<BuffSlot>().SetName(buffList[i].Name, buffList[i]);
        }
    }
}
