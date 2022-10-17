using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] Inventory inventoryUI;

    private void Start()
    {
        inventoryUI.InitalizeInventoryUI(24);   //인벤토리를 24칸으로 설정
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isActiveAndEnabled == false) inventoryUI.Show();
            else inventoryUI.Hide();
        }
    }
}
