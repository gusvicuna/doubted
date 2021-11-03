using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Signup : MonoBehaviour
{
    public InputField usernameInputField;
    public Text feedbackText;

    private OnlineManager _onlineManager;
    // Start is called before the first frame update
    void Start()
    {
        _onlineManager = OnlineManager.singleton;
        feedbackText.text = "";
    }

    public void CreateUser()
    {
        UserData newUser = new UserData();
        newUser.username = usernameInputField.text.ToLower();
        StartCoroutine(_onlineManager.SignUp(newUser.Stringify(), result => {
            if (result != null) {
                feedbackText.text = "User created.";
            }
        }));
    }
}
