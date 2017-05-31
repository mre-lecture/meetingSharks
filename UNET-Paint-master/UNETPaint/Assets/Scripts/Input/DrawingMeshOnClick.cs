using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class DrawingMeshOnClick : MonoBehaviour, IInputClickHandler
{

    private DrawingSettings drawingSettings;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        drawingSettings = GameObject.Find("DrawingSettings").GetComponent<DrawingSettings>();
        if (drawingSettings.mode == "scaling" && gameObject.transform.parent)
        {
            drawingSettings.GetComponent<CustomInputManager>().executeOnClick = false;
            gameObject.transform.parent.GetComponent<EditDrawingMode>().OnSelect();
        }
    }
}
