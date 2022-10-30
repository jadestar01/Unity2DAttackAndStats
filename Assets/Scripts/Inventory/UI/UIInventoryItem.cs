using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using ColorPallete;
using Inventory.UI;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        //������ ������ �����ϴ� ��ũ��Ʈ
        [SerializeField] private Image slotImage;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityTxt;
        [SerializeField] private Image borderImage;
        [SerializeField] private QualityColorPallete pallete;
        //Event Trigger Component�� �����Ǵ� �̺�Ʈ ��ϵ��̴�.
        public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn,
            OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;
        public bool empty = true;
        public ItemQuality itemQuality = 0;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        public void Update()
        {
            if (empty)
                slotImage.color = pallete.ColorPallete(0);
        }

        public void ResetData()
        {
            //������ ������ �̹����� ����, empty���·� �����Ѵ�,
            itemImage.gameObject.SetActive(false);
            empty = true;
        }

        public void Deselect()
        {
            //������ ������ ������ �����Ѵ�.
            borderImage.enabled = false;
        }

        public void SetSlotColor(ItemQuality quality)
        {
            slotImage.color = pallete.ColorPallete(quality);
        }

        public void SetData(Sprite sprite, int quantity, ItemQuality quality)
        {
            //������ ������ �̹����� ������ �����Ѵ�. empty���¸� �����Ѵ�.
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            if (quantity != 1)
                quantityTxt.text = quantity + "";
            else
                quantityTxt.text = "";
            slotImage.color = pallete.ColorPallete(quality);
            SetSlotColor(quality);
            empty = false;
        }

        public void Select()
        {
            //������ ������ ������ �����Ѵ�.
            borderImage.enabled = true;
        }


        public void OnPointerClick(PointerEventData pointerData)
        {
            //�������� ���� �ൿ �� ������ ��ư�� ������� ȣ��ȴ�.
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //�� ĭ �巡��(�Լ��ߵ�)��� ����, �ƴ϶��, OnItenBeginDrag �̺�Ʈ
            if (empty)
                return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //�� ĭ �巡�� ����(�Լ��ߵ�)��� OnItemEndDrag �̺�Ʈ
            OnItemEndDrag.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            //���(�Լ��ߵ�)��� OnItemDroppedOn �̺�Ʈ
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}