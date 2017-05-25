using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputManager : MonoBehaviour {

    public void OnTap()
    {
        GameObject localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
        localPlayer.GetComponent<DrawingManager>().inputDown = true;
    }
}
