using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region Public Attributes

    public Text usernameText;
    public GameObject activeGamePrefab;
    public GameObject activeGamesList;

    #endregion
    public void OnEnable() {
        usernameText.text = PlayerPrefs.GetString("username");
    }

    public void AddGame(GameData gameData) {
        GameObject activeGame = Instantiate(activeGamePrefab, activeGamesList.transform);
        activeGame.GetComponent<ActiveGame>().salaText.text = gameData.sala.ToString();
        activeGame.GetComponent<ActiveGame>().playersText.text = gameData.players.ToString();
    }
}
