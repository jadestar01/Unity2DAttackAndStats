using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Resource Stats")]
    public int curHP = 10;
    public int maxHP = 50;
    Coroutine HPregen;
    public int curMP = 10;
    public int maxMP = 50;
    Coroutine MPregen;
    public int curFP = 10;
    public int maxFp = 50;
    Coroutine FPregen;
    [Space]

    [Header("Skill Stats")]
    public int STR = 10;
    public int INT = 10;
    public int DEX = 10;

    void Start()
    {
        HPRegen(); //MPRegen(); FPRegen();
    }

    void Update()
    {
        //Debug.Log("curHP : " + curHP + " curMP : " + curMP + " curFP : " + curFP);
    }

    void HPRegen()
    {
        HPregen = StartCoroutine(RegenCycle(curHP, maxHP, 5, 5));
    }

    void MPRegen()
    {
        MPregen = StartCoroutine(RegenCycle(curHP, maxHP, 0.5f, 1));
    }

    void FPRegen()
    {
        FPregen = StartCoroutine(RegenCycle(curFP, maxHP, 5, 10));
    }

    IEnumerator RegenCycle(int cur, int max, float time, int amount)
    {
        while(true)
        {
            yield return new WaitForSeconds(time);
            if (cur >= max) { cur = max; }
            else
            {
                if (max - cur > amount) { cur += amount; Debug.Log("update : " + (cur+amount) + " curHP : " +curHP); }
                else cur = max;
            }
        }
    }
}
