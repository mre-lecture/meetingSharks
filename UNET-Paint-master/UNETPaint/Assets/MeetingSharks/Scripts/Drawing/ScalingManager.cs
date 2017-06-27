using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingManager : MonoBehaviour {

    public Vector3 lastCursorPos;
    public bool isScaling;
    private GameObject cursor;

    private int drawingId;

    private void Start()
    {
        drawingId = gameObject.transform.parent.GetComponent<DrawingInfo>().id;
        cursor = GameObject.Find("Cursor");
    }

    private void Update()
    {
        if(isScaling)
        {
            float distanceOldX = lastCursorPos.x - gameObject.transform.position.x;
            float distanceX = cursor.transform.position.x - gameObject.transform.position.x;
            float scaleX = distanceX - distanceOldX;

            float distanceOldY = lastCursorPos.y - gameObject.transform.position.y;
            float distanceY = cursor.transform.position.y - gameObject.transform.position.y;
            float scaleY = distanceY - distanceOldY;

            GameObject localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
            DrawingManager dm = localPlayer.GetComponent<DrawingManager>();
            dm.AddToScaleDrawing(drawingId, new Vector3(scaleX*2, scaleY*2, 0));
            lastCursorPos = cursor.transform.position;
        }
    }

}
