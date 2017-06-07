using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using System;

public class KeyboeardInputFieldManager : MonoBehaviour,IInputClickHandler {
    public GameObject keyboardPrefab;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        GameObject keyboard = GameObject.FindGameObjectWithTag("Keyboard");
        if (keyboard)
        {

        }
        else
        {
            InputField inputField = gameObject.GetComponent<InputField>();
            keyboard = Instantiate(keyboardPrefab);
            GameObject canvas = GameObject.Find("Canvas");
            keyboard.transform.SetParent(canvas.transform);
            keyboard.transform.localPosition = new Vector3(1, -150, 0);
            keyboard.transform.rotation = new Quaternion(0, 0, 0, 0);
            keyboard.transform.localScale = new Vector3(1, 1, 1);
            keyboard.GetComponent<CustomKeyboardManager>().inputField = inputField;
        }
        
    }
}
