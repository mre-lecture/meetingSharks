using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingSettings : MonoBehaviour {

    private GameObject localPlayer;

    private void Start()
    {
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

    public void OnDrawMesh(bool drawMesh)
    {
        localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        localPlayer.GetComponent<DrawingManager>().drawMesh = drawMesh;
    }

    public void OnDelete()
    {
        localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        localPlayer.GetComponent<DrawingManager>().DeleteDrawings();
    }

}
