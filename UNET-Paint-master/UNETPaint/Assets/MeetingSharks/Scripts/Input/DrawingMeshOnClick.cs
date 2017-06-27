using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class DrawingMeshOnClick : MonoBehaviour, IInputClickHandler
{

    public void OnInputClicked(InputClickedEventData eventData)
    {
        DrawingManager dm = GameObject.FindGameObjectWithTag("localPlayer").GetComponent<DrawingManager>();
        if (dm.mode == "scaling" || dm.mode == "moving") 
        {
            if (gameObject.transform.parent)
            {
                CustomInputManager.executeOnClick = false;
                gameObject.transform.parent.GetComponent<EditDrawingMode>().OnSelect();
            }
        }
    }
}
