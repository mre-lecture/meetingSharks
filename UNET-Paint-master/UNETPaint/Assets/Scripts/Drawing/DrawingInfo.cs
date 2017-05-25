using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HoloToolkit.Unity.InputModule;
using System;

public class DrawingInfo : NetworkBehaviour, IInputClickHandler {

    public GameObject drawingSettings;

    [SyncVar]
    public int id;
    [SyncVar]
    public Vector3 lastPos;
    public List<Vector3> points;
    [SyncVar (hook = "OnColor")]
    public Color color;
    [SyncVar(hook = "OnWidth")]
    public float width;
    [SyncVar]
    public string username;
    [SyncVar]
    public string drawingObjectName;

    private void Start()
    {
        drawingSettings = GameObject.Find("DrawingSettings");
    }

    private void OnColor(Color newColor)
    {
        gameObject.GetComponent<TrailRenderer>().material.color = color;
    }

    public void OnWidth(float newWidth)
    {
        gameObject.GetComponent<TrailRenderer>().startWidth = newWidth;
        gameObject.GetComponent<TrailRenderer>().endWidth = newWidth;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        drawingSettings.GetComponent<DrawingSettings>().StartMoving(gameObject);
    }
}
