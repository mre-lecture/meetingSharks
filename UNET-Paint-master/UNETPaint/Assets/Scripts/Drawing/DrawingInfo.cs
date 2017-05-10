using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DrawingInfo : NetworkBehaviour {

    [SyncVar]
    public int id;
    public List<Vector3> points;

}
