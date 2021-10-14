using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public InputField usernameInputField;
    public Text feedbackText;

    private OnlineManager onlineManager;
    // Start is called before the first frame update
    void Start()
    {
        onlineManager = OnlineManager.singleton;
        feedbackText.text = "";
    }

    public void CreateUser() {

        StartCoroutine(onlineManager.Signup(usernameInputField.text.ToString(), result => {
            if (result != null) {
                feedbackText.text = "User created.";
            }
        }));
    }
}
