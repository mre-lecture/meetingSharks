using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputManager : MonoBehaviour {

    private GameObject drawingSettingsObject;
    public bool executeOnClick;

    private void Start()
    {
        executeOnClick = true;
        drawingSettingsObject = GameObject.Find("DrawingSettings");
    }

    public void OnTap()
    {
        if (executeOnClick)
        {
            GameObject localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
            localPlayer.GetComponent<DrawingManager>().inputDown = true;

            DrawingSettings drawingSettings = drawingSettingsObject.GetComponent<DrawingSettings>();
            
        }
        executeOnClick = true;
    }
}
