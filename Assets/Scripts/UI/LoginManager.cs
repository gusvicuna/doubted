using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField loginUsername;
    public Text feedbackText;

    public GameObject mainMenuPanel;
    public GameObject miniMenuPanel;

    // Start is called before the first frame update
    void Start()
    {
        feedbackText.text = "";

        if (PlayerPrefs.GetString("username") != null) {
            loginUsername.text = PlayerPrefs.GetString("username");
        }
        loginUsername.onValueChanged.AddListener(delegate { SaveUsername(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveUsername() {
        PlayerPrefs.SetString("username", loginUsername.text.ToString());
    }

    public void LogIn() {

        //TODO: Load user...

        mainMenuPanel.SetActive(true);
        miniMenuPanel.SetActive(true);
        this.gameObject.SetActive(false);

    }
}
