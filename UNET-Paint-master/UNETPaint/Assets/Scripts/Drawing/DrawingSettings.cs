using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DrawingSettings : NetworkBehaviour {

    private GameObject localPlayer;

    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                localPlayer = player;
            }
        }
    }

    public void SetDrawingWidth(float width)
    {
        if (localPlayer != null)
        {
            localPlayer.GetComponent<DrawingManager>().width = width;
        }
    }

    public void SetDrawingDistance(float distance)
    {
        if(localPlayer != null)
        {
            localPlayer.GetComponent<DrawingManager>().drawingDistance = distance;
        }
    }

}
