using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponController : MonoBehaviour
{
    [HideInInspector] public float distance = 0.8f;
    public bool isAttack;
    [HideInInspector] public float speedCoefficient = 1.0f;
    [HideInInspector] public List<GameObject> child = new List<GameObject>();
    [HideInInspector] public GameObject melee;
    [HideInInspector] public GameObject magic;
    [HideInInspector] public GameObject range;

    [HideInInspector] int weaponActive = 0;
    [HideInInspector] public float angle;
    [HideInInspector] public Vector2 mouse, position;

    void Start()
    {
        isAttack = false;
        child.Add(melee); child.Add(magic); child.Add(range);
    }

    void Update()
    {
        WeaponActive();
        WeaponTransform();
    }

    void WeaponActive()
    {
        for (int i = 0; i < child.Count; i++) child[i].SetActive(i == weaponActive ? true : false);
        if (Input.GetKeyDown(KeyCode.Alpha1)) weaponActive = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) weaponActive = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) weaponActive = 2;
    }

    void WeaponTransform()
    {
        if (!isAttack)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //앵커 이동
            child[weaponActive].transform.GetChild(0).transform.position = transform.parent.position;

            //유예거리
            position = new Vector2(mouse.x - child[weaponActive].transform.GetChild(0).transform.position.x, mouse.y - child[weaponActive].transform.GetChild(0).transform.position.y).normalized * distance;
            child[weaponActive].transform.GetChild(0).transform.position = position + (Vector2)transform.position;

            //회전
            angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
            child[weaponActive].transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            //플립
            if (mouse.x - child[weaponActive].transform.GetChild(0).transform.position.x >= 0) child[weaponActive].transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            else if (mouse.x - child[weaponActive].transform.GetChild(0).transform.position.x < 0) child[weaponActive].transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
