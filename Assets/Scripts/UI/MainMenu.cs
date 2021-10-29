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

    private MenuManager _menuManager;

    #endregion
    public void OnEnable() {
        usernameText.text = PlayerPrefs.GetString("username");
        _menuManager = MenuManager.singleton;

        UpdateGames();
    }

    public void UpdateGames() {
        Debug.Log("Updating games");
        foreach (Transform child in activeGamesList.transform) {
            Destroy(child.gameObject);
        }
        foreach (GameData gameData in _menuManager.userData.games) {
            GameObject activeGame = Instantiate(activeGamePrefab, activeGamesList.transform);
            activeGame.GetComponent<ActiveGame>().salaText.text = gameData.sala.ToString();
            activeGame.GetComponent<ActiveGame>().playersText.text = gameData.totalPlayers.ToString();
        }
    }

}
