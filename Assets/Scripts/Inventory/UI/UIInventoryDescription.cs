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
                        this.type.text = "포션";
                        break;
                    }
                case (int)ItemType.Scroll:
                    {
                        this.type.text = "스크롤";
                        break;
                    }
                case (int)ItemType.Melee:
                    {
                        this.type.text = "근접무기";
                        break;
                    }
                case (int)ItemType.Magic:
                    {
                        this.type.text = "마법무기";
                        break;
                    }
                case (int)ItemType.Range:
                    {
                        this.type.text = "원거리무기";
                        break;
                    }
                case (int)ItemType.Trinket:
                    {
                        this.type.text = "장신구";
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