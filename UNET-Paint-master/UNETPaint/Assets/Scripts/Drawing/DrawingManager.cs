using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class DrawingManager : NetworkBehaviour
{

    private GameObject cursor;
    private int thisDrawingId = 0;
    private Plane objPlane;
    private List<GameObject> drawingMeshes;
    
    public float drawingDistance;
    public Color color;
    public float width;
    public bool drawMesh;

    public bool inputDown;
    public bool inputUp;
    public bool pressing;

	void Start () {
        objPlane = new Plane(GetNormalForPlane(), GetPositionForPlane());
        cursor = GameObject.Find("Cursor");
        drawingMeshes = new List<GameObject>();

        drawingDistance = 10;
        color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
        width = 0.1f;
    }
	
	void Update () {
        if ((!pressing && inputDown))
        {
            inputDown = false;
            pressing = true;
            if (isLocalPlayer)
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
                    CmdInstantiateDrawing(thisDrawingId, lastPos, color, width, drawMesh, playerInfo.username);
                }
                
            }
        } else if((pressing && !inputDown))
        {
            inputDown = false;
            if (isLocalPlayer)
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
            if (isLocalPlayer)
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
    void CmdInstantiateDrawing(int id, Vector3 lastPos, Color color, float width, bool drawMesh, string username)
    {
        GameObject drawing = (GameObject)Instantiate(Resources.Load("Prefabs/DrawingObjects/Drawing"), this.transform.position,
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
        drawingInfo.drawMesh = drawMesh;
        drawingInfo.username = username;

        /*
        if(drawMesh)
        {
            drawingTrailRenderer.enabled = false;
        } else
        {
            drawingTrailRenderer.enabled = true;
        }
        */

        NetworkServer.Spawn(drawing);
    }

    [Command]
    void CmdDrawToPoint(int id, Vector3 point, Color color)
    {
        GameObject drawing = GetDrawingById(id);
        if (drawing != null)
        {
            DrawingInfo drawingInfo = drawing.GetComponent<DrawingInfo>();
            drawing.GetComponent<TrailRenderer>().enabled = !drawingInfo.drawMesh;
            if (Vector3.Distance(drawingInfo.lastPos, point) > 0.1)
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
            drawingInfo.points.Add(point);
            if (drawingInfo.drawMesh)
            {
                GameObject cube = (GameObject)Instantiate(Resources.Load("Prefabs/DrawingObjects/Cube"));
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

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        base.OnDeserialize(reader, initialState);
    }

}
