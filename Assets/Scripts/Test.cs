using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public int time = 5;
    public float degree;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        if (Input.GetKeyDown(KeyCode.Q)) transform.DORotate(new Vector3(0, 0, degree), time, RotateMode.LocalAxisAdd);
        if (Input.GetKeyDown(KeyCode.E)) transform.DORotate(new Vector3(0, 0, -1 * degree), time, RotateMode.LocalAxisAdd);
    }
}
