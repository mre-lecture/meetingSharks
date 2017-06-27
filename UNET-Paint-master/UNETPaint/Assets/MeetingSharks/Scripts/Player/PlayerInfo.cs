using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerInfo : NetworkBehaviour {

    [HideInInspector]
    [SyncVar (hook = "OnUsernameChange")]
    public string username;

    public GameObject usernameSlot;
    private GameObject usernameSlotInstance;
    private Text usernameText;
    private bool hasUsernameListPanel;

    private void Start()
    {
        // Checks if a panel exists to put the usernames into
        if(GameObject.Find("UsernamelistPanel"))
        {
            hasUsernameListPanel = true;
        }

        // randomly generate a username
        if (isLocalPlayer) {
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
        if (hasUsernameListPanel)
        {
            usernameSlotInstance = Instantiate(usernameSlot);
            usernameText = usernameSlotInstance.GetComponent<Text>();
            usernameText.text = username;
            usernameSlotInstance.transform.SetParent(usernamelistPanel.transform);
            usernameSlotInstance.transform.localScale = new Vector3(1, 1, 1);
            usernameSlotInstance.transform.localPosition = new Vector3(usernameSlotInstance.transform.localPosition.x, usernameSlotInstance.transform.localPosition.y, 0);
        }
    }

    private void OnDestroy()
    {
        // Can only destroy the usernameSlot when a panel exists which contains the slot
        if (hasUsernameListPanel)
        {
            Destroy(usernameSlotInstance);
        }
    }

}
