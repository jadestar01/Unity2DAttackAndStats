using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    public GameObject UI;
    bool isUIOpen;
    public GameObject character;
    enum stat { STR, INT, DEX };
    void Start()
    {
        isUIOpen = false;
    }

    void Update()
    {
        UI.SetActive(isUIOpen);
        if (isUIOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape)) isUIOpen = false;
        }
        else if (!isUIOpen)
        { 
            if (Input.GetKeyDown(KeyCode.Tab)) isUIOpen = true;
        }
    }

    public void STRUp()     //publicÀ» ¾²ÀÚ!
    {
        character.GetComponent<Stats>().statArr[(int)stat.STR].AddStat(1);
    }

    public void INTUp()
    {
        character.GetComponent<Stats>().statArr[(int)stat.INT].AddStat(1);
    }

    public void DEXUp()
    {
        character.GetComponent<Stats>().statArr[(int)stat.DEX].AddStat(1);
    }
}
