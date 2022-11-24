using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    /*
    public GameObject character;
    [Header("Resource Stats")]
    public int curHP = 10;
    public int maxHP;
    public Slider HPbar;
    public RectTransform HPrt;
    public RectTransform HPbarContour;
    Coroutine HPregen;
    public int curMP = 10;
    public int maxMP;
    public Slider MPbar;
    public RectTransform MPrt;
    public RectTransform MPbarContour;
    Coroutine MPregen;
    public int curFP = 10;
    public int maxFP ;
    public Slider FPbar;
    public RectTransform FPrt;
    public RectTransform FPbarContour;
    Coroutine FPregen;

    int health;
    int mana;
    int focus;
    int physicalForce;
    int magicalForce;
    bool isStatChanged;

    void Start()
    {
        isStatChanged = false;
        maxHP = 0; maxMP = 0; maxFP = 0; health = 0; mana = 0; focus = 0;
        StatChangeCheck();
        StatUpdate();

        HPregen = StartCoroutine(HPRegen(5, 5));
        MPregen = StartCoroutine(MPRegen(0.5f, 1));
        FPregen = StartCoroutine(FPRegen(5, 10));
    }

    void Update()
    {
        ResourceUIUpdate();
        StatChangeCheck();
        StatUpdate();
    }

    public bool AddHP(int val)
    {
        if (curHP == maxHP)
            return false;
        else
        {
            if (curHP + val < maxHP)
                curHP += val;
            else
                curHP = maxHP;
            return true;
        }
    }

    void ResourceUIUpdate()
    {
        HPrt.sizeDelta = new Vector2(maxHP * 5, HPrt.rect.height);
        MPrt.sizeDelta = new Vector2(maxMP * 5, MPrt.rect.height);
        FPrt.sizeDelta = new Vector2(maxFP * 5, FPrt.rect.height);

        HPbarContour.sizeDelta = new Vector2(maxHP * 5 + 40, HPbarContour.rect.height);
        MPbarContour.sizeDelta = new Vector2(maxMP * 5 + 40, MPbarContour.rect.height);
        FPbarContour.sizeDelta = new Vector2(maxFP * 5 + 40, FPbarContour.rect.height);

        HPbar.value = (float)curHP / (float)maxHP;
        MPbar.value = (float)curMP / (float)maxMP;
        FPbar.value = (float)curFP / (float)maxFP;
    }

    void StatUpdate()
    {
        if (isStatChanged)
        {
            isStatChanged = false;
            maxHP = health;
            maxMP = mana;
            maxFP = focus;
        }
    }

    void StatChangeCheck()
    {
        if (character.GetComponent<Stats>().health != health) { health = character.GetComponent<Stats>().health; isStatChanged = true; }
        if (character.GetComponent<Stats>().mana != mana) { mana = character.GetComponent<Stats>().mana; isStatChanged = true; }
        if (character.GetComponent<Stats>().focus != focus) { focus = character.GetComponent<Stats>().focus; isStatChanged = true; }
        if (character.GetComponent<Stats>().physicalForce != physicalForce) { physicalForce = character.GetComponent<Stats>().physicalForce; isStatChanged = true; }
        if (character.GetComponent<Stats>().magicalForce != magicalForce) { magicalForce = character.GetComponent<Stats>().magicalForce; isStatChanged = true; }
    }

    IEnumerator HPRegen(float time, int amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (curHP >= maxHP) { curHP = maxHP; }
            else
            {
                if (maxHP - curHP > amount) { curHP += amount; }
                else curHP = maxHP;
            }
        }
    }
    IEnumerator MPRegen(float time, int amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (curMP >= maxMP) { curMP = maxMP; }
            else
            {
                if (maxMP - curMP > amount) { curMP += amount; }
                else curMP = maxMP;
            }
        }
    }
    IEnumerator FPRegen(float time, int amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (curFP >= maxFP) { curFP = maxFP; }
            else
            {
                if (maxFP - curFP > amount) { curFP += amount; }
                else curFP = maxFP;
            }
        }
    }
    */
}
