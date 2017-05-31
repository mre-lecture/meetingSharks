using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ScaleHandlerClick : MonoBehaviour, IInputClickHandler
{
    private DrawingSettings drawingSettings;
    public bool isScaling;
    private GameObject cursor;
    private Vector3 lastCursorPos;
    private int id;

    // make a parent empty gameobject for the scale handlers and put the methods and variables there
    private void Start()
    {
        drawingSettings = GameObject.Find("DrawingSettings").GetComponent<DrawingSettings>();
        cursor = GameObject.Find("Cursor");
        id = gameObject.transform.parent.parent.GetComponent<DrawingInfo>().id;
    }

    private void Update()
    {
        if (isScaling)
        {
            Vector3 currCursorPos = cursor.transform.position;
            float distance = Vector3.Distance(lastCursorPos, currCursorPos);
            if (System.Math.Abs(distance) > 0.01)
            {
                GameObject localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
                localPlayer.GetComponent<DrawingManager>().AddToScaleDrawing(id, distance);
            }
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        drawingSettings.GetComponent<CustomInputManager>().executeOnClick = false;
        if(isScaling)
        {
            isScaling = false;
        } else
        {
            lastCursorPos = cursor.transform.position;
            isScaling = true;
        }
    }
}
