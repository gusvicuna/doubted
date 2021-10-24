using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNotification : MonoBehaviour
{
    public Text gameIdText;

    private MenuManager _menuManager;

    void Start() {
        _menuManager = MenuManager.singleton;
    }

    public void AcceptGame() {
        _menuManager.AcceptGameRequest(gameIdText.text);
    }
    public void DeclineGame() {
        _menuManager.DeclineGameRequest(gameIdText.text);
    }
}
