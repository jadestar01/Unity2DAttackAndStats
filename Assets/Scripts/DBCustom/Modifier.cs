using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Modifier : MonoBehaviour
{
    public TMP_Text name;
    public TMP_InputField value;

    public void SetModifier(string name, float value)
    {
        this.name.text = name;
        this.value.text = value.ToString();
    }
}
