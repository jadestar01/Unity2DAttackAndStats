using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public Dictionary<int, string> dic;
    // Start is called before the first frame update
    void Start()
    {
        dic = new Dictionary<int, string>();
        dic.Add(10, "A");
        dic.Add(20, "B");
        dic.Add(30, "C");
        StartCoroutine(a());
    }

    // Update is called once per frame
    void Update()
    {
        PrintDic();
    }

    void PrintDic()
    {
        foreach (KeyValuePair<int, string> data in dic)
        {
            Debug.Log(data.Key + " " + data.Value);
        }
    }

    IEnumerator a()
    {
        yield return new WaitForSeconds(5.0f);
        dic.Remove(10);
    }
}
