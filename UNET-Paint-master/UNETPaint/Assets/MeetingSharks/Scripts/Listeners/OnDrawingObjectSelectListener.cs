using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnDrawingObjectSelectListener : MonoBehaviour {

	void Start () {
        GameObject drawingSettings = GameObject.Find("DrawingSettings");
        if (drawingSettings)
        {
            Dropdown dropdown = gameObject.GetComponent<Dropdown>();
            dropdown.onValueChanged.AddListener(drawingSettings.GetComponent<DrawingSettings>().OnDrawingObjectSelect);
        }
	}
}
