using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefaultModifier : MonoBehaviour
{
    public ModifierData modifierData = new ModifierData();
    public GameObject DefaultModifierPanel;
    public GameObject buffSlot;
    public GameObject statSlot;

    public void SetDefaultModifier()
    {
        if (modifierData.buff == null)
        {
            GameObject item = Instantiate(buffSlot, Vector2.zero, Quaternion.identity);
            item.transform.SetParent(DefaultModifierPanel.transform);
            item.GetComponent<BuffSlot>().SetName(modifierData.buff.Name, modifierData.buff);
        }
        else
        {
            GameObject item = Instantiate(statSlot, Vector2.zero, Quaternion.identity);
            item.transform.SetParent(DefaultModifierPanel.transform);
            item.GetComponent<Modifier>().SetModifier(NameChange(), modifierData.value);
        }
    }

    public string NameChange()
    {
        if (modifierData.statModifier.GetType() == typeof(HealthModifier))
        {
            return "체력 회복";
        }
        else if (modifierData.statModifier.GetType() == typeof(ManaModifier))
        {
            return "마나 회복";
        }
        else if (modifierData.statModifier.GetType() == typeof(StaminaModifier))
        {
            return "기력 회복";
        }
        else
            return "";
    }
}
