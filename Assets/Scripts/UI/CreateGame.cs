using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGame : MonoBehaviour
{
    #region Public Attributes

    public InputField nameInputField;
    public InputField passwordInputField;
    public Dropdown numberPlayersDropdown;

    public Text feedbackText;

    public MenuManager menuManager;
    public OnlineManager onlineManager;

    #endregion
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

    public void NewGame() {

        StartCoroutine(onlineManager.NewGame(menuManager.userData.id.ToString(), numberPlayersDropdown.captionText.text, nameInputField.text, result => {
            if (result != null) {
                feedbackText.text = "Game created.";
            }
        }));
    }
}
