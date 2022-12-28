using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BuffManagement;


public class MobController : MonoBehaviour
{
    public string monsterName = "";         //�̸�
    public float maxHealth = 100;           //�ִ�ü��
    public float curHealth = 100;           //����ü��
    public float speed = 5;                 //�ӵ�
    public float strike = 0;                //���
    public float vampirism = 0;             //����
    public float physicalMinDmg = 0;        //�ּ� ��������
    public float physicalMaxDmg = 0;        //�ִ� ��������
    public float physicalCritRate = 0;      //ġ��ŸȮ��
    public float physicalCritDmg = 0;       //ġ��Ÿ �����
    public float physicalPenetration = 0;   //���������
    public float physicalAttackSpeed = 0;   //���ݼӵ�
    public float magicalMinDmg = 0;         //�ּ� ��������
    public float magicalMaxDmg = 0;         //�ִ� ��������
    public float magicalCritRate = 0;       //�ش�ȭȮ��
    public float magicalCritDmg = 0;        //�ش�ȭ �����
    public float magicalPenetration = 0;    //���������
    public float magicalAttackSpeed = 0;    //�ֹ��ӵ�
    public float armor = 0;                 //����
    public float registance = 0;            //���׷�
    public float dodge = 0;                 //ȸ����
    public float grit = 0;                  //�ټ�
    public bool isDead = false;

    public Dictionary<int, BuffData> buffList;

    private void Start()
    {
        buffList = new Dictionary<int, BuffData>();
    }

    private void Update()
    {
        Die();
        EndSearcher();
    }

    [Button]
    public void AddBuff(BuffSO buff, GameObject main)
    {
        if (buffList.ContainsKey(buff.BuffCode))
        {
            //Debug.Log(buff.Name + "�� �ߺ��Ǿ����ϴ�!");
            StopCoroutine(buffList[buff.BuffCode].buff.Cor);
            buffList.Remove(buff.BuffCode);
        }

        BuffData buffData = new BuffData(buff, main, gameObject);

        buffList.Add(buff.BuffCode, buffData);
        buffList[buff.BuffCode].buff.Cor = StartCoroutine(buffList[buff.BuffCode].BuffActive());
    }

    void EndSearcher()
    {
        foreach (KeyValuePair<int, BuffData> buffData in buffList)
        {
            if (buffData.Value.isEnd)
            {
                //Debug.Log(buffData.Key + "����!");
                buffList.Remove(buffData.Key);
                break;
            }
        }
    }

    bool Die()
    {
        if (curHealth <= 0)
        {
            Debug.Log(monsterName + "����");
            isDead = true;
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
}
