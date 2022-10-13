using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManagement : MonoBehaviour
{
    [SerializeField] private MeleeWeapon melee;
    [SerializeField] private MagicWeapon magic;
    [SerializeField] private RangeWeapon range;
    public Dictionary<int, Weapon.WeaponClass> weapon = new Dictionary<int, Weapon.WeaponClass>();

    private void Start()
    {
        //Melee
        for (int i = 0; i < melee.meleeWeaponList.Count; i++)        
            weapon.Add(melee.meleeWeaponList[i].ID, melee.meleeWeaponList[i]);
        //Magic
        for (int i = 0; i < magic.magicWeaponList.Count; i++)
            weapon.Add(magic.magicWeaponList[i].ID, magic.magicWeaponList[i]);
        //Range
        for (int i = 0; i < range.rangeWeaponList.Count; i++)
            weapon.Add(range.rangeWeaponList[i].ID, range.rangeWeaponList[i]);
    }

    void Show()
    {
        foreach (KeyValuePair<int, Weapon.WeaponClass> pair in weapon)
        {
            Weapon.WeaponClass wea = pair.Value;
            wea.PrintInfo();
        }
    }
}