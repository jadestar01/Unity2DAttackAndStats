using Inv.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField] private EquippableItemSO weapon;
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private List<ItemParameter> parametersToModify, itemCurrnetState;

    public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        if (weapon != null)
        {
            inventoryData.AddItem(weapon, 1, itemCurrnetState);
        }

        this.weapon = weaponItemSO;
        this.itemCurrnetState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }

    public void ModifyParameters()
    {
        foreach (var parameter in parametersToModify)
        {
            if (itemCurrnetState.Contains(parameter))
            {
                int index = itemCurrnetState.IndexOf(parameter);
                float newValue = itemCurrnetState[index].value + parameter.value;
                itemCurrnetState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue
                };
            }
        }
    }
}
