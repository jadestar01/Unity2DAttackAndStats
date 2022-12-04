using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextEditor : MonoBehaviour
{
    public float time = 2.5f;

    void Start()
    {
        gameObject.GetComponent<Image>().DOFade(0, time);
        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, time);
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
