using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffManagement : MonoBehaviour
{
    //buff���� �Լ�(�ð�/ƽ)�� �޾ƿͼ� �ڷ�ƾ�� ����ؼ� �۵���Ų��.
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

    void AddBuff(BuffSO buff)                                               //������ ����Ʈ�� �߰��Ѵ�.
    {
        //�ߺ� ���� ����
        if (buffList.ContainsKey(buff.BuffCode))                            //���� �ڵ��� ������ �̹� �ִٸ�,
        {
            StopCoroutine(buffList[buff.BuffCode].Cor);                     //�ش� �ڵ��� �ڷ�ƾ�� �����Ű��,
            buffList.Remove(buff.BuffCode);                                 //���� ����Ʈ���� �����.
        }
        //���� ����
        buffList.Add(buff.BuffCode, buff);                                  //���� ����Ʈ�� �߰��ϰ�,
        buffList[buff.BuffCode].Cor = StartCoroutine(BuffActive(buff));     //�ش� �ڵ��� �ڷ�ƾ�� ���۽�Ų��.
    }

    IEnumerator BuffActive(BuffSO buff)
    {
        //Duration���� Tick���� ������ �����Ų��.
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