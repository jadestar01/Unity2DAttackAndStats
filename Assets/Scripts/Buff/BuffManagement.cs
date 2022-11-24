using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffManagement : MonoBehaviour
{
    //buff에서 함수(시간/틱)을 받아와서 코루틴을 등록해서 작동시킨다.
    public GameObject buffSlotPrefab;
    public RectTransform Buff;
    public RectTransform Debuff;
    private Dictionary<int, BuffSO> buffList;
    private Dictionary<int, GameObject> buffSlotList;
    public BuffSO buff1;
    public BuffSO buff2;

    private void Start()
    {
        buffList = new Dictionary<int, BuffSO>();
    }

    private void Update()
    {
        foreach (KeyValuePair<int, BuffSO> buff in buffList)
        {
            Debug.Log(buff.Value.Name);
        }
    }

    public void Pain()
    {
        AddBuff(buff1);
    }
    public void Poison()
    {
        AddBuff(buff2);
    }

    void AddBuff(BuffSO buff)                                               //버프를 리스트에 추가한다.
    {
        //중복 버프 제거
        if (buffList.ContainsKey(buff.BuffCode))                            //같은 코드의 버프가 이미 있다면,
        {
            StopCoroutine(buffList[buff.BuffCode].Cor);                     //해당 코드의 코루틴을 종료시키고,
            buffList.Remove(buff.BuffCode);                                 //버프 리스트에서 지운다.
        }
        //버프 적용
        buffList.Add(buff.BuffCode, buff);                                  //버프 리스트에 추가하고,
        buffList[buff.BuffCode].Cor = StartCoroutine(BuffActive(buff));     //해당 코드의 코루틴을 시작시킨다.
    }

    IEnumerator BuffActive(BuffSO buff)
    {
        //Duration동안 Tick마다 버프를 적용시킨다.
        float ticker = 0.0f;
        while (buff.Duration > ticker)
        {
            buff.AffectTarget(gameObject);
            yield return new WaitForSeconds(buff.Tick);
            ticker += buff.Tick;
        }
        buffList.Remove(buff.BuffCode);
    }

    GameObject BuffSlotStarter(BuffSO buff)
    {
        GameObject buffObject = Instantiate(buffSlotPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        buffObject.GetComponent<BuffUI>().SetBuff(buff.Image, buff.Name, buff.Description, buff.Duration);

        if (buff.Type == BuffSO.BuffType.Buff)
            buffObject.transform.SetParent(Buff);
        else
            buffObject.transform.SetParent(Debuff);
        return buffObject;
    }

    public struct BuffSlot
    {
        int code;
        string name;
        string description;
        float duration;
        Sprite Image;
    }
}