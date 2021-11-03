using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    #region Public Attributes

    public InputField loginUsername;
    public Text feedbackText;

    public GameObject mainMenuPanel;
    public GameObject miniMenuPanel;

    public MenuManager menuManager;
    public OnlineManager onlineManager;

    #endregion


    #region MonoBehaviour Callbacks

    void Start()
    {
        menuManager = MenuManager.singleton;
        onlineManager = OnlineManager.singleton;

        feedbackText.text = "";

        if (PlayerPrefs.GetString("username") != null) {
            loginUsername.text = PlayerPrefs.GetString("username");
        }
        if (PlayerPrefs.GetInt("logedIn") == 1) {
            menuManager.LogIn();
        }
        loginUsername.onValueChanged.AddListener(delegate { SaveUsername(); });
    }

    #endregion

    public void ChangePanels() {
        mainMenuPanel.SetActive(true);
        miniMenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }


    #region Private Methods

    private void SaveUsername() {
        PlayerPrefs.SetString("username", loginUsername.text.ToLower());
    }

    #endregion
}
