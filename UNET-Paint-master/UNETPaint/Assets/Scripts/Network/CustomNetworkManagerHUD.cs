using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetworkManagerHUD : MonoBehaviour {

    private CustomNetworkManager networkManager;
    private string ipAdress;

    void Start () {
        networkManager = gameObject.GetComponent<CustomNetworkManager>();
        ipAdress = networkManager.networkAddress;
    }

    private void OnGUI()
    {
        if (!networkManager.IsClientConnected())
        {
            ipAdress = GUILayout.TextField(ipAdress, GUILayout.Width(200));
            if (GUILayout.Button("Join"))
            {
                networkManager.networkAddress = ipAdress;
                networkManager.JoinRoom();
            }
            if (GUILayout.Button("Host"))
            {
                networkManager.StartupHost();
            }
        } else
        {
            if(GUILayout.Button("Back To Lobby")) {
                networkManager.LeaveRoom();
            }
        }
    }

}
