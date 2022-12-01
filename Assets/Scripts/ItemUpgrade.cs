using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemUpgrade : MonoBehaviour
{
    [SerializeField] private Stats playerStat;
    [SerializeField] private InventorySO inventoryData;
    public void Upgrade(int upgradeMaterialIndex, int upgradeItemIndex)
    {
        InventoryItem material = inventoryData.GetItemAt(upgradeMaterialIndex);
        UpgradeItemSO upgradeMaterial = (UpgradeItemSO)material.item;
        int upgradeRate = upgradeMaterial.upgradeRate;
        InventoryItem item;

        //��ȭ ������ üũ
        if ((inventoryData.GetItemAt(upgradeItemIndex).item.Type == ItemSO.ItemType.Melee) ||
            (inventoryData.GetItemAt(upgradeItemIndex).item.Type == ItemSO.ItemType.Magic) ||
            (inventoryData.GetItemAt(upgradeItemIndex).item.Type == ItemSO.ItemType.Range) ||
            (inventoryData.GetItemAt(upgradeItemIndex).item.Type == ItemSO.ItemType.Trinket))
        {
            item = inventoryData.GetItemAt(upgradeItemIndex);
        }
        else
        {
            return;
        }
        if (item.FindIndex(7) == -1)
            return;
        if (item.itemState[item.FindIndex(7)].value > 0)
        { }
        else
        { return; }

        int rate = Random.Range(0, 101); //0~100������ ���� ����
        if (rate <= upgradeRate) //���׷��̵� ���� ���´ٸ�,
        {
            //��ȭ����
            if (upgradeMaterial.upgradeType == UpgradeItemSO.UpgradeType.Normal)
            {
                //�Ϲ� ��ȭ
                //����� ������ ��ȸ�ϸ�, �����ۿ� �ش� ������ �ִ� �� Ȯ���ؾ��Ѵ�.
                int change = 0;
                for (int i = 0; i < material.itemState.Count; i++)
                {
                    if (item.FindIndex(material.itemState[i].itemParameter.ParameterCode) != -1)
                    {
                        item.AddValue(material.itemState[i].itemParameter.ParameterCode, material.itemState[i].value);
                        change++;
                    }
                }
                if (change == 0)
                    return;
            }
            else if (upgradeMaterial.upgradeType == UpgradeItemSO.UpgradeType.Special)
            {
                //�ʿ� ��ȭ
                //�����ۿ� �ش� ������ �����ش�.
                for (int i = 0; i < material.itemState.Count; i++)
                {
                    item.AddParameter(material.itemState[i]);
                }
            }
        }
        else
        {
            //��ȭ����
        }
        //����
        playerStat.isItemChanged = true;
        //������ ���׷��̵� Ƚ�� ����
        item.itemState[item.FindIndex(7)] = item.itemState[item.FindIndex(7)].AddParameterValue(-1);

        //��������
        inventoryData.RemoveItem(upgradeMaterialIndex, 1);
    }
}