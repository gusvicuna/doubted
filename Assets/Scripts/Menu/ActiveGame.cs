using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActiveGame : MonoBehaviour
{
    public Text salaText;
    public Text playersText;
    public Text turnText;
    public GameObject joinButton;

    public GameData gameData;

    public MenuManager menuManager;

    public void Start()
    {
        menuManager = MenuManager.singleton;
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("currentGameId", (int)gameData.id);
        PlayerPrefs.SetInt("currentUserId", (int)menuManager.currentUserData.id);

        SceneManager.LoadScene("GameScene");
    }
}
