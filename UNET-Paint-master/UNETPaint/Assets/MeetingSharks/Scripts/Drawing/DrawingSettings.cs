using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingSettings : MonoBehaviour {

    [HideInInspector]
    public GameObject toMoveObject;
    [HideInInspector]
    public bool startMoving;
    
    private GameObject localPlayer;
    private List<string> drawingObjectnames;

    private void Start()
    {
        InitDrawingObjectsDropDown();
    }

    private void Update()
    {
        
    }

    private void InitDrawingObjectsDropDown()
    {
        drawingObjectnames = new List<string>();
        drawingObjectnames.Add("Line");
        Object[] drawingObjects = (Object[]) Resources.LoadAll("MeetingSharks/Prefabs/DrawingObjects", typeof(GameObject));
        GameObject dropdownGameObject = GameObject.Find("DrawingObjectsDropdown");
        if (dropdownGameObject)
        {
            foreach (Object drawingObject in drawingObjects)
            {
                drawingObjectnames.Add(drawingObject.name);
            }
            Dropdown dropdown = dropdownGameObject.GetComponent<Dropdown>();
            dropdown.AddOptions(drawingObjectnames);
        }
    }

    public void SetDrawingWidth(string width)
    {
        float parsedWidth;
        if (float.TryParse(width, out parsedWidth))
        {
            localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
            localPlayer.GetComponent<DrawingManager>().width = parsedWidth;
        }
    }

    public void SetDrawingDistance(string distance)
    {
        float parsedDistance;
        if (float.TryParse(distance, out parsedDistance))
        {
            localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
            localPlayer.GetComponent<DrawingManager>().drawingDistance = parsedDistance;
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

    private void SetMode(string mode)
    {
        DestroyAllSelectionBoxes();
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
