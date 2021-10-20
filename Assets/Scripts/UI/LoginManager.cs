using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
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
        loginUsername.onValueChanged.AddListener(delegate { SaveUsername(); });
    }

    #endregion

    #region Public Methods

    public void LogIn() {

        StartCoroutine(onlineManager.GetPlayerId(PlayerPrefs.GetString("username"), result => {
            if (result != null) {
                menuManager.userData.id = result["user_id"];
                menuManager.updating = true;
                menuManager.DoSomething();
                ChangePanels();
            }
        }));
    }

    #endregion


    #region Private Methods

    private void ChangePanels() {
        mainMenuPanel.SetActive(true);
        miniMenuPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void SaveUsername() {
        PlayerPrefs.SetString("username", loginUsername.text.ToString());
    }

    #endregion
}
