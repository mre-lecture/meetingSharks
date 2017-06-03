﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class DrawingManager : NetworkBehaviour
{
    public string mode;
    public float drawingDistance;
    public float width;

    public bool inputDown;
    public bool inputUp;
    public bool pressing;

    private GameObject cursor;
    private int thisDrawingId;
    private Plane objPlane;
    private List<GameObject> drawingMeshes;

    public Color color;
    public string drawingObjectName;

	void Start () {
        objPlane = new Plane(GetNormalForPlane(), GetPositionForPlane());
        cursor = GameObject.Find("Cursor");
        drawingMeshes = new List<GameObject>();
        color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
        drawingDistance = 1;
        mode = "drawing";
        width = 0.01f;
        thisDrawingId = 0;
    }
	
	void Update () {
        if ((!pressing && inputDown))
        {
            inputDown = false;
            pressing = true;
            if (isLocalPlayer && mode == "drawing")
            {
                objPlane.SetNormalAndPosition(GetNormalForPlane(), GetPositionForPlane());
                thisDrawingId = GetLatestDrawingId() + 1;
                Ray mRay = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(cursor.transform.localPosition));
                float rayDistance;
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    Vector3 lastPos = mRay.GetPoint(rayDistance);
                    GameObject localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
                    PlayerInfo playerInfo = localPlayer.GetComponent<PlayerInfo>();
                    CmdInstantiateDrawing(thisDrawingId, lastPos, color, width, playerInfo.username, drawingObjectName);
                }
                
            }
        } else if((pressing && !inputDown))
        {
            inputDown = false;
            if (isLocalPlayer && mode == "drawing")
            {
                objPlane.SetNormalAndPosition(GetNormalForPlane(), GetPositionForPlane());
                Ray mRay = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(cursor.transform.localPosition));
                float rayDistance;
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    Vector3 point = mRay.GetPoint(rayDistance);
                    CmdDrawToPoint(thisDrawingId, point, color);
                }
            }
        } else if((inputDown && pressing))
        {
            pressing = false;
            inputDown = false;
            if (isLocalPlayer && mode == "drawing")
            {
                GameObject drawing = GetDrawingById(thisDrawingId);
                if (drawing)
                {
                    if (drawing.GetComponent<DrawingInfo>().points.Count <= 1)
                    {
                        CmdDestroyDrawing(thisDrawingId);
                    }
                    else
                    {
                        CmdGroupMeshes(thisDrawingId);
                    }
                }
            }
        }
	}

    private int GetLatestDrawingId()
    {
        return GetLatestIDWithTag("Drawing");
    }

    private int GetLatestIDWithTag(string tag)
    {
        int id = 0;
        GameObject[] drawings = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject drawing in drawings)
        {
            if (drawing.GetComponent<DrawingInfo>().id > id)
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
    void CmdInstantiateDrawing(int id, Vector3 lastPos, Color color, float width, string username, string drawingObjectName)
    {
        GameObject drawing = (GameObject)Instantiate(Resources.Load("Prefabs/Drawing"), this.transform.position,
                Quaternion.identity);
        DrawingInfo drawingInfo = drawing.GetComponent<DrawingInfo>();
        TrailRenderer drawingTrailRenderer = drawing.GetComponent<TrailRenderer>();
        drawingInfo.id = id;
        drawingInfo.lastPos = lastPos;
        drawingInfo.color = color;
        drawingInfo.width = width;
        drawingTrailRenderer.material.color = color;
        drawingTrailRenderer.startWidth = width;
        drawingTrailRenderer.endWidth = width;
        drawingInfo.username = username;
        drawingInfo.drawingObjectName = drawingObjectName;

        NetworkServer.Spawn(drawing);
    }

    [Command]
    void CmdDrawToPoint(int id, Vector3 point, Color color)
    {
        GameObject drawing = GetDrawingById(id);
        if (drawing != null)
        {
            DrawingInfo drawingInfo = drawing.GetComponent<DrawingInfo>();
            if (Vector3.Distance(drawingInfo.lastPos, point) > 0.01)
            {
                drawing.transform.position = point;
                RpcDrawToPoint(id, point, color);
            }
        }
    }

    [ClientRpc]
    void RpcDrawToPoint(int id, Vector3 point, Color color)
    {
        GameObject drawing = GetDrawingById(id);
        if(drawing)
        {
            drawing.transform.position = point;
            DrawingInfo drawingInfo = drawing.GetComponent<DrawingInfo>();
            TrailRenderer drawingTrailRenderer = drawing.GetComponent<TrailRenderer>();
            drawingInfo.points.Add(point);
            string drawingObjectName = drawingInfo.drawingObjectName;

            if (drawingObjectName == "" || drawingObjectName == "Line")
            {
                drawingTrailRenderer.enabled = true;
            }
            else
            {
                drawingTrailRenderer.enabled = false;
            }

            if (drawingObjectName != "" && drawingObjectName != "Line")
            {
                GameObject cube = (GameObject)Instantiate(Resources.Load("Prefabs/DrawingObjects/" + drawingObjectName));
                MeshRenderer mesh = cube.GetComponent<MeshRenderer>();
                mesh.material.color = color;

                Vector3 distance = drawingInfo.lastPos - point;
                cube.transform.localScale = new Vector3(Math.Abs(distance.x), cube.transform.localScale.y, cube.transform.localScale.z);
                Vector3 divided = cube.transform.localScale;
                divided.y = 0;
                divided.z = 0;
                divided.x = divided.x / 2f;
                cube.transform.position = point + divided;
                drawingMeshes.Add(cube);
            }
            drawingInfo.lastPos = point;
        }
    }

    [Command]
    void CmdGroupMeshes(int drawingId)
    {
        RpcGroupMeshes(drawingId);
    }

    [ClientRpc]
    void RpcGroupMeshes(int drawingId)
    {
        GameObject drawing = GetDrawingById(drawingId);
        if (drawing)
        {
            foreach (GameObject drawingMesh in drawingMeshes)
            {
                drawingMesh.transform.SetParent(drawing.transform);
            }
        }
        drawingMeshes = new List<GameObject>();
    }

    [Command]
    void CmdDestroyDrawing(int id)
    {
        GameObject drawing = GetDrawingById(id);
        Destroy(drawing);
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

    public void DeleteDrawings()
    {
        GameObject localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        PlayerInfo playerInfo = localPlayer.GetComponent<PlayerInfo>();
        CmdOnDelete(playerInfo.username);
    }

    [Command]
    public void CmdOnDelete(string username)
    {
        GameObject[] drawings = GameObject.FindGameObjectsWithTag("Drawing");
        foreach (GameObject drawing in drawings)
        {
            if (drawing.GetComponent<DrawingInfo>().username == username)
            {
                Destroy(drawing);
            }
        }
    }

    public void MoveDrawingTo(int id, Vector3 position)
    {
        CmdMoveDrawingTo(id, position);
    }

    [Command]
    private void CmdMoveDrawingTo(int id, Vector3 position)
    {
        RpcMoveDrawingTo(id, position);
    }

    [ClientRpc]
    private void RpcMoveDrawingTo(int id, Vector3 position)
    {
        GameObject drawing = GetDrawingById(id);
        drawing.transform.position = position;
    }

    public void RotateDrawing(int id, Quaternion rotation)
    {
        CmdRotateDrawing(id, rotation);
    }

    [Command]
    private void CmdRotateDrawing(int id, Quaternion rotation)
    {
        RpcRotateDrawing(id, rotation);
    }

    [ClientRpc]
    private void RpcRotateDrawing(int id, Quaternion rotation)
    {
        GameObject drawing = GetDrawingById(id);
        drawing.transform.rotation = rotation;
    }

    public void AddToScaleDrawing(int id, Vector3 scaling)
    {
        CmdAddToScaleDrawing(id, scaling);
    }

    [Command]
    private void CmdAddToScaleDrawing(int id, Vector3 scaling)
    {
        GameObject drawing = GetDrawingById(id);
        float scaleX = drawing.transform.localScale.x + scaling.x;
        float scaleY = drawing.transform.localScale.y + scaling.y;
        float scaleZ = drawing.transform.localScale.z + scaling.z;
        Vector3 scalingVector = new Vector3(scaleX, scaleY, scaleZ);
        RpcAddToScaleDrawing(id, scalingVector);
    }

    [ClientRpc]
    private void RpcAddToScaleDrawing(int id, Vector3 scaling)
    {
        GameObject drawing = GetDrawingById(id);
        drawing.transform.localScale = scaling;
    }
    
    /*
    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        reader.SeekZero();
        base.OnDeserialize(reader, initialState);
    }
    */
}
