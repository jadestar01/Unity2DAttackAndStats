using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelDisplayer : MonoBehaviour
{
    TextMeshProUGUI level;
    [SerializeField]private PlayerLevel playerLevel;
    void Start()
    {
        level = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        level.text = "Lv." + playerLevel.lv.ToString();
    }
}
