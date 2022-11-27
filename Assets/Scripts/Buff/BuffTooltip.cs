using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffTooltip : MonoBehaviour
{
    [SerializeField] private RectTransform buffTooltip;
    [SerializeField] private TMP_Text namePlate;
    [SerializeField] private TMP_Text descriptionPlate;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void TooltipActive(string Name, string Description, Vector3 position)
    {
        gameObject.SetActive(true);
        buffTooltip.position = position;
        namePlate.text = Name;
        descriptionPlate.text = Description;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)buffTooltip.transform);
    }
}
