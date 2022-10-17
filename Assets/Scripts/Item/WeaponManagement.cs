using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponManagement : MonoBehaviour
{
    [SerializeField] private MeleeWeapon melee;
    [SerializeField] private MagicWeapon magic;
    [SerializeField] private RangeWeapon range;
    public Dictionary<int, Weapon.WeaponClass> weapon = new Dictionary<int, Weapon.WeaponClass>();

    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityTxt;
    [SerializeField] private Image borderImage;
    public event Action<WeaponManagement> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;
    private bool empty = true;

    public void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = quantity + "";
        empty = false;
    }

    public void Select()
    {
        borderImage.enabled = true;
    }

    public void OnBeginDrag()
    {
        if (empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerClick(BaseEventData data)
    {
        if (empty)
            return;
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Right)
            OnRightMouseBtnClick?.Invoke(this);
        else
            OnItemClicked.Invoke(this);
    }
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