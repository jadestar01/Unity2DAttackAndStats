using Inventory;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponController : MonoBehaviour
{
    private static WeaponController instance;
    public static WeaponController Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [HideInInspector] public float distance = 0.8f;
    public List<GameObject> anchor = new List<GameObject>();
    [HideInInspector] public GameObject[] weapon = new GameObject[3];
    [SerializeField] private GameObject player;
    [SerializeField] private InventoryController inventoryController;

    public GameObject Melee;
    public GameObject Magic;
    public GameObject Range;

    private GameObject melee;
    private GameObject magic;
    private GameObject range;

    [HideInInspector] public int weaponActive = 0;
    [HideInInspector] public float angle;
    [HideInInspector] public Vector2 mouse, position;

    public bool isAttack;

    void Start()
    {
        isAttack = false;
        anchor = new List<GameObject>();
        weapon = new GameObject[3];

        anchor.Add(Melee); anchor.Add(Magic); anchor.Add(Range);
    }

    void Update()
    {
        GetWeapon();
        WeaponActive();
        WeaponTransform();
        UseConsumeItem();
    }

    void UseConsumeItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4)) 
        { inventoryController.PerformAction(39); }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        { inventoryController.PerformAction(40); }
    }

    void GetWeapon()
    {
        if (Melee.transform.childCount != 0) melee = Melee.transform.GetChild(0).gameObject; else melee = null; weapon[0] = melee;
        if (Magic.transform.childCount != 0) magic = Magic.transform.GetChild(0).gameObject; else magic = null; weapon[1] = magic;
        if (Range.transform.childCount != 0) range = Range.transform.GetChild(0).gameObject; else range = null; weapon[2] = range;
    }


    void WeaponActive()
    {
        if (!isAttack)
        {
            for (int i = 0; i < anchor.Count; i++) anchor[i].SetActive(i == weaponActive ? true : false);
            if (Input.GetKeyDown(KeyCode.Alpha1)) weaponActive = 0;
            else if (Input.GetKeyDown(KeyCode.Alpha2)) weaponActive = 1;
            else if (Input.GetKeyDown(KeyCode.Alpha3)) weaponActive = 2;
            else { }
        }
    }

    void WeaponTransform()
    {
        if (!isAttack && WeaponInHand())
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;

            //무기 회전
            weapon[weaponActive].transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            //무기 이동
            weapon[weaponActive].transform.position = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized * distance + (Vector2)transform.position;

            //앵커 조정
            for (int i = 0; i < anchor.Count; i++)
            {
                anchor[i].transform.localEulerAngles = Vector3.zero;
                anchor[i].transform.localPosition = Vector3.zero;
            }

            if (mouse.x >= player.transform.position.x) weapon[weaponActive].transform.GetComponent<SpriteRenderer>().flipX = false;
            else if (mouse.x < player.transform.position.x) weapon[weaponActive].transform.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    bool WeaponInHand()
    {
        return anchor[weaponActive].transform.childCount != 0;
    }
}
