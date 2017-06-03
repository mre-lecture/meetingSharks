using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ScaleHandlerClick : MonoBehaviour, IInputClickHandler
{
    private ScalingManager scalingManager;
    
    private void Start()
    {
        scalingManager = transform.parent.GetComponent<ScalingManager>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        CustomInputManager.executeOnClick = false;
        if(scalingManager.isScaling)
        {
            scalingManager.isScaling = false;
        } else
        {
            scalingManager.isScaling = true;
        }
    }

}
