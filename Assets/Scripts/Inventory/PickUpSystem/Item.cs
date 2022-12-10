using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField] public ItemSO InventoryItem { get; set; }       //����� ������
    [field: SerializeField] public int Quantity { get; set; } = 1;                  //�������� ����
    [SerializeField] private AudioSource audioSource;                               //ȹ�� �Ҹ�
    [SerializeField] private float duration = 0.3f;                                 //ȹ�� �̺�Ʈ �ð�
    private void Start()
    {
        //������ ����
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
    }

    internal void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickUp());
    }

    private IEnumerator AnimateItemPickUp()
    {
        //�������� �������ν�, �Ҹ�������, �������� ������ ������� �̺�Ʈ
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
