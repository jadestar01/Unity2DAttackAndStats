using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BuffManagement;


public class MobController : MonoBehaviour
{
    public string monsterName = "";         //이름
    public float maxHealth = 100;           //최대체력
    public float curHealth = 100;           //현재체력
    public float speed = 5;                 //속도
    public float strike = 0;                //충격
    public float vampirism = 0;             //흡혈
    public float physicalMinDmg = 0;        //최소 물리피해
    public float physicalMaxDmg = 0;        //최대 물리피해
    public float physicalCritRate = 0;      //치명타확률
    public float physicalCritDmg = 0;       //치명타 대미지
    public float physicalPenetration = 0;   //물리관통력
    public float physicalAttackSpeed = 0;   //공격속도
    public float magicalMinDmg = 0;         //최소 마법피해
    public float magicalMaxDmg = 0;         //최대 마법피해
    public float magicalCritRate = 0;       //극대화확률
    public float magicalCritDmg = 0;        //극대화 대미지
    public float magicalPenetration = 0;    //마법관통력
    public float magicalAttackSpeed = 0;    //주문속도
    public float armor = 0;                 //방어력
    public float registance = 0;            //저항력
    public float dodge = 0;                 //회피율
    public float grit = 0;                  //근성
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
            //Debug.Log(buff.Name + "은 중복되었습니다!");
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
                //Debug.Log(buffData.Key + "만료!");
                buffList.Remove(buffData.Key);
                break;
            }
        }
    }

    bool Die()
    {
        if (curHealth <= 0)
        {
            Debug.Log(monsterName + "죽음");
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
