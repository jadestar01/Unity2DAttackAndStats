using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

public abstract class BuffSO : ScriptableObject
{
    public enum BuffType { None, Buff, Debuff };
    public bool isTicking;
    [field: SerializeField] public Sprite Image { get; set; }                           //버프 이미지
    [field: SerializeField] public int BuffCode;                                        //버프 코드
    [field: SerializeField] public BuffType Type { get; set; }                          //버프 타입 (Buff/Debuff)
    [field: SerializeField] public string Name { get; set; }                            //버프 이름
    [field: SerializeField][field: TextArea] public string Description { get; set; }    //버프 설명
    [field: SerializeField] public float Duration { get; set; }                         //버프 시간 -1이면 해제 전 까지 무한히 지속
    [field: SerializeField] public float Tick { get; set; }                             //버프 틱
    public Coroutine Cor;
    public abstract void AffectTarget(GameObject Main, GameObject Target);                               //효과 오버라이드
}