﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDrawingScaleListener : MonoBehaviour {

    void Start()
    {
        GameObject drawingSettings = GameObject.Find("DrawingSettings");
        if (drawingSettings)
        {
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(drawingSettings.GetComponent<DrawingSettings>().SetScaleMode);
        }
    }

}
