using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.Assertions.Must;

public class MagicAttack : MonoBehaviour
{
    public float distance = 0.8f;
    public float projectileDistance = 4;
    public float castingTime = 0.3f;
    public float projectileSpeed = 1.5f;

    public Object projectile;
    public Transform shotPosition;
    float x; float y;
    Vector3 shotPos;

    bool isAttack;
    bool canMove;

    float angle;
    int dir;
    Vector2 mouse, position;

    void Start()
    {
        canMove = true; isAttack = false;
    }

    void Update()
    {
        Flip();
        Move();
        AttackMoveControll();
        Manaball();
    }

    void Manaball()
    {
        if (Input.GetMouseButtonDown(0) && !isAttack & manaCheck(5))
        {
            manaMinus(5);
            isAttack = true; canMove = false;
            float deg = Mathf.Atan2(mouse.y - transform.parent.position.y, mouse.x - transform.parent.position.x) * Mathf.Rad2Deg;
            Vector3 myPos = new Vector3(transform.position.x, transform.position.y, 0);

            //투사체 생성
            GameObject pro = Instantiate(projectile, shotPosition.position, Quaternion.Euler(0, 0, deg)) as GameObject;
            Sequence Manaball = DOTween.Sequence();
            //영창
            Manaball.Append(pro.transform.DOScale(0.1f, 0));
            Manaball.Append(pro.transform.DOScale(1, castingTime));
            Manaball.AppendCallback(() => canMove = true);
            Manaball.AppendCallback(() => isAttack = false);
            //발사
            Manaball.Append(pro.transform.DOMove(new Vector3(shotPosition.position.x + Mathf.Cos(deg * Mathf.Deg2Rad) * projectileDistance, shotPosition.position.y + Mathf.Sin(deg * Mathf.Deg2Rad) * projectileDistance, 0), projectileSpeed));
            Manaball.AppendCallback(() => Destroy(pro));
        }
    }

    bool manaCheck(int mana)
    { return transform.parent.parent.GetComponent<Resource>().curMP > mana && transform.parent.parent.GetComponent<Resource>().curMP - mana >= 0;  }

    void manaMinus(int mana)
    { transform.parent.parent.GetComponent<Resource>().curMP -= mana; }

    void AttackMoveControll()
    {
        if (canMove) { transform.parent.parent.GetComponent<Move>().canMove = true; }
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
