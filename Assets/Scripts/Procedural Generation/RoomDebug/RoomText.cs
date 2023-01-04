using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomText : MonoBehaviour
{
    public void SetRoomNumber(int num)
    {
        gameObject.GetComponent<TMP_Text>().text = num.ToString();
    }
}
