using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public InputField usernameInputField;
    public Text feedbackText;
    // Start is called before the first frame update
    void Start()
    {
        feedbackText.text = "";
    }

    public void CreateUser() {

        //TODO: Save User...
    }
}
