using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditDrawingManager : MonoBehaviour {

    public Color selectorColor = new Color32(98, 99, 255, 128);

    public PrimitiveType handleType;
    public float handleScale = 0.2f;
    public Color scaleColor = new Color32(99, 159, 255, 128);

    public Color moveSelectorColor = new Color32(0, 155, 0, 128);

    GameObject selectorBox;
    GameObject scaleHandleBox;
    GameObject moveSelectorBox;

    public static Material selectorMat;
    public static Material scaleMat;
    public static Material moveMat;
    public static PrimitiveType myHandleType;
    public static float myHandleScale;

    private void Start()
    {
        myHandleType = handleType;
        myHandleScale = handleScale;

        selectorBox = GameObject.Find("SelectorMaterial");
        scaleHandleBox = GameObject.Find("ScaleMaterial");
        moveSelectorBox = GameObject.Find("MoveMaterial");

        selectorMat = selectorBox.GetComponent<Renderer>().material;
        selectorMat.color = selectorColor;

        scaleMat = scaleHandleBox.GetComponent<Renderer>().material;
        scaleMat.color = scaleColor;

        moveMat = moveSelectorBox.GetComponent<Renderer>().material;
        moveMat.color = moveSelectorColor;
    }

}
