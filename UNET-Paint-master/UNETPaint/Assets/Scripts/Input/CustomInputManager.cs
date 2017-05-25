using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputManager : MonoBehaviour {

    private GameObject drawingSettingsObject;

    private void Start()
    {
        drawingSettingsObject = GameObject.Find("DrawingSettings");
    }

    public void OnTap()
    {
        GameObject localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        localPlayer.GetComponent<DrawingManager>().inputDown = true;

        DrawingSettings drawingSettings = drawingSettingsObject.GetComponent<DrawingSettings>();
        if(drawingSettings.startMoving)
        {
            drawingSettings.listenOnClick = false;
            drawingSettings.startMoving = false;
            drawingSettings.toMoveObject = null;
        }
    }
}
