using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        //Description창의 설명에 대한 스크립트
        //나중에 아이템을 받아서 타입을 보고 샅샅이 뜯어버리게 만들자.
        public enum ItemType
        {
            None,               //기본설정
            Potion,             //소모품
            Scroll,             //소모품
            Melee,              //근접무기
            Magic,              //마법무기
            Range,              //원거리무기
            Trinket             //장신구
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
            //설명 리셋
            ResetDescription();
        }

        public void Update()
        {
            nameHeight = nameSpace.rect.height;
            descriptionHeight = descriptionSpace.rect.height;
        }

        public void ResetDescription()
        {
            //모든 설명 오브젝트를 리셋한다.
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