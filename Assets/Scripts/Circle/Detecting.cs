using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detecting : MonoBehaviour
{
    void Start()
    {
        GetComponent<CircleCollider2D>().radius = transform.parent.GetComponent<Mob>().detectingRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.parent.GetComponent<Mob>().inDetecting = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.parent.GetComponent<Mob>().inDetecting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.parent.GetComponent<Mob>().inDetecting = false;
        }
    }
}
