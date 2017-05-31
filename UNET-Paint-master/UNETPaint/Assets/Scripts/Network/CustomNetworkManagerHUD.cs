using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CustomNetworkManagerHUD : MonoBehaviour {

    public GameObject ipAddressObject;
    public UnityEngine.TouchScreenKeyboard keyboard;

    void Start () {
    }

    private void Update()
    {
        if (TouchScreenKeyboard.visible == false && keyboard != null)
        {
            if (keyboard.done == true)
            {
                //keyboard.text;
                keyboard = null;
            }
        }
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
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
        NetworkManager.singleton.networkAddress = ip;
    }
}
