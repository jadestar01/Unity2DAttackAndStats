using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Net;

public class DamageMessage : MonoBehaviour
{
    public float time = 2f;                     //���� �ð�
    public void SetMessage(int value, Color color, string adder)
    {
        //0~1000�� ������� ���´ٰ� �������� ��, 
        //�����/20
        GetComponent<TMP_Text>().DOFade(0, time);
        if (value > 0)
            GetComponent<TMP_Text>().text = adder + value.ToString();
        else if (value < 0)
            GetComponent<TMP_Text>().text = value.ToString();
        GetComponent<TMP_Text>().color = color;
        GetComponent<RectTransform>().position = new Vector2(GetComponent<RectTransform>().position.x + Random.Range(-100, 100), GetComponent<RectTransform>().position.y + Random.Range(-100, 100));
        DestroyTimer();
    }

    IEnumerator DestroyTimer()
    {
        yield return time;
        Destroy(gameObject);
    }
}
