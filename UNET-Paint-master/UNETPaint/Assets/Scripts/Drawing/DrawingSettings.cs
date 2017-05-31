using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingSettings : MonoBehaviour {

    public GameObject toMoveObject;
    public string mode;
    public bool startMoving;
    private Vector3 lastCursorPos;
    private GameObject cursor;
    
    private GameObject localPlayer;
    private List<string> drawingObjectnames;
    private CustomInputManager customInputManager;

    private void Start()
    {
        mode = "drawing";
        cursor = GameObject.Find("Cursor");
        InitDrawingObjectsDropDown();
        customInputManager = GameObject.Find("DrawingSettings").GetComponent<CustomInputManager>();
    }

    private void Update()
    {
        if (startMoving && toMoveObject)
        {
            Vector3 currCursorPos = new Vector3(cursor.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
            float x = (currCursorPos.x - lastCursorPos.x) + toMoveObject.transform.position.x;
            float y = (currCursorPos.y - lastCursorPos.y) + toMoveObject.transform.position.y;
            float z = (currCursorPos.z - lastCursorPos.z) + toMoveObject.transform.position.z;
            localPlayer = GameObject.FindGameObjectWithTag("localPlayer");

            localPlayer.GetComponent<DrawingManager>().MoveDrawingTo(toMoveObject.GetComponent<DrawingInfo>().id, new Vector3(x, y, z));
            localPlayer.GetComponent<DrawingManager>().RotateDrawing(toMoveObject.GetComponent<DrawingInfo>().id, Camera.main.transform.rotation);
            lastCursorPos = currCursorPos;
        }
    }

    private void InitDrawingObjectsDropDown()
    {
        drawingObjectnames = new List<string>();
        drawingObjectnames.Add("Line");
        Object[] drawingObjects = (Object[]) Resources.LoadAll("Prefabs/DrawingObjects", typeof(GameObject));
        GameObject dropdownGameObject = GameObject.Find("DrawingObjectsDropdown");
        foreach(Object drawingObject in drawingObjects)
        {
            drawingObjectnames.Add(drawingObject.name);
        }
        Dropdown dropdown = dropdownGameObject.GetComponent<Dropdown>();
        dropdown.AddOptions(drawingObjectnames);
    }

    public void SetDrawingWidth(string width)
    {
        float parsedWidth;
        if (float.TryParse(width, out parsedWidth))
        {
            localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
            if (localPlayer != null)
            {
                localPlayer.GetComponent<DrawingManager>().width = parsedWidth;

            }
        }
    }

    public void SetDrawingDistance(string distance)
    {
        float parsedDistance;
        if (float.TryParse(distance, out parsedDistance))
        {
            localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
            if (localPlayer != null)
            {
                localPlayer.GetComponent<DrawingManager>().drawingDistance = parsedDistance;
            }
        }
    }

    public void OnDrawingObjectSelect(int value)
    {
        string selectedObject = drawingObjectnames[value];
        localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        localPlayer.GetComponent<DrawingManager>().drawingObjectName = selectedObject;
    }

    public void OnDelete()
    {
        localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        localPlayer.GetComponent<DrawingManager>().DeleteDrawings();
    }

    public void SetDrawingMode()
    {
        mode = "drawing";
        localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        localPlayer.GetComponent<DrawingManager>().mode = mode;
    }

    public void SetMovingMode()
    {
        mode = "moving";
        localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        localPlayer.GetComponent<DrawingManager>().mode = mode;
    }

    public void StartMoving(GameObject toMoveObject)
    {
        if (toMoveObject)
        {
            this.toMoveObject = toMoveObject;
            if (startMoving)
            {
                startMoving = false;
                toMoveObject = null;
            }
            else
            {
                if (mode == "moving")
                {
                    lastCursorPos = new Vector3(cursor.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
                    startMoving = true;
                }
            }
        }
    }

}
