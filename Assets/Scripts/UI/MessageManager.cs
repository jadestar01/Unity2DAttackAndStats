using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public GameObject messageBox;
    public GameObject messagePanel;
    //���� �޼�������Ʈ�� ��á�µ���, �޼����� ���´ٸ�, 0�� �޼����� ������ �����.
    List<GameObject> messageList = new List<GameObject>();


    public void Message(string message)
    {
        GameObject box = Instantiate(messageBox, Vector2.zero, Quaternion.identity);
        box.transform.SetParent(messagePanel.transform);
        box.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = message;
        messageList.Add(box);
        if (messageList.Count > 3)
        {
            Destroy(messageList[0].gameObject);
            messageList.RemoveAt(0);
        }
    }
}