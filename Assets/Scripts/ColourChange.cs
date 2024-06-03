using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourChange : MonoBehaviour
{

    public Image objectColor;
    public Color color1;

    public void colorchange()
    {
        objectColor.color = color1;
    }
}
