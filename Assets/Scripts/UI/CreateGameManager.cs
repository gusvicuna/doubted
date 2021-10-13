using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGameManager : MonoBehaviour
{
    #region Public Attributes

    public InputField nameInputField;
    public InputField passwordInputField;
    public Dropdown numberPlayersDropdown;

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

    public void CreateGame() {

        //TODO: create game...
    }
}
