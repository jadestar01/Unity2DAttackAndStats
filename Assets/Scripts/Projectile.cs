using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    public int penetration = -1;   //�����, -1�̸� ���Ѱ��� n+1�� ������.
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (penetration == -1) { }
            else if (penetration > 0) penetration--;
            else if (penetration == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
