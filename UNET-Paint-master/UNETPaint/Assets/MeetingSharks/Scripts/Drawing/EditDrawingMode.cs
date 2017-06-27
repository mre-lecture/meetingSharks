using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditDrawingMode : MonoBehaviour {

    private GameObject o;
    private Renderer r;
    private Bounds b;
    private Material selectorM;
    private Material scaleM;
    private Material moveM;

	private void Start () {
        selectorM = EditDrawingManager.selectorMat;
        scaleM = EditDrawingManager.scaleMat;
        moveM = EditDrawingManager.moveMat;
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
        o.transform.localScale = new Vector3(length * 1.01f, width * 1.01f, height * 1.01f);

        o.AddComponent<DrawingMeshOnClick>();

        r = o.GetComponent<Renderer>();
        DrawingManager dm = GameObject.FindGameObjectWithTag("localPlayer").GetComponent<DrawingManager>();
        if (dm.mode == "scaling")
        {
            InitScaleMode();
        } else if(dm.mode == "moving")
        {
            InitMovingMode();
        }
        
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

    private void InitScaleMode()
    {
        List<GameObject> cornerObjects = CreateCornerObjects(scaleM);
        r.material = selectorM;

        o.AddComponent<ScalingManager>();
        foreach(GameObject go in cornerObjects)
        {
            go.AddComponent<ScaleHandlerClick>();
        }
    }

    private void InitMovingMode()
    {
        List<GameObject> cornerObjects = CreateCornerObjects(scaleM);
        r.material = moveM;
        o.AddComponent<MoveManager>();
        foreach (GameObject go in cornerObjects)
        {
            go.AddComponent<MoveHandlerClick>();
        }
    }

    private List<GameObject> CreateCornerObjects(Material mat)
    {
        List<GameObject> cornerObjects = new List<GameObject>();
        Vector3 p0 = b.min;
        Vector3 p1 = b.max;
        Vector3 p2 = new Vector3(p0.x, p0.y, p1.z);
        Vector3 p3 = new Vector3(p0.x, p1.y, p0.z);
        Vector3 p4 = new Vector3(p0.x, p1.y, p1.z);
        Vector3 p5 = new Vector3(p1.x, p0.y, p0.z);
        Vector3 p6 = new Vector3(p1.x, p0.y, p1.z);
        Vector3 p7 = new Vector3(p1.x, p1.y, p0.z);

        cornerObjects.Add(CreateEndPoints(p0, mat));
        cornerObjects.Add(CreateEndPoints(p1, mat));
        cornerObjects.Add(CreateEndPoints(p2, mat));
        cornerObjects.Add(CreateEndPoints(p3, mat));
        cornerObjects.Add(CreateEndPoints(p4, mat));
        cornerObjects.Add(CreateEndPoints(p5, mat));
        cornerObjects.Add(CreateEndPoints(p6, mat));
        cornerObjects.Add(CreateEndPoints(p7, mat));

        return cornerObjects;
    }

    private GameObject CreateEndPoints(Vector3 endPoint, Material mat)
    {
        GameObject cube = GameObject.CreatePrimitive(EditDrawingManager.myHandleType);
        float scale = o.transform.localScale.x * EditDrawingManager.myHandleScale;
        cube.transform.localScale = new Vector3(scale, scale, scale);
        cube.transform.position = endPoint;
        cube.transform.parent = o.transform;

        Renderer cRend = cube.GetComponent<Renderer>();
        cRend.material = mat;
        
        return cube;
    }

}
