using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ColorPallete;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        //Descriptionâ�� ���� ���� ��ũ��Ʈ
        //���߿� �������� �޾Ƽ� Ÿ���� ���� ������ �������� ������.
        public enum ItemType
        {
            None,               //�⺻����
            Potion,             //�Ҹ�ǰ
            Scroll,             //�Ҹ�ǰ
            Melee,              //��������
            Magic,              //��������
            Range,              //���Ÿ�����
            Trinket             //��ű�
        };

        [SerializeField] private Image itemImage;
        [SerializeField] private Image tooltipContour;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text type;
        [SerializeField] private TMP_Text quality;
        [SerializeField] private RectTransform nameSpace;
        public float nameHeight;
        [SerializeField] private RectTransform descriptionSpace;
        public float descriptionHeight;
        [SerializeField] private QualityColorPallete pallete;

        public void Awake()
        {
            //���� ����
            ResetDescription();
        }

        public void Update()
        {
            nameHeight = nameSpace.rect.height;
            descriptionHeight = descriptionSpace.rect.height;
        }

        public void ResetDescription()
        {
            //��� ���� ������Ʈ�� �����Ѵ�.
            itemImage.gameObject.SetActive(false);
            title.text = "";
            description.text = "";
            type.text = "";
            quality.text = "";
            gameObject.SetActive(false);
        }

        public void SetDescription(Sprite sprite, string itemName, int type, ItemQuality quality, string itemDescription)
        {
            ResetDescription();
            gameObject.SetActive(true);
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            this.title.text = itemName;
            this.title.color = pallete.ColorPallete(quality);
            switch (type)
            {
                case (int)ItemType.None:
                    {
                        this.type.text = " ";
                        break;
                    }
                case (int)ItemType.Potion:
                    {
                        this.type.text = "����";
                        break;
                    }
                case (int)ItemType.Scroll:
                    {
                        this.type.text = "��ũ��";
                        break;
                    }
                case (int)ItemType.Melee:
                    {
                        this.type.text = "��������";
                        break;
                    }
                case (int)ItemType.Magic:
                    {
                        this.type.text = "��������";
                        break;
                    }
                case (int)ItemType.Range:
                    {
                        this.type.text = "���Ÿ�����";
                        break;
                    }
                case (int)ItemType.Trinket:
                    {
                        this.type.text = "��ű�";
                        break;
                    }
            }
            this.tooltipContour.color = pallete.ColorPallete(quality);
            this.quality.color = pallete.ColorPallete(quality);
            this.quality.text = pallete.QualityString(quality);
            description.text = itemDescription;
        }
    }
}