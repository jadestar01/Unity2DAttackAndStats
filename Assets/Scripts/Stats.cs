using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Stats : MonoBehaviour
{
    enum stat { STR, INT, DEX };
    [Header("Skill Stats")]
    public int initSTR;
    public int initINT;
    public int initDEX;
    [Space]

    public Stat[] statArr = new Stat[3];

    public int health;
    public int mana;
    public int focus;

    public int physicalForce;
    public int magicalForce;

    public class Stat
    {
        int stat;
        int preStat;

        int health;
        int mana;
        int focus;
        int physicalForce;
        int magicalForce;

        public Stat(int stat, int health, int mana, int focus, int physicalForce, int magicalForce)
        { 
            this.stat = stat;
            preStat = stat;
            this.health = health;
            this.mana = mana;
            this.focus = focus;
            this.physicalForce = physicalForce;
            this.magicalForce = magicalForce;
        }
        public bool StatUpdate() //스텟을 업데이트하고, 스텟 업데이트를 체크한다.
        {
            if (preStat == stat)
                return false;
            else { preStat = stat; return true; }
        }

        public int GetStat()
        {
            return stat;
        }

        public void SetStat(int set)
        {
            stat = set;
        }

        public void AddStat(int add)
        {
            stat += add;
        }

        public int GetHealth()
        { return health; }
        public int GetMana()
        { return mana; }
        public int GetFocus()
        { return focus; }
        public int GetPhysicalForce()
        { return physicalForce; }
        public int GetMagicalForce()
        { return magicalForce; }
    }

    void Start()
    {
        //stat health mana focus phsical magical
        statArr[(int)stat.STR] = new Stat(initSTR, 7, 0, 1, 5, 0);
        statArr[(int)stat.INT] = new Stat(initINT, 2, 3, 0, 0, 8);
        statArr[(int)stat.DEX] = new Stat(initDEX, 2, 0, 3, 8, 0);

        for (int i = 0; i < statArr.Length; i++)
        {
            health += statArr[i].GetHealth() * statArr[i].GetStat();
            mana += statArr[i].GetMana() * statArr[i].GetStat();
            focus += statArr[i].GetFocus() * statArr[i].GetStat();
            physicalForce += statArr[i].GetPhysicalForce() * statArr[i].GetStat();
            magicalForce += statArr[i].GetMagicalForce() * statArr[i].GetStat();
        }
    }

    void Update()
    {
        StatUpdate();
    }

    void StatUpdate()
    {
        bool isUpdated = false;

        for (int i = 0; i < statArr.Length; i++)
            if (statArr[i].StatUpdate()) isUpdated = true;

        if (isUpdated)
        {
            health = 0; mana = 0; focus = 0; physicalForce = 0; magicalForce = 0;
            for (int i = 0; i < statArr.Length; i++)
            {
                health += statArr[i].GetHealth() * statArr[i].GetStat();
                mana += statArr[i].GetMana() * statArr[i].GetStat();
                focus += statArr[i].GetFocus() * statArr[i].GetStat();
                physicalForce += statArr[i].GetPhysicalForce() * statArr[i].GetStat();
                magicalForce += statArr[i].GetMagicalForce() * statArr[i].GetStat();
            }
        }
    }
}
