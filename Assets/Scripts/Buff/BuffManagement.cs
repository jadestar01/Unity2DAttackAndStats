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
    //buff���� �Լ�(�ð�/ƽ)�� �޾ƿͼ� �ڷ�ƾ�� ����ؼ� �۵���Ų��.
    public GameObject buffSlotPrefab;
    [SerializeField] private List<ContentSizeFitter> csf;
    public RectTransform Buff;
    public RectTransform Debuff;
    private Dictionary<int, BuffData> buffList;
    public BuffSO buff1;
    public BuffSO buff2;

    private void Start()
    {
        buffList = new Dictionary<int, BuffData>();
    }

    private void Update()
    {
        //EndBuff();
    }

    public void Pain()
    {
        AddBuff(buff1, gameObject);
    }
    public void Poison()
    {
        AddBuff(buff2, gameObject);
    }

    void AddBuff(BuffSO buff, GameObject target)    //�ߺ��� �˻��Ͽ�, ������ ����Ʈ�� �߰��ϰ�, �۵���Ų��.
    {
        if (buffList.ContainsKey(buff.BuffCode))
        {
            //�ߺ��߻�
            Debug.Log(buff.Name + "�� �ߺ��Ǿ����ϴ�!");
            Destroy(buffList[buff.BuffCode].buffSlot);
            StopCoroutine(buffList[buff.BuffCode].buff.Cor);
            buffList.Remove(buff.BuffCode);
        }
        BuffData buffData = new BuffData();
        GameObject buffSlot = Instantiate(buffSlotPrefab, Vector2.zero, Quaternion.identity);
        if (buff.Type == BuffSO.BuffType.Buff)
            buffSlot.transform.SetParent(Buff);
        else if (buff.Type == BuffSO.BuffType.Debuff)
            buffSlot.transform.SetParent(Debuff);
        buffData.Setter(buff, target, buffSlot);
        buffList.Add(buff.BuffCode, buffData);
        buffList[buff.BuffCode].buff.Cor = StartCoroutine(buffList[buff.BuffCode].BuffActive());
    }

    public struct BuffData
    {
        public BuffSO buff;
        public BuffUI buffUI;
        public GameObject buffSlot;
        public GameObject target;
        float ticker;

        public void Setter(BuffSO buff, GameObject target, GameObject buffSlot)
        {
            this.buff = buff;
            this.target = target;
            this.buffSlot = buffSlot;
            buffUI = buffSlot.GetComponent<BuffUI>();
            ticker = 0;

            buffUI.SetBuff(buff.Image, buff.Name, buff.Description, buff.Duration);
        }

        public IEnumerator BuffActive()
        {
            while (buff.Duration > ticker)
            {
                yield return new WaitForSeconds(buff.Tick);
                buff.AffectTarget(target);
                ticker += buff.Tick;
            }
        }
    }
}