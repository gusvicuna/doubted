using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsPanel : MonoBehaviour
{
    #region Public Attributes

    public InputField usernameInputField;
    public Text feedbackText;

    #endregion

    private void Start() {
        feedbackText.text = "";
    }
}
