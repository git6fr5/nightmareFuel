using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text tooltipText;

    void OnMouseOver()
    {
        print("hello");
        Show();
    }

    void Show()
    {
        tooltipText.enabled = true;

    }
}
