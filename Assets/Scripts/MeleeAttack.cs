using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions.Must;

public class MeleeAttack : MonoBehaviour
{
    public float distance;              //Į�� ��ü�� �Ÿ�
    public float attackDeg = 90;          //�����ϴ� ȣ�� ���� 0 ~ 180
    public float attackSpeed = 0.5f;           //�����ϴ� �ӵ�
    public float attackDelay = 0;              //������ �ϱ� ���� Į�� ���ø��� �ӵ�
    public float attackReady = 0.5f;           //���� ��Ÿ��
    bool isAttack;
    BoxCollider2D edge;                 //Į�� ���� ��ġ ���߿� �ڽ����� ���� �ݶ��̴���
    SpriteRenderer spriteRenderer;

    float angle;
    int dir;
    Vector2 mouse, position;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isAttack = false;
    }

    private void Update()
    {
        MeleeFlip();
        MeleeMove();
        Slash();
    }

    void Slash()
    {
        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            isAttack = true;
            if (transform.position.x >= 0) dir = 1; else dir = -1;
            float deg = Mathf.Atan2(mouse.y - transform.parent.position.y, mouse.x - transform.parent.position.x) * Mathf.Rad2Deg;

            Sequence SlashSeq = DOTween.Sequence();
            SlashSeq.Append(transform.parent.transform.DORotate(new Vector3(0, 0, dir * 90), attackReady, RotateMode.LocalAxisAdd));
            SlashSeq.Append(transform.parent.transform.DORotate(new Vector3(0, 0, -1 * dir * attackDeg * 2), attackSpeed, RotateMode.WorldAxisAdd));
            SlashSeq.Append(transform.parent.transform.DORotate(Vector3.zero, 0));
            SlashSeq.Play();
        }
    }

    void MeleeMove()
    {
        if (!isAttack)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //�����Ÿ�
            position = new Vector2(mouse.x - transform.parent.position.x, mouse.y - transform.parent.position.y).normalized * distance;
            this.transform.position = position + (Vector2)transform.parent.position;

            //ȸ��
            angle = Mathf.Atan2(mouse.y - transform.parent.position.y, mouse.x - transform.parent.position.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }

    void MeleeFlip()
    {
        if (!isAttack)
        {
            if (transform.position.x >= 0) spriteRenderer.flipX = false;
            else if (transform.position.x < 0) spriteRenderer.flipX = true;
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackReady + attackSpeed + attackDelay);
        isAttack = false; //�ݶ��̴� ����
    }
}
