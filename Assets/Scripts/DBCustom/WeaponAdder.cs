using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAdder : MonoBehaviour
{
    public GameObject weapon;

    public void WeaponAdd()
    {
        if (GameObject.Find("DB").GetComponent<DBCustomManager>().DB == 0)
        {
            GameObject.Find("DB").GetComponent<DBCustomManager>().equipItem.weapon = weapon;
            GameObject.Find("DB").GetComponent<DBCustomManager>().E_itemWeapon.text = weapon.name;
        }
    }
}
