using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monoinstance : MonoBehaviour
{
    public static Monoinstance instance;
    private void Start()
    {
        Monoinstance.instance = this;
    }
}
