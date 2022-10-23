using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        //Descriptionâ�� ���� ���� ��ũ��Ʈ
        //���߿� �������� �޾Ƽ� Ÿ���� ���� ������ �������� ������.
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;

        public void Awake()
        {
            //���� ����
            ResetDescription();
        }

        public void ResetDescription()
        {
            //��� ���� ������Ʈ�� �����Ѵ�.
            itemImage.gameObject.SetActive(false);
            title.text = "";
            description.text = "";
        }

        public void SetDescription(Sprite sprite, string itemName, string itemDescription)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            title.text = itemName;
            description.text = itemDescription;
        }
    }
}