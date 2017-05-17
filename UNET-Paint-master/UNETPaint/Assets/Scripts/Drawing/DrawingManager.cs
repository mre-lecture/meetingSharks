using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HoloToolkit.Unity.InputModule;
using System;

public class DrawingManager : NetworkBehaviour, IInputClickHandler
{

    public GameObject drawingPrefab;

    int thisDrawingId = 0;
    Vector3 startPos;
    Plane objPlane;

    public float drawingDistance;
    public Color color;
    public float width;

    public bool inputDown;
    public bool inputUp;
    public bool pressing;

	void Start () {
        objPlane = new Plane(GetNormalForPlane(), GetPositionForPlane());
        GameObject[] drawings = GameObject.FindGameObjectsWithTag("Drawing");
        foreach(GameObject drawing in drawings)
        {
            DrawingInfo identity = drawing.GetComponent<DrawingInfo>();
            if(isLocalPlayer)
                CmdGetPointsForDrawing(identity.id);
        }

        drawingDistance = 10;
        color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
        width = 0.1f;
    }
	
	void Update () {
		if(Input.GetMouseButtonDown(0) || (!pressing && inputDown))
        {
            inputDown = false;
            pressing = true;
            if (isLocalPlayer)
            {
                objPlane.SetNormalAndPosition(GetNormalForPlane(), GetPositionForPlane());
                thisDrawingId = GetLatestDrawingId() + 1;
                Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    startPos = mRay.GetPoint(rayDistance);
                }

                CmdInstantiateDrawing(thisDrawingId, color, width);
            }
        } else if(Input.GetMouseButton(0) || (pressing && !inputDown))
        {
            inputDown = false;
            if (isLocalPlayer)
            {
                objPlane.SetNormalAndPosition(GetNormalForPlane(), GetPositionForPlane());
                Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    Vector3 point = mRay.GetPoint(rayDistance);
                    CmdDrawToPoint(thisDrawingId, point);
                }
            }
        } else if(Input.GetMouseButtonUp(0) || (inputDown && pressing))
        {
            pressing = false;
            inputDown = false;
            if (isLocalPlayer)
            {
                if (Vector3.Distance(GetDrawingById(thisDrawingId).transform.position, startPos) < 0.1)
                {
                    CmdDestroyDrawing(thisDrawingId);
                }
            }
        }
	}

    private int GetLatestDrawingId()
    {
        int id = 0;
        GameObject[] drawings = GameObject.FindGameObjectsWithTag("Drawing");
        foreach(GameObject drawing in drawings)
        {
            if(drawing.GetComponent<DrawingInfo>().id > id)
            {
                id = drawing.GetComponent<DrawingInfo>().id;
            }
        }
        return id;
    }

    private GameObject GetDrawingById(int id)
    {
        GameObject drawing = null;
        GameObject[] drawings = GameObject.FindGameObjectsWithTag("Drawing");
        foreach(GameObject d in drawings)
        {
            if(d.GetComponent<DrawingInfo>().id == id)
            {
                drawing = d;
            }
        }
        return drawing;
    }

    [Command]
    void CmdInstantiateDrawing(int id, Color color, float width)
    {
        GameObject drawing = (GameObject)Instantiate(drawingPrefab, this.transform.position,
                Quaternion.identity);
        DrawingInfo drawingInfo = drawing.GetComponent<DrawingInfo>();
        drawingInfo.id = id;
        drawingInfo.color = color;
        drawingInfo.width = width;
        drawing.GetComponent<TrailRenderer>().material.color = color;
        drawing.GetComponent<TrailRenderer>().startWidth = width;
        drawing.GetComponent<TrailRenderer>().endWidth = width;
        if (isServer)
        {
            NetworkServer.Spawn(drawing);
        } else
        {
            NetworkServer.SpawnWithClientAuthority(drawing, connectionToClient);
        }
    }

    [Command]
    void CmdDrawToPoint(int id, Vector3 point)
    {
        GameObject drawing = GetDrawingById(id);
        if(drawing != null)
        {
            drawing.transform.position = point;
            drawing.GetComponent<DrawingInfo>().points.Add(point);
        }
    }

    [Command]
    void CmdDestroyDrawing(int id)
    {
        GameObject drawing = GetDrawingById(id);
        Destroy(drawing);
    }

    [Command]
    void CmdGetPointsForDrawing(int id)
    {
        GameObject drawing = GetDrawingById(id);
        List<Vector3> points = drawing.GetComponent<DrawingInfo>().points;
        foreach (Vector3 point in points)
        {
            TargetInitPointsForDrawing(connectionToClient, id, point);
        }
    }

    [TargetRpc]
    void TargetInitPointsForDrawing(NetworkConnection target, int id, Vector3 point)
    {
        GameObject drawing = GetDrawingById(id);
        drawing.GetComponent<DrawingInfo>().points.Add(point);
    }

    private Vector3 GetNormalForPlane()
    {
        return Camera.main.transform.forward * -1;
    }
 
    private Vector3 GetPositionForPlane()
    {
        Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * drawingDistance;
        return position;
    }

    public void SetDrawingWidth(float drawingWidth)
    {
        width = drawingWidth;
    }

    void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
    {
        inputDown = true;
    }
}
