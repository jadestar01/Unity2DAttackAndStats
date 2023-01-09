using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public int xSize;
    public int ySize;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
