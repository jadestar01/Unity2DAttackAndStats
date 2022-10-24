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

        public enum ItemQuality
        {
            None,
            Normal,
            Rare,
            Epic,
            Unique,
            Legendary
        };

        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text type;
        [SerializeField] private TMP_Text quality;
        [SerializeField] private RectTransform nameSpace;
        public float nameHeight;
        [SerializeField] private RectTransform descriptionSpace;
        public float descriptionHeight;

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

        public void SetDescription(Sprite sprite, string itemName, int type, int quality, string itemDescription)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            title.text = itemName;
            switch (type)
            {
                case (int)ItemType.None:
                    {
                        this.type.text = "None";
                        break;
                    }
                case (int)ItemType.Potion:
                    {
                        this.type.text = "Potion";
                        break;
                    }
                case (int)ItemType.Scroll:
                    {
                        this.type.text = "Scroll";
                        break;
                    }
                case (int)ItemType.Melee:
                    {
                        this.type.text = "Melee";
                        break;
                    }
                case (int)ItemType.Magic:
                    {
                        this.type.text = "Magic";
                        break;
                    }
                case (int)ItemType.Range:
                    {
                        this.type.text = "Range";
                        break;
                    }
                case (int)ItemType.Trinket:
                    {
                        this.type.text = "Trinket";
                        break;
                    }
            }
            switch (quality)
            {
                case (int)ItemQuality.None:
                    {
                        this.quality.text = "None";
                        this.quality.color = None;
                        break;
                    }
                case (int)ItemQuality.Normal:
                    {
                        this.quality.text = "Normal";
                        this.quality.color = Normal;
                        break;
                    }
                case (int)ItemQuality.Rare:
                    {
                        this.quality.text = "Rare";
                        this.quality.color = Rare;
                        break;
                    }
                case (int)ItemQuality.Epic:
                    {
                        this.quality.text = "Epic";
                        this.quality.color = Epic;
                        break;
                    }
                case (int)ItemQuality.Unique:
                    {
                        this.quality.text = "Unique";
                        this.quality.color = Unique;
                        break;
                    }
                case (int)ItemQuality.Legendary:
                    {
                        this.quality.text = "Legendary";
                        this.quality.color = Legendary;
                        break;
                    }
            }
            description.text = itemDescription;
        }
    }
}