using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorPallete;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;             //ĵ����
    [SerializeField] private Camera mainCam;            //ī�޶�
    [SerializeField] private UIInventoryItem item;      //������

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>(); //������ �߻����� ������ ������. Canvas�� ���� ������.
        mainCam = Camera.main;
        item = GetComponentInChildren<UIInventoryItem>();
    }

    public void SetData(Sprite sprite, int quantity, ItemQuality quality)
    {
        //�������� ������.
        item.SetData(sprite, quantity, quality);
    }

    void Update()
    {
        //���콺 ��ġ�� ������Ʈ�Ͽ� ��ũ���� ������
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        //�������� �巡�� ���� ��, Toggle�� Ȱ��ȭ �Ǹ�, ����� �޽����� ����.
        //Debug.Log($"Item toggled {val}");
        gameObject.SetActive(val);
    }
}
