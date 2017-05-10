using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DrawingManager : NetworkBehaviour {

    public GameObject drawingPrefab;
    GameObject thisDrawing;
    Vector3 startPos;
    Plane objPlane;

	void Start () {
        objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
        GameObject[] drawings = GameObject.FindGameObjectsWithTag("Drawing");
        foreach(GameObject drawing in drawings)
        {
            DrawingInfo identity = drawing.GetComponent<DrawingInfo>();
            if(isLocalPlayer)
                CmdGetPointsForDrawing(identity.id);
        }
    }
	
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            if (isLocalPlayer)
            {
                thisDrawing = (GameObject)Instantiate(drawingPrefab, this.transform.position,
                    Quaternion.identity);
                int newDrawingId = GetLatestDrawingId() + 1;
                thisDrawing.GetComponent<DrawingInfo>().id = newDrawingId;
                Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    startPos = mRay.GetPoint(rayDistance);
                }

                CmdInstantiateDrawing(newDrawingId);
            }
        } else if(Input.GetMouseButton(0))
        {
            if (isLocalPlayer)
            {
                Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                if (objPlane.Raycast(mRay, out rayDistance))
                {
                    Vector3 point = mRay.GetPoint(rayDistance);
                    thisDrawing.transform.position = point;

                    CmdDrawToPoint(thisDrawing.GetComponent<DrawingInfo>().id, point);
                }
            }
        } else if(Input.GetMouseButtonUp(0))
        {
            if(thisDrawing != null && Vector3.Distance(thisDrawing.transform.position, startPos) < 0.1)
            {
                if (isLocalPlayer)
                {
                    Destroy(thisDrawing);

                    CmdDestroyDrawing(thisDrawing.GetComponent<DrawingInfo>().id);
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
    void CmdInstantiateDrawing(int id)
    {
        GameObject drawing = (GameObject)Instantiate(drawingPrefab, this.transform.position,
                Quaternion.identity);
        drawing.GetComponent<DrawingInfo>().id = id;
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


}
