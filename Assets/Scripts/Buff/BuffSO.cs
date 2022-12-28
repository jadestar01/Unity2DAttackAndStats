using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.UI;

public abstract class BuffSO : ScriptableObject
{
    public enum BuffType { None, Buff, Debuff };
    public bool isTicking;
    [field: SerializeField] public Sprite Image { get; set; }                           //���� �̹���
    [field: SerializeField] public int BuffCode;                                        //���� �ڵ�
    [field: SerializeField] public BuffType Type { get; set; }                          //���� Ÿ�� (Buff/Debuff)
    [field: SerializeField] public string Name { get; set; }                            //���� �̸�
    [field: SerializeField][field: TextArea] public string Description { get; set; }    //���� ����
    [field: SerializeField] public float Duration { get; set; }                         //���� �ð� -1�̸� ���� �� ���� ������ ����
    [field: SerializeField] public float Tick { get; set; }                             //���� ƽ
    public Coroutine Cor;
    public abstract void AffectTarget(GameObject Main, GameObject Target);                               //ȿ�� �������̵�
}