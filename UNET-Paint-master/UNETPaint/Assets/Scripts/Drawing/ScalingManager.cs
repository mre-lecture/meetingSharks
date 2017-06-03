using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingManager : MonoBehaviour {

    public bool isScaling;

    private int drawingId;

    private void Start()
    {
        drawingId = gameObject.transform.parent.GetComponent<DrawingInfo>().id;
    }

    private void Update()
    {
        if(isScaling)
        {

        }
    }

}
