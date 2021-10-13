using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameManager : MonoBehaviour
{
    #region Public Attributes

    public InputField nameInputField;
    public InputField passwordInputField;
    public Text feedbackText;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        feedbackText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
