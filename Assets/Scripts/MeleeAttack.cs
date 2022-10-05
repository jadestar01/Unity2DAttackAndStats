using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions.Must;

public class MeleeAttack : MonoBehaviour
{
    public float distance;              //칼과 본체의 거리
    public float attackDeg = 90;          //공격하는 호의 범위 0 ~ 180
    public float attackSpeed = 0.5f;           //공격하는 속도
    public float attackDelay = 0;              //공격을 하기 위해 칼을 들어올리는 속도
    public float attackReady = 0.5f;           //공격 쿨타임
    bool isAttack;
    BoxCollider2D edge;                 //칼의 공격 위치 나중에 자식으로 넣자 콜라이더를
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

            //유예거리
            position = new Vector2(mouse.x - transform.parent.position.x, mouse.y - transform.parent.position.y).normalized * distance;
            this.transform.position = position + (Vector2)transform.parent.position;

            //회전
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
        isAttack = false; //콜라이더 종료
    }
}
