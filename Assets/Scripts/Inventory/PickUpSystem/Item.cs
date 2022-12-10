using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField] public ItemSO InventoryItem { get; set; }       //드랍된 아이템
    [field: SerializeField] public int Quantity { get; set; } = 1;                  //아이템의 개수
    [SerializeField] private AudioSource audioSource;                               //획득 소리
    [SerializeField] private float duration = 0.3f;                                 //획득 이벤트 시간
    private void Start()
    {
        //아이템 생성
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
    }

    internal void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickUp());
    }

    private IEnumerator AnimateItemPickUp()
    {
        //아이템을 먹음으로써, 소리가나고, 아이템이 서서히 사라지는 이벤트
        audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
