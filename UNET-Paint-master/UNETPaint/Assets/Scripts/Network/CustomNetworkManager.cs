using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    [HideInInspector]
    public delegate void ClientConnectAction();
    [HideInInspector]
    public delegate void ClientDisconnectAction();
    [HideInInspector]
    public delegate void HostStartAction();
    [HideInInspector]
    public delegate void HostStopAction();

    [HideInInspector]
    public static event ClientConnectAction OnClientConnectEvent;
    [HideInInspector]
    public static event ClientDisconnectAction OnClientDisconnectEvent;
    [HideInInspector]
    public static event HostStartAction OnHostStartEvent;
    [HideInInspector]
    public static event HostStopAction OnHostStopEvent;

    [SerializeField]
    private bool isHost;

    public void StartupHost()
    {
        StartHost();
    }

    public void JoinRoom()
    {
        StartClient();
    }

    public void LeaveRoom()
    {
        if(!isHost)
        {
            StopClient();
        } else
        {
            StopHost();
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        if(OnClientConnectEvent != null)
            OnClientConnectEvent();
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if(OnClientDisconnectEvent != null) 
            OnClientDisconnectEvent();
        base.OnClientDisconnect(conn);
    }

    public override void OnStartHost()
    {
        if(OnHostStartEvent != null)
            OnHostStartEvent();
        isHost = true;
        base.OnStartHost();
    }

    public override void OnStopHost()
    {
        if(OnHostStopEvent != null)
            OnHostStopEvent();
        isHost = false;
        base.OnStopHost();
    }

}
