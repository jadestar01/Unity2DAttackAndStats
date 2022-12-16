using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAdder : MonoBehaviour
{
    public Sprite sprite;

    public void SpriteAdd()
    {
        if (GameObject.Find("DB").GetComponent<DBCustomManager>().DB == 0)
        {
            GameObject.Find("DB").GetComponent<DBCustomManager>().equipItem.ItemImage = sprite;
            GameObject.Find("DB").GetComponent<DBCustomManager>().E_itemImage.sprite = sprite;
        }
        else if (GameObject.Find("DB").GetComponent<DBCustomManager>().DB == 1)
        {
            GameObject.Find("DB").GetComponent<DBCustomManager>().consumeItem.ItemImage = sprite;
            GameObject.Find("DB").GetComponent<DBCustomManager>().C_itemImage.sprite = sprite;
        }
        else if (GameObject.Find("DB").GetComponent<DBCustomManager>().DB == 2)
        {
            GameObject.Find("DB").GetComponent<DBCustomManager>().upgradeItem.ItemImage = sprite;
            GameObject.Find("DB").GetComponent<DBCustomManager>().U_itemImage.sprite = sprite;
        }
    }
}
