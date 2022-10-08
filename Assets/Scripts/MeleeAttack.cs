using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions.Must;

public class MeleeAttack : MonoBehaviour
{
    public float distance;                      //칼과 본체의 거리
    public float attackDeg = 90;                //공격하는 호의 범위 0 ~ 180
    public float attackSpeed = 0.5f;            //공격하는 속도
    public float attackDelay = 0;               //공격을 하기 위해 칼을 들어올리는 속도
    public float attackReady = 0.5f;            //공격 쿨타임
    bool isAttack;
    bool canMove;

    float angle;
    int dir;
    Vector2 mouse, position;

    private void Start()
    {
        gameObject.GetComponent<TrailRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        isAttack = false;
        canMove = true;
    }

    private void Update()
    {
        Flip();
        Move();
        AttackMoveControll();
        Slash();
        UpperSlash();
        Piercing();
    }

    void Slash()
    {
        if (Input.GetMouseButtonDown(0) && !isAttack)
        {
            isAttack = true;
            if (mouse.x - transform.position.x >= 0) dir = 1; else dir = -1;
            float deg = Mathf.Atan2(mouse.y - transform.parent.position.y, mouse.x - transform.parent.position.x) * Mathf.Rad2Deg;

            Sequence SlashSeq = DOTween.Sequence();
            SlashSeq.Append(transform.parent.transform.DORotate(new Vector3(0, 0, dir * 90), attackReady, RotateMode.LocalAxisAdd));
            SlashSeq.AppendCallback(() => gameObject.GetComponent<BoxCollider2D>().enabled = true);
            SlashSeq.AppendCallback(() => gameObject.GetComponent<TrailRenderer>().enabled = true);
            SlashSeq.Append(transform.parent.transform.DORotate(new Vector3(0, 0, -1 * dir * attackDeg * 2), attackSpeed, RotateMode.WorldAxisAdd));
            SlashSeq.AppendCallback(() => gameObject.GetComponent<BoxCollider2D>().enabled = false);
            SlashSeq.AppendCallback(() => gameObject.GetComponent<TrailRenderer>().enabled = false);
            SlashSeq.Append(transform.parent.transform.DORotate(Vector3.zero, 0));
            SlashSeq.OnComplete(() => isAttack = false);
            SlashSeq.Play();
        }
    }

    void UpperSlash()
    {
        if (Input.GetMouseButtonDown(1) && !isAttack)
        {
            isAttack = true;
            if (mouse.x - transform.position.x >= 0) dir = 1; else dir = -1;
            float deg = Mathf.Atan2(mouse.y - transform.parent.position.y, mouse.x - transform.parent.position.x) * Mathf.Rad2Deg;

            Sequence UpperSlashSeq = DOTween.Sequence();
            UpperSlashSeq.Append(transform.parent.transform.DORotate(new Vector3(0, 0, -1 * dir * 90), attackReady, RotateMode.LocalAxisAdd));

            UpperSlashSeq.AppendCallback(() => gameObject.GetComponent<BoxCollider2D>().enabled = true);
            UpperSlashSeq.AppendCallback(() => gameObject.GetComponent<TrailRenderer>().enabled = true);
            UpperSlashSeq.Append(transform.parent.transform.DORotate(new Vector3(0, 0, dir * attackDeg * 2), attackSpeed, RotateMode.WorldAxisAdd));

            UpperSlashSeq.AppendCallback(() => gameObject.GetComponent<BoxCollider2D>().enabled = false);
            UpperSlashSeq.AppendCallback(() => gameObject.GetComponent<TrailRenderer>().enabled = false);
            UpperSlashSeq.Append(transform.parent.transform.DORotate(Vector3.zero, 0));
            UpperSlashSeq.OnComplete(() => isAttack = false);
            UpperSlashSeq.Play();
        }
    }

    void Piercing()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isAttack)
        {
            isAttack = true;
            canMove = false;
            if (mouse.x - transform.position.x >= 0) dir = 1; else dir = -1;
            float deg = Mathf.Atan2(mouse.y - transform.parent.position.y, mouse.x - transform.parent.position.x) * Mathf.Rad2Deg;

            Sequence PiercingSeq = DOTween.Sequence();
            //당기고
            PiercingSeq.OnComplete(() => transform.parent.transform.position = transform.parent.transform.parent.transform.position);
            PiercingSeq.Append(transform.parent.transform.DOMove(transform.position + new Vector3(Mathf.Cos(deg * Mathf.Deg2Rad) * -1, Mathf.Sin(deg * Mathf.Deg2Rad) * -1, 0), attackReady));
            //짜른다!
            PiercingSeq.AppendCallback(() => gameObject.GetComponent<BoxCollider2D>().enabled = true);
            PiercingSeq.AppendCallback(() => gameObject.GetComponent<TrailRenderer>().enabled = true);
            PiercingSeq.AppendCallback(() => gameObject.GetComponent<TrailRenderer>().startWidth = 0.3f);
            PiercingSeq.OnComplete(() => transform.parent.transform.position = transform.parent.transform.parent.transform.position);
            PiercingSeq.Append(transform.parent.transform.DOMove(transform.position + new Vector3(Mathf.Cos(deg * Mathf.Deg2Rad) * 1, Mathf.Sin(deg * Mathf.Deg2Rad) * 1, 0), attackSpeed));
            PiercingSeq.AppendCallback(() => gameObject.GetComponent<BoxCollider2D>().enabled = false);
            PiercingSeq.AppendCallback(() => gameObject.GetComponent<TrailRenderer>().enabled = false);
            PiercingSeq.AppendCallback(() => gameObject.GetComponent<TrailRenderer>().startWidth = 1f);
            PiercingSeq.AppendCallback(() => isAttack = false) ;
            PiercingSeq.OnComplete(() => canMove = true);
            PiercingSeq.Play();
        }
    }

    void AttackMoveControll()
    {
        if (canMove) { transform.parent.parent.GetComponent<Move>().canMove = true;  }
        else if (!canMove) { transform.parent.parent.GetComponent<Move>().canMove = false; }
    }

    void Move()
    {
        if (!isAttack)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //앵커 이동
            transform.parent.transform.position = transform.parent.transform.parent.transform.position;

            //유예거리
            position = new Vector2(mouse.x - transform.parent.position.x, mouse.y - transform.parent.position.y).normalized * distance;
            this.transform.position = position + (Vector2)transform.parent.position;

            //회전
            angle = Mathf.Atan2(mouse.y - transform.parent.position.y, mouse.x - transform.parent.position.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }

    void Flip()
    {
        if (!isAttack)
        {
            if (mouse.x - transform.position.x >= 0) gameObject.GetComponent<SpriteRenderer>().flipX = false;
            else if (mouse.x - transform.position.x < 0) gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
