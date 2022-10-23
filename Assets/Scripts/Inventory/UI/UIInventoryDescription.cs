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
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;

        public void Awake()
        {
            //설명 리셋
            ResetDescription();
        }

        public void ResetDescription()
        {
            //모든 설명 오브젝트를 리셋한다.
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