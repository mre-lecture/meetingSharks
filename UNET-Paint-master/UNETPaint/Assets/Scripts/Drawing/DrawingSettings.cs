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
        SetMode("drawing");
    }

    public void SetScaleMode()
    {
        SetMode("scaling");
    }

    public void SetMoveMode()
    {
        SetMode("moving");
    }

    public void SetRotateMode()
    {
        SetMode("rotating");
    }

    private void SetMode(string modeString)
    {
        DestroyAllSelectionBoxes();
        mode = modeString;
        localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        localPlayer.GetComponent<DrawingManager>().mode = mode;
    }

    private void DestroyAllSelectionBoxes()
    {
        GameObject[] selectionBoxes = GameObject.FindGameObjectsWithTag("SelectionBox");
        foreach(GameObject selectionBox in selectionBoxes)
        {
            Destroy(selectionBox);
        }
    }

}
