using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region Public Attributes

    public Text usernameText;

    #endregion
    public void OnEnable() {
        usernameText.text = PlayerPrefs.GetString("username");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
