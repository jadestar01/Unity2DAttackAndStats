using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public TextMeshProUGUI level;
    public int lv = 0;

    private void Update()
    {
        level.text = lv.ToString();
    }

    public void LevelUp() { lv++; }

    public void LevelDown() { lv--; }
}