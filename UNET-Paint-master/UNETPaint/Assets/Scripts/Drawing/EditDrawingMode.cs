using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditDrawingMode : MonoBehaviour {

    private GameObject o;
    private Renderer r;
    private Bounds b;
    private Material selectorM;
    private Material scaleM;

	private void Start () {
        selectorM = EditDrawingManager.selectorMat;
        scaleM = EditDrawingManager.scaleMat;
	}

    public void OnSelect()
    {
        if(!o)
        {
            DrawSelectionBox();
        } else
        {
            Destroy(o);
        }
    }

    void DrawSelectionBox()
    {
        b = GetBounds();
        float length = b.size.x;
        float width = b.size.y;
        float height = b.size.z;

        o = GameObject.CreatePrimitive(PrimitiveType.Cube);
        o.tag = "SelectionBox";
        o.transform.position = b.center;
        o.transform.parent = gameObject.transform;
        o.transform.localScale = new Vector3(length * 1.1f, width * 1.1f, height * 1.1f);
        Vector3 p0 = b.min;
        Vector3 p1 = b.max;
        Vector3 p2 = new Vector3(p0.x, p0.y, p1.z);
        Vector3 p3 = new Vector3(p0.x, p1.y, p0.z);
        Vector3 p4 = new Vector3(p0.x, p1.y, p1.z);
        Vector3 p5 = new Vector3(p1.x, p0.y, p0.z);
        Vector3 p6 = new Vector3(p1.x, p0.y, p1.z);
        Vector3 p7 = new Vector3(p1.x, p1.y, p0.z);

        CreateEndPoints(p0);
        CreateEndPoints(p1);
        CreateEndPoints(p2);
        CreateEndPoints(p3);
        CreateEndPoints(p4);
        CreateEndPoints(p5);
        CreateEndPoints(p6);
        CreateEndPoints(p7);

        r = o.GetComponent<Renderer>();
        r.material = selectorM;


        o.AddComponent<DrawingMeshOnClick>();
    }

    private Bounds GetBounds()
    {
        Vector3 center = Vector3.zero;
        foreach (Transform child in transform)
        {
            center += child.gameObject.GetComponent<Renderer>().bounds.center;
        }
        center /= transform.childCount;
        Bounds bounds = new Bounds(center, Vector3.zero);
        foreach (Transform child in transform)
        {
            bounds.Encapsulate(child.gameObject.GetComponent<Renderer>().bounds);
        }
        return bounds;
    }

    private void CreateEndPoints(Vector3 endPoint)
    {
        GameObject cube = GameObject.CreatePrimitive(EditDrawingManager.myHandleType);
        float scale = o.transform.localScale.x * EditDrawingManager.myHandleScale;
        cube.transform.localScale = new Vector3(scale, scale, scale);
        cube.transform.position = endPoint;
        cube.transform.parent = o.transform;

        Renderer cRend = cube.GetComponent<Renderer>();
        cRend.material = scaleM;

        cube.AddComponent<ScaleHandlerClick>();
    }

}
