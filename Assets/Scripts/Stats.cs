using Inventory.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static BuffManagement;

public class Stats : MonoBehaviour
{
    public float initHealth;
    public float initMana;
    public float initStamina;
    public float curHealth;
    public float curMana;
    public float curStamina;
    public float health;
    public float mana;
    public float stamina;
    public float speed;
    public float haste;
    public float strike;
    public float vampirism;
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

    public bool isItemChanged;
    public bool isReadyToUpgrade;
    int size = 8;

    [SerializeField] private InventorySO inventoryData;

    [SerializeField] private BuffManagement buffManagement;

    [SerializeField] private AgentWeapon agentWeapon;

    [SerializeField] private Dictionary<int, BuffData> temporaryData = new Dictionary<int, BuffData>();  //주기적으로 버프의 업데이트를 확인
    [SerializeField] private Dictionary<int, BuffData> buffData = new Dictionary<int, BuffData>();
    float[] buffStat = new float[50];

    Dictionary<int, float> playerStat = new Dictionary<int, float>();

    InventoryItem[] temporaryList = new InventoryItem[8];               //주기적으로 아이템의 업데이트를 확인
    InventoryItem[] hotbarList = new InventoryItem[8];
    int[] slotNum = new int[8];
    int[] statNum = new int[24];

    [SerializeField] public List<RectTransform> HPbarArr = new List<RectTransform>();
    [SerializeField] public List<RectTransform> MPbarArr = new List<RectTransform>();
    [SerializeField] public List<RectTransform> SPbarArr = new List<RectTransform>();

    private void Start()
    {
        Init();
        SetStat();
    }

    private void Update()
    {
        UpdateTemporary();
        ListCompare();
        ListCopy();
        BuffDataReader();

        BarController();
        ResourceException();
    }

    void BarController()
    {
        HPbarArr[0].sizeDelta = new Vector2(health * 2.35f, HPbarArr[0].rect.height);                           //Back
        HPbarArr[1].sizeDelta = new Vector2(health * 2.35f * curHealth / health, HPbarArr[1].rect.height);      //CurHP
        HPbarArr[2].sizeDelta = new Vector2(health * 2.35f + 1.6f * 2.35f, HPbarArr[2].rect.height);            //Contour

        MPbarArr[0].sizeDelta = new Vector2(mana * 2.35f, MPbarArr[0].rect.height);                             //Back
        MPbarArr[1].sizeDelta = new Vector2(mana * 2.35f * curMana / mana, MPbarArr[1].rect.height);            //CurHP
        MPbarArr[2].sizeDelta = new Vector2(mana * 2.35f + 1.6f * 2.35f, MPbarArr[2].rect.height);              //Contour

        SPbarArr[0].sizeDelta = new Vector2(stamina * 2.35f, SPbarArr[0].rect.height);                          //Back
        SPbarArr[1].sizeDelta = new Vector2(stamina * 2.35f * curStamina / stamina, SPbarArr[1].rect.height);   //CurHP
        SPbarArr[2].sizeDelta = new Vector2(stamina * 2.35f + 1.6f * 2.35f, SPbarArr[2].rect.height);           //Contour
    }

    void ResourceException()
    {
        if(curHealth > health)
            curHealth = health;
        if(curMana > mana)
            curMana = mana;
        if(curStamina > stamina)
            curStamina = stamina;

        if (curHealth <= 0)
            Debug.Log("당신은 죽었습니다!");
    }

    void Init()
    {
        curHealth = initHealth;
        curMana = initMana;
        curStamina = initStamina;

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
        statNum[6] = 8;         //Vampirism
        statNum[7] = 10;        //PhysicalMinDmg
        statNum[8] = 11;        //PhysicalMaxDmg
        statNum[9] = 12;        //PhysicalCritRate
        statNum[10] = 13;       //PhysicalCritDmg
        statNum[11] = 14;       //PhysicalAttackSpeed
        statNum[12] = 15;       //PhysicalPenetration
        statNum[13] = 20;       //MagicalMinDmg
        statNum[14] = 21;       //MagicalMaxDmg
        statNum[15] = 22;       //MagicalCritRate
        statNum[16] = 23;       //MagicalCritDmg
        statNum[17] = 24;       //MagicalAttackSpeed
        statNum[18] = 25;       //MagicalPenetration
        statNum[19] = 30;       //Armor
        statNum[20] = 31;       //Registance
        statNum[21] = 32;       //Dodge
        statNum[22] = 33;       //Grit
        statNum[23] = 34;       //Diminution

        ResetStatList();
        initList();
    }

    void ResetStatList()
    {
        for (int i = 0; i < 24; i++)
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

    public void ListCopy()
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
            if (i == 0 && agentWeapon.currentWeapon != 0) continue;
            else if (i == 1 && agentWeapon.currentWeapon != 1) continue;
            else if (i == 2 && agentWeapon.currentWeapon != 2) continue;
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
        health = playerStat[0] + buffStat[0] + initHealth;
        mana = playerStat[1] + buffStat[1] + initMana;
        stamina = playerStat[2] + buffStat[2] + initStamina;
        speed = playerStat[3] + buffStat[3];
        haste = playerStat[4] + buffStat[4];
        strike = playerStat[5] + buffStat[5];
        vampirism = playerStat[8] + buffStat[8];
        physicalMinDmg = playerStat[10] + buffStat[10];
        physicalMaxDmg = playerStat[11] + buffStat[11];
        physicalCritRate = playerStat[12] + buffStat[12];
        physicalCritDmg = playerStat[13] + buffStat[13];
        physicalAttackSpeed = playerStat[14] + buffStat[14];
        physicalPenetration = playerStat[15] + buffStat[15];
        magicalMinDmg = playerStat[20] + buffStat[20];
        magicalMaxDmg = playerStat[21] + buffStat[21];
        magicalCritRate = playerStat[22] + buffStat[22];
        magicalCritDmg = playerStat[23] + buffStat[23];
        magicalAttackSpeed = playerStat[24] + buffStat[24];
        magicalPenetration = playerStat[25] + buffStat[25];
        armor = playerStat[30] + buffStat[30];
        registance = playerStat[31] + buffStat[31];
        dodge = playerStat[32] + buffStat[32];
        grit = playerStat[33] + buffStat[33];
        diminution = playerStat[34] + buffStat[34];
    }

    void BuffDataReader()
    {
        temporaryData = buffManagement.buffList;        //버프 데이터를 임시저장소에 저장한다.
        if (!(buffData.Count == temporaryData.Count && !buffData.Except(temporaryData).Any())) //개수가 같고, 차집합이 없다면 둘은 같다는 의미다. 이에 역이니 다르다면이 된다.
        {
            buffData.Clear();
            //Debug.Log("버프 데이터 갱신");
            foreach (KeyValuePair<int, BuffData> bd in temporaryData)
            {
                buffData.Add(bd.Key, bd.Value);
            }

            BuffParameterUpdate();
            SetStat();
        }
    }

    void BuffParameterUpdate()
    {
        for (int i = 0; i < 50; i++)
            buffStat[i] = 0;
        foreach (KeyValuePair<int, BuffData> buff in buffData)
        {
            if (!buff.Value.buff.isTicking)
            {
                //Debug.Log("버프 확인! " + buff.Value.buff.Name);
                StatBuffSO statBuff = (StatBuffSO)buff.Value.buff;
                foreach (ItemParameter stat in statBuff.stat)
                {
                    //Debug.Log(stat.itemParameter.ParameterCode + "에 " + stat.value + "추가");
                    buffStat[stat.itemParameter.ParameterCode] += stat.value;
                }
            }
        }
    }
}