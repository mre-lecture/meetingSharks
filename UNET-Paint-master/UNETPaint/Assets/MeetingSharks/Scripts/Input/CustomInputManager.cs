using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputManager : MonoBehaviour {

    public static bool executeOnClick = true;

    public void OnTap()
    {
        if (executeOnClick)
        {
            DrawingManager dm = GameObject.FindGameObjectWithTag("localPlayer").GetComponent<DrawingManager>();
            dm.inputDown = true;
        }
        executeOnClick = true;
    }
}
