using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    Vector2 move;
    SpriteRenderer spriteRenderer;
    public GameObject mouse;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (mouse.GetComponent<Mouse>().cursorLoc.x > transform.position.x) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;

        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(move.normalized * moveSpeed * Time.deltaTime);
    }
}
