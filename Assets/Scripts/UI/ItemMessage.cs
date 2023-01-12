using DG.Tweening;
using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMessage : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemQuantity;

    public float time = 2.5f;

    void Start()
    {
        gameObject.GetComponent<Image>().DOFade(0, time);
        itemImage.DOFade(0, time);
        itemName.DOFade(0, time);
        itemQuantity.DOFade(0, time);
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public void AddItemMessage(ItemSO item, int Quantity)
    {
        this.itemImage.sprite = item.ItemImage;
        this.itemName.text = item.Name;
        this.itemName.color = QualityColorPallete.Instance.ColorPallete(item.Quality);
        this.itemQuantity.text = "x" + Quantity.ToString();
    }
}
