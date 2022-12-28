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
        float upgradeRate = upgradeMaterial.upgradeRate;
        InventoryItem item;

        if (!playerStat.isReadyToUpgrade)
        {
            MessageManager.Instance.Message("제단과 너무 멀리있습니다!");
            return;
        }


        //강화 아이템 체크
        if ((inventoryData.GetItemAt(upgradeItemIndex).item.Type == ItemSO.ItemType.Melee) ||
            (inventoryData.GetItemAt(upgradeItemIndex).item.Type == ItemSO.ItemType.Magic) ||
            (inventoryData.GetItemAt(upgradeItemIndex).item.Type == ItemSO.ItemType.Range) ||
            (inventoryData.GetItemAt(upgradeItemIndex).item.Type == ItemSO.ItemType.Trinket))
        {
            item = inventoryData.GetItemAt(upgradeItemIndex);
        }
        else
        {
            MessageManager.Instance.Message("강화할 수 없는 아이템입니다!");
            return;
        }
        if (item.FindIndex(7) == -1)
        {
            MessageManager.Instance.Message("강화할 수 없는 아이템입니다!");
            return;
        }
        if (item.itemState[item.FindIndex(7)].value > 0)
        { }
        else
        {
            MessageManager.Instance.Message("강화 횟수를 초과할 수 없습니다!");
            return;
        }

        int rate = Random.Range(0, 101); //0~100사이의 난수 생성
        if (rate <= upgradeRate) //업그레이드 내에 들어온다면,
        {
            //강화성공
            if (upgradeMaterial.upgradeType == UpgradeItemSO.UpgradeType.Normal)
            {
                //일반 강화
                //재료의 스텟을 순회하며, 아이템에 해당 스텟이 있는 지 확인해야한다.
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
                {
                    MessageManager.Instance.Message("강화 항목이 없습니다!");
                    return;
                }
                item.upgradeResults.Add(EquippableItemSO.UpgradeResult.Normal);
                MessageManager.Instance.Message("일반 강화에 성공했습니다!");
            }
            else if (upgradeMaterial.upgradeType == UpgradeItemSO.UpgradeType.Special)
            {
                //초월 강화
                //아이템에 해당 스텟을 더해준다.
                for (int i = 0; i < material.itemState.Count; i++)
                {
                    item.AddParameter(material.itemState[i]);
                }
                item.upgradeResults.Add(EquippableItemSO.UpgradeResult.Special);
                MessageManager.Instance.Message("초월 강화에 성공했습니다!");
            }
        }
        else
        {
            //강화실패
            item.upgradeResults.Add(EquippableItemSO.UpgradeResult.Fail);
            MessageManager.Instance.Message("강화에 실패했습니다!");
        }
        //갱신
        playerStat.isItemChanged = true;
        //아이템 업그레이드 횟수 감소
        item.itemState[item.FindIndex(7)] = item.itemState[item.FindIndex(7)].AddParameterValue(-1);

        //개수감소
        inventoryData.RemoveItem(upgradeMaterialIndex, 1);
    }
}