﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerInfo : NetworkBehaviour {

    [SyncVar (hook = "OnUsernameChange")]
    public string username;

    public GameObject usernameSlot;
    private GameObject usernameSlotInstance;
    private Text usernameText;

    private void Start()
    {
        if(isLocalPlayer) {
            username = "User" + Random.Range(0, 1000000);
            CmdChangeUsername(username);
        }
        AddUsernameToUI();
    }


    [Command]
    void CmdChangeUsername(string newName)
    {
        username = newName;
    }

    [ClientRpc]
    void RpcChangeUsername(string newName)
    {
        username = newName;
    }

    void OnUsernameChange(string name)
    {
        if (usernameText != null)
        {
            usernameText.text = name;
        }
    }

    public void AddUsernameToUI()
    {
        GameObject usernamelistPanel = GameObject.Find("UsernamelistPanel");
        usernameSlotInstance = Instantiate(usernameSlot);
        usernameText = usernameSlotInstance.GetComponent<Text>();
        usernameText.text = username;
        usernameSlotInstance.transform.SetParent(usernamelistPanel.transform);
    }

    private void OnDestroy()
    {
        Destroy(usernameSlotInstance);
    }

}