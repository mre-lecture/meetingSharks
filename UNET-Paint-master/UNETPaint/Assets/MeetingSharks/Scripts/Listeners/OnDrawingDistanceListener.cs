using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDrawingDistanceListener : MonoBehaviour
{

    void Start()
    {
        GameObject drawingSettings = GameObject.Find("DrawingSettings");
        if (drawingSettings)
        {
            InputField inputfield = gameObject.GetComponent<InputField>();
            inputfield.onValueChanged.AddListener(drawingSettings.GetComponent<DrawingSettings>().SetDrawingDistance);
        }
    }
}
