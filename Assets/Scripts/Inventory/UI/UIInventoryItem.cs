using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        //아이템 슬롯을 관리하는 스크립트
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityTxt;
        [SerializeField] private Image borderImage;
        //Event Trigger Component로 관리되는 이벤트 목록들이다.
        public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn,
            OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;
        public bool empty = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            //아이템 슬롯의 이미지를 비우고, empty상태로 변경한다,
            itemImage.gameObject.SetActive(false);
            empty = true;
        }

        public void Deselect()
        {
            //아이템 슬롯의 선택을 해제한다.
            borderImage.enabled = false;
        }

        public void SetData(Sprite sprite, int quantity)
        {
            //아이템 슬롯의 이미지와 개수를 설정한다. empty상태를 해제한다.
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityTxt.text = quantity + "";
            empty = false;
        }

        public void Select()
        {
            //아이템 슬롯의 선택을 시작한다.
            borderImage.enabled = true;
        }


        public void OnPointerClick(PointerEventData pointerData)
        {
            //포인터의 여러 행동 중 오른쪽 버튼이 눌린경우 호출된다.
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
            //빈 칸 드래그(함수발동)라면 리턴, 아니라면, OnItenBeginDrag 이벤트
            if (empty)
                return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //빈 칸 드래그 종료(함수발동)라면 OnItemEndDrag 이벤트
            OnItemEndDrag.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            //드랍(함수발동)라면 OnItemDroppedOn 이벤트
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}