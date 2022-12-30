using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMessageManager : MonoBehaviour
{
    private static DamageMessageManager instance;
    public static DamageMessageManager Instance
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject Text;

    [Button]
    public void SetMessage(int damage, Vector2 position, string adder, string type)
    {
        GameObject message = Instantiate(Text, position, Quaternion.identity);
        //message.transform.SetParent(gameObject.transform);
        if(type == "physical")
            message.GetComponent<DamageMessage>().SetMessage(damage, new Color(223 / 255f, 113 / 255f, 38 / 255f, 255 / 255f), adder);
        else if (type == "magical")
            message.GetComponent<DamageMessage>().SetMessage(damage, new Color(118 / 255f, 66 / 255f, 138 / 255f, 255 / 255f), adder);
        else if(type == "health")
            message.GetComponent<DamageMessage>().SetMessage(damage, new Color(172 / 255f, 50 / 255f, 50 / 255f, 255 / 255f), adder);
        else if (type == "mana")
            message.GetComponent<DamageMessage>().SetMessage(damage, new Color(91 / 255f, 110 / 255f, 225 / 255f, 255 / 255f), adder);
        else if(type == "stamina")
            message.GetComponent<DamageMessage>().SetMessage(damage, new Color(251 / 255f, 242 / 255f, 54 / 255f, 255 / 255f), adder);
    }
}