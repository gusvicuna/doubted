using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNotification : MonoBehaviour
{
    public Text gameIdText;

    public PlayerData playerData;

    private MenuManager _menuManager;

    void Start() {
        _menuManager = MenuManager.singleton;
    }

    public void AcceptGame()
    {
        playerData.acceptationState = true;
        _menuManager.AcceptGameRequest(playerData);
    }
    public void DeclineGame() {
        _menuManager.DeclineGameRequest(playerData);
    }
}
