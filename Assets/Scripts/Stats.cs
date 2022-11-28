using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Stats : MonoBehaviour
{
    public float health;
    public float mana;
    public float stamina;
    public float speed;
    public float haste;
    public float strike;
    public float physicalMinDmg;
    public float physicalMaxDmg;
    public float physicalCritRate;
    public float physicalCritDmg;
    public float physicalAttackSpeed;
    public float physicalPenetration;
    public float magicalMinDmg;
    public float magicalMaxDmg;
    public float magicalCritRate;
    public float magicalCritDmg;
    public float magicalAttackSpeed;
    public float magicalPenetration;
    public float armor;
    public float registance;
    public float dodge;
    public float grit;
    public float diminution;

    bool isItemChanged;

    int size = 8;
    [SerializeField] private InventorySO inventoryData;
    Dictionary<int, float> playerStat = new Dictionary<int, float>();
    InventoryItem[] temporaryList = new InventoryItem[8];       //주기적으로 아이템의 업데이트를 확인
    InventoryItem[] hotbarList = new InventoryItem[8];
    int[] slotNum = new int[8];
    int[] statNum = new int[23];

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateTemporary();
        ListCompare();
        ListCopy();
    }
    //파라미터를 읽어서 값들을 적용시킨다.

    void Init()
    {
        isItemChanged = false;
        slotNum[0] = 36;    //Melee
        slotNum[1] = 37;    //Magic
        slotNum[2] = 38;    //Range
        slotNum[3] = 41;    //Trinket
        slotNum[4] = 42;    //Trinket
        slotNum[5] = 43;    //Trinket
        slotNum[6] = 44;    //Trinket
        slotNum[7] = 45;    //Trinket

        statNum[0] = 0;         //Health
        statNum[1] = 1;         //Mana
        statNum[2] = 2;         //Stamina
        statNum[3] = 3;         //Speed
        statNum[4] = 4;         //Haste
        statNum[5] = 5;         //Strike
        statNum[6] = 10;        //PhysicalMinDmg
        statNum[7] = 11;        //PhysicalMaxDmg
        statNum[8] = 12;        //PhysicalCritRate
        statNum[9] = 13;        //PhysicalCritDmg
        statNum[10] = 14;       //PhysicalAttackSpeed
        statNum[11] = 15;       //PhysicalPenetration
        statNum[12] = 20;       //MagicalMinDmg
        statNum[13] = 21;       //MagicalMaxDmg
        statNum[14] = 22;       //MagicalCritRate
        statNum[15] = 23;       //MagicalCritDmg
        statNum[16] = 24;       //MagicalAttackSpeed
        statNum[17] = 25;       //MagicalPenetration
        statNum[18] = 30;       //Armor
        statNum[19] = 31;       //Registance
        statNum[20] = 32;       //Dodge
        statNum[21] = 33;       //Grit
        statNum[22] = 34;       //Diminution

        ResetStatList();
        initList();
    }

    void ResetStatList()
    {
        for (int i = 0; i < 23; i++)
        {
            playerStat[statNum[i]] = 0;
        }
    }

    void initList()
    {
        for (int i = 0; i < size; i++)
        {
            temporaryList[i] = inventoryData.GetItemAt(slotNum[i]);
            hotbarList[i] = inventoryData.GetItemAt(slotNum[i]);
        }
    }

    void UpdateTemporary()
    {
        for (int i = 0; i < size; i++)
        {
            temporaryList[i] = inventoryData.GetItemAt(slotNum[i]);
        }
    }

    void ListCompare()
    {
        for (int i = 0; i < size; i++)
        {
            //아이템의 빈 여부가 다르다면,
            if (hotbarList[i].IsEmpty && !temporaryList[i].IsEmpty)
            {
                isItemChanged = true;
                break;
            }
            if (!hotbarList[i].IsEmpty && temporaryList[i].IsEmpty)
            {
                isItemChanged = true;
                break;
            }
            //아이템이 서로 빈 칸이 아니라면,
            if (!hotbarList[i].IsEmpty && !temporaryList[i].IsEmpty)
            {
                //아이템의 이름이 다르다면,
                if (hotbarList[i].item.Name != temporaryList[i].item.Name)
                {
                    isItemChanged = true;
                    break;
                }
                //아이템이 이름이 같더라도, 내부 항목의 수가 다르다면,
                if (hotbarList[i].itemState.Count != temporaryList[i].itemState.Count)
                {
                    isItemChanged = true;
                    break;
                }
                //아이템 이름이 같고, 내부 항목의 수도 같지만, 내부 항목의 값이 다르다면,
                for (int j = 0; j < hotbarList[i].itemState.Count; j++)
                {
                    if (hotbarList[i].itemState[j].value != temporaryList[i].itemState[j].value)
                    {
                        isItemChanged = true;
                        break;
                    }
                    if (isItemChanged)
                        break;
                }
            }
        }
    }

    void ListCopy()
    {
        if (isItemChanged)
        {
            isItemChanged = false;
            for (int i = 0; i < size; i++)
            {
                hotbarList[i] = temporaryList[i];
            }
            ResetStat();
            UpdateStat();
            SetStat();
        }
    }

    void ResetStat()
    {
        for (int i = 0; i < 23; i++)
        {
            playerStat[statNum[i]] = 0;
        }
    }

    void UpdateStat()
    {
        for (int i = 0; i < size; i++)
        {
            //핫바리스트 순회
            for (int j = 0; j < hotbarList[i].itemState.Count; j++)
            {
                //핫바에 들어있는 아이템의 스테어터스를 가져와서 담는다.
                if (statNum.Contains(hotbarList[i].itemState[j].itemParameter.ParameterCode))
                {
                    playerStat[hotbarList[i].itemState[j].itemParameter.ParameterCode] += hotbarList[i].itemState[j].value;
                }
            }
        }
    }

    void SetStat()
    {
        health = playerStat[0];
        mana = playerStat[1];
        stamina = playerStat[2];
        speed = playerStat[3];
        haste = playerStat[4];
        strike = playerStat[5];
        physicalMinDmg = playerStat[10];
        physicalMaxDmg = playerStat[11];
        physicalCritRate = playerStat[12];
        physicalCritDmg = playerStat[13];
        physicalAttackSpeed = playerStat[14];
        physicalPenetration = playerStat[15];
        magicalMinDmg = playerStat[20];
        magicalMaxDmg = playerStat[21];
        magicalCritRate = playerStat[22];
        magicalCritDmg = playerStat[23];
        magicalAttackSpeed = playerStat[24];
        magicalPenetration = playerStat[25];
        armor = playerStat[30];
        registance = playerStat[31];
        dodge = playerStat[32];
        grit = playerStat[33];
        diminution = playerStat[34];
    }


    /*
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
        StatCheckAndUpdate();
    }

    void StatCheckAndUpdate()
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
    */
}
