using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach (GameData gameData in _menuManager.currentUserData.games) {
            GameObject activeGame = Instantiate(activeGamePrefab, activeGamesList.transform);
            activeGame.GetComponent<ActiveGame>().gameData = gameData;
            activeGame.GetComponent<ActiveGame>().salaText.text = gameData.gameName;
            activeGame.GetComponent<ActiveGame>().playersText.text = gameData.maxPlayers.ToString();
            activeGame.GetComponent<ActiveGame>().turnText.text = gameData.playerTurn.ToString();
            bool gameReady = gameData.players.All(playerData => playerData.acceptationState);
            if(gameReady) activeGame.GetComponent<ActiveGame>().joinButton.SetActive(true);
        }
    }

}
