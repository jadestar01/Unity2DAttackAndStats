using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorPallete;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;             //캔버스
    [SerializeField] private Camera mainCam;            //카메라
    [SerializeField] private UIInventoryItem item;      //아이템

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>(); //오류가 발생하지 않을까 생각함. Canvas가 많기 때문임.
        mainCam = Camera.main;
        item = GetComponentInChildren<UIInventoryItem>();
    }

    public void SetData(Sprite sprite, int quantity, ItemQuality quality)
    {
        //아이템을 설정함.
        item.SetData(sprite, quantity, quality);
    }

    void Update()
    {
        //마우스 위치를 업데이트하여 스크린에 보여줌
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        //아이템을 드래그 했을 때, Toggle이 활성화 되며, 디버그 메시지를 띄운다.
        //Debug.Log($"Item toggled {val}");
        gameObject.SetActive(val);
    }
}
