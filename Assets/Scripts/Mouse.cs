using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public Vector2 cursorLoc;

    SpriteRenderer cursor;

    void Start()
    {
        cursor = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        cursorLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = cursorLoc;
    }
}
