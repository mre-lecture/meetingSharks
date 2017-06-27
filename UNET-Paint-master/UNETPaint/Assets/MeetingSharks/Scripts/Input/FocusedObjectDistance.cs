using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusedObjectDistance : MonoBehaviour {

    public bool drawOnObject;
    public Toggle drawOnObjectToggle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDrawingDistance(GameObject focusedObject)
    {
        if(drawOnObject)
        {
            GameObject cursor = GameObject.Find("Cursor");
            float distance = Vector3.Distance(Camera.main.transform.position, cursor.transform.position);
            GameObject localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
            DrawingManager drawingManager = localPlayer.GetComponent<DrawingManager>();
            drawingManager.drawingDistance = distance;
            //drawOnObjectToggle.isOn = false;
        }
    }

    public void SetDrawOnObject(bool drawOnObject)
    {
        this.drawOnObject = drawOnObject;
    }

}
