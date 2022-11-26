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
    public string Name;
    private string Description;
    public float duration;
    float filler;
    float timer;
    public bool isBuffEnd = false;

    private void Start()
    {
        filler = 0.0f;
        timer = duration;
        isBuffEnd = false;
    }

    private void Update()
    {
        backImage.fillAmount = filler;
        durationTxt.text = timer.ToString("F1") + "s";
        if (timer == 0)
            isBuffEnd = true;
        if (isBuffEnd)
            Destroy(gameObject);
    }

    public void SetBuff(Sprite image, string name, string description, float duration)
    {
        buffImage.sprite = image;
        backImage.sprite = image;
        this.Name = name;
        this.Description = description;
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
}