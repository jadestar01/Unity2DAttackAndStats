using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLevel : MonoBehaviour
{
    TextMeshProUGUI level;
    public int lv = 0;

    public void LevelUp() { lv++; }

    public void LevelDown() { lv--; }
}
