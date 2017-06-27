﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDrawingMoveListener : MonoBehaviour {

    void Start()
    {
        GameObject drawingSettings = GameObject.Find("DrawingSettings");
        if (drawingSettings)
        {
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(drawingSettings.GetComponent<DrawingSettings>().SetMoveMode);
        }
    }

}
