using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class CustomOnClickListener : MonoBehaviour, IInputClickHandler
{
    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameObject.Find("DrawingSettings").GetComponent<CustomInputManager>().executeOnClick = false;
    }
}
