using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BuffManagement : MonoBehaviour
{
    //buff에서 함수(시간/틱)을 받아와서 코루틴을 등록해서 작동시킨다.
    public GameObject buffSlotPrefab;
    public RectTransform Buff;
    public RectTransform Debuff;
    public Dictionary<int, BuffData> buffList;
    public BuffSO buff1;
    public BuffSO buff2;
    public BuffSO buff3;
    public BuffSO buff4;
    public BuffSO buff5;
    public GameObject buffTooltip;

    private void Start()
    {
        buffList = new Dictionary<int, BuffData>();
    }

    private void Update()
    {
        EndSearcher();
    }

    public void Pain()
    {
        AddBuff(buff1, gameObject);
    }
    public void Poison()
    {
        AddBuff(buff2, gameObject);
    }

    public void HPup()
    {
        AddBuff(buff3, gameObject);
    }
    public void DMGup()
    {
        AddBuff(buff4, gameObject);
    }

    public void HPRegen()
    {
        AddBuff(buff5, gameObject);
    }


    void AddBuff(BuffSO buff, GameObject target)    //중복을 검사하여, 버프를 리스트에 추가하고, 작동시킨다.
    {
        if (buffList.ContainsKey(buff.BuffCode))
        {
            //Debug.Log(buff.Name + "은 중복되었습니다!");
            Destroy(buffList[buff.BuffCode].buffSlot);
            StopCoroutine(buffList[buff.BuffCode].buff.Cor);
            buffList.Remove(buff.BuffCode);
        }
        GameObject buffSlot = Instantiate(buffSlotPrefab, Vector2.zero, Quaternion.identity);
        BuffData buffData = new BuffData(buff, target, buffSlot);

        if (buff.Type == BuffSO.BuffType.Buff)
            buffSlot.transform.SetParent(Buff);
        else if (buff.Type == BuffSO.BuffType.Debuff)
            buffSlot.transform.SetParent(Debuff);

        buffList.Add(buff.BuffCode, buffData);
        buffList[buff.BuffCode].buff.Cor = StartCoroutine(buffList[buff.BuffCode].BuffActive());
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)Buff.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)Debuff.transform);
    }

    void EndSearcher()
    {
        foreach (KeyValuePair<int, BuffData> buffData in buffList)
        {
            if (buffData.Value.isEnd)
            {
                //Debug.Log(buffData.Key + "만료!");
                buffList.Remove(buffData.Key);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)Buff.transform);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)Debuff.transform);
                break;
            }
        }
    }

    public void TooltipActive() { buffTooltip.SetActive(true); }

    public void TooltipInactive(){ buffTooltip.SetActive(false); }

    //struct : 구조체 (변수 뭉탱이)             -> 값만 전달
    //class : 구조체 상위호환 (변수 뭉탱이)      -> 존재 자체를 전달
    public class BuffData
    {
        public BuffSO buff;
        public BuffUI buffUI;
        public GameObject buffSlot;
        public GameObject target;
        public float ticker;
        public bool isEnd;

        public BuffData(BuffSO buff, GameObject target, GameObject buffSlot)
        {
            isEnd = false;
            this.buff = buff;
            this.target = target;
            this.buffSlot = buffSlot;
            buffUI = buffSlot.GetComponent<BuffUI>();
            ticker = 0;

            buffUI.SetBuff(buff.Image, buff.Name, buff.Description, buff.Duration);
        }

        public void Setter(BuffSO buff, GameObject target, GameObject buffSlot)
        {
            isEnd = false;
            this.buff = buff;
            this.target = target;
            this.buffSlot = buffSlot;
            buffUI = buffSlot.GetComponent<BuffUI>();
            ticker = 0;

            buffUI.SetBuff(buff.Image, buff.Name, buff.Description, buff.Duration);
        }

        public IEnumerator BuffActive()
        {
            if (buff.isTicking)
            {
                while (buff.Duration > ticker)
                {
                    yield return new WaitForSeconds(buff.Tick);
                    buff.AffectTarget(target);
                    ticker += buff.Tick;
                }
            }
            else
            {
                buff.AffectTarget(target);
                while (buff.Duration > ticker)
                {
                    yield return new WaitForSeconds(0.1f);
                    ticker += 0.1f;
                }
            }
            isEnd = true;
        }
    }
}