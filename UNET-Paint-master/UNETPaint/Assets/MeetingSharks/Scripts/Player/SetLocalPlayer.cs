using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetLocalPlayer : NetworkBehaviour {

	void Start () {
		if(isLocalPlayer)
        {
            gameObject.tag = "localPlayer";
        }
	}

}
