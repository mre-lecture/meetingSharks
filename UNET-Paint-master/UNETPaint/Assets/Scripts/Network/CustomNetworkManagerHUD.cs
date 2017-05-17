﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManagerHUD : MonoBehaviour {

    void Start () {
    }

    public void StartHost()
    {
        NetworkManager.singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.singleton.StartClient();
    }

    public void SetNetworkAddress(string ip)
    {
        NetworkManager.singleton.networkAddress = ip;
    }
}
