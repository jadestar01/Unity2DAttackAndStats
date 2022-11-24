using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;
using Unity.VisualScripting;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private Image buffImage;
    [SerializeField] private Image backImage;
    [SerializeField] private TMP_Text durationTxt;
    private string name;
    private string description;
    public float duration;
    float filler;
    float timer;

    private void Start()
    {
    }

    private void Update()
    {
        backImage.fillAmount = filler;
        durationTxt.text = timer.ToString("F1") + "s";
        if (timer == duration)
            Destroy(gameObject);
    }

    public void SetBuff(Sprite image, string name, string description, float duration)
    {
        buffImage.sprite = image;
        backImage.sprite = image;
        this.name = name;
        this.description = description;
        this.duration = duration;

        StartBuff(duration);
    }

    void StartBuff(float duration)
    {
        //시간을 받아와서 버프 이미지 활성화
        filler = 0.0f;
        timer = duration;
        DOTween.To(() => filler, x => filler = x, 1, duration).SetEase(Ease.Linear); ;
        DOTween.To(() => timer, x => timer = x, 0, duration).SetEase(Ease.Linear); ;
    }

    public void DestroyBuff()
    {
        Destroy(gameObject);
    }
}
