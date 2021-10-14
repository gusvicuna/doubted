using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGame : MonoBehaviour
{
    #region Public Attributes

    public InputField nameInputField;
    public InputField passwordInputField;
    public Text feedbackText;

    #endregion

    private OnlineManager onlineManager;
    private MenuManager menuManager;
    // Start is called before the first frame update
    void Start()
    {
        onlineManager = OnlineManager.singleton;
        menuManager = MenuManager.singleton;
        feedbackText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinSala() {
        StartCoroutine(onlineManager.JoinGame(menuManager.userData.id.ToString(),nameInputField.text, result => {
            if (result != null) {
                feedbackText.text = "Game joined.";
            }
        }));
    }
}
