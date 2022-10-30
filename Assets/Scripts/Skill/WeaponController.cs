using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponController : MonoBehaviour
{
    [HideInInspector] public float distance = 0.8f;
    public bool isAttack;
    [HideInInspector] public List<GameObject> anchor;
    [HideInInspector] public List<GameObject> weapon;
    [SerializeField] private GameObject player;

    public GameObject Melee;
    public GameObject Magic;
    public GameObject Range;

    //수정필요, weaponaget에서 받아오자.
    private GameObject melee;
    private GameObject magic;
    private GameObject range;

    [HideInInspector] public int weaponActive = 0;
    [HideInInspector] public float angle;
    [HideInInspector] public Vector2 mouse, position;

    void Start()
    {
        isAttack = false;
        anchor = new List<GameObject>();
        weapon = new List<GameObject>();

        anchor.Add(Melee); anchor.Add(Magic); anchor.Add(Range);
    }

    void Update()
    {
        GetWeapon();
        WeaponActive();
        WeaponTransform();
    }

    void GetWeapon()
    {
        if (Melee.transform.childCount != 0) melee = Melee.transform.GetChild(0).gameObject; else melee = null;
        if (Magic.transform.childCount != 0) magic = Magic.transform.GetChild(0).gameObject; else magic = null;
        if (Range.transform.childCount != 0) range = Range.transform.GetChild(0).gameObject; else range = null;
    }


    void WeaponActive()
    {
        if (!isAttack)
        {
            for (int i = 0; i < anchor.Count; i++) anchor[i].SetActive(i == weaponActive ? true : false);
            if (Input.GetKeyDown(KeyCode.Alpha1)) weaponActive = 0;
            else if (Input.GetKeyDown(KeyCode.Alpha2)) weaponActive = 1;
            else if (Input.GetKeyDown(KeyCode.Alpha3)) weaponActive = 2;
        }
    }

    void WeaponTransform()
    {
        if (!isAttack)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;
            anchor[weaponActive].transform.position = player.transform.position;

            /*
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
            */
        }
    }
}
