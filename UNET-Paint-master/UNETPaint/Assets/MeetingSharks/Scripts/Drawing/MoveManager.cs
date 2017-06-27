using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveManager : MonoBehaviour
{

    private int drawingId;
    public bool isMoving;
    public float distance;

    private void Start()
    {
        drawingId = transform.parent.GetComponent<DrawingInfo>().id;
    }

    private void Update()
    {
        if(isMoving)
        {
            GameObject localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
            localPlayer.GetComponent<DrawingManager>().MoveDrawingTo(drawingId, GetPositionForObject());
        }
    }

    private Vector3 GetPositionForObject()
    {
        Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * distance;
        return position;
    }

}
