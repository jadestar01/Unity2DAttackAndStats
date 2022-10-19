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

        //��� ������ �̹����� �����, EMPTY�� �����Ѵ�.
        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            empty = true;
        }

        //����ĭ�� �����.
        public void Deselect()
        {
            borderImage.enabled = false;
        }

        //�������� ä���.
        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityTxt.text = quantity + "";
            empty = false;
        }

        //�������� �����ϸ�, �����̹����� Ȱ��ȭ ��Ų��.
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