using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomKeyboardManager : MonoBehaviour {
    public InputField inputField;
    public void KeyboardButton()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText.text == "Löschen")
        {
            if (inputField.text.Length > 0)
            {
                inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
            }
        } else if(buttonText.text == "Enter")
        {
            Destroy(gameObject);
        }
        else
        {
            inputField.text += buttonText.text;
        }
    }

}