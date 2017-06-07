using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class MoveHandlerClick : MonoBehaviour, IInputClickHandler
{

    private MoveManager moveManager;

    private void Start()
    {
        moveManager = transform.parent.GetComponent<MoveManager>();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        CustomInputManager.executeOnClick = false;
        if (moveManager.isMoving)
        {
            moveManager.isMoving = false;
        }
        else
        {
            moveManager.distance = System.Math.Abs(Vector3.Distance(Camera.main.transform.position, gameObject.transform.position));
            moveManager.isMoving = true;
        }
    }
}
