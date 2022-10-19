using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inv.UI
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityTxt;
        [SerializeField] private Image borderImage;
        public event Action<InventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMousBtnClick;
        private bool empty = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        //모든 아이템 이미지를 지우고, EMPTY로 설정한다.
        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            empty = true;
        }

        //설정칸을 지운다.
        public void Deselect()
        {
            borderImage.enabled = false;
        }

        //아이템을 채운다.
        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityTxt.text = quantity + "";
            empty = false;
        }

        //아이템을 설정하며, 보더이미지를 활성화 시킨다.
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
            PointerEventData pointerData = (PointerEventData)data;
            if (pointerData.button == PointerEventData.InputButton.Right)
                OnRightMousBtnClick?.Invoke(this);
            else
                OnItemClicked.Invoke(this);
        }
    }
}