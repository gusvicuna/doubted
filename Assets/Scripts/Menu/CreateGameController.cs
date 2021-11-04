using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateGameController : MonoBehaviour
{
    #region Public Attributes

    [Header("Child GameObjects")]
    public InputField nameInputField;
    public InputField passwordInputField;
    public Dropdown numberPlayersDropdown;
    public GameObject playersPanel;
    public GameObject addFriendsScroll;
    public Text feedbackText;
    public Toggle canCalzarToggle;
    public Toggle obligarToggle;

    [Header("Prefabs:")]
    public GameObject missingPlayerPanelPrefab;
    public GameObject addFriendPrefab;

    public GameData gameData;

    #endregion


    #region Private Attributes

    //Singletons
    private MenuManager _menuManager;
    private OnlineManager _onlineManager;

    #endregion


    #region MonoBehaviour Callbacks

    void OnEnable()
    {
        //Singletons
        _onlineManager = OnlineManager.singleton;
        _menuManager = MenuManager.singleton;

        feedbackText.text = "";

        //Create GameData for the game
        gameData = new GameData();

        PlayerData playerData = new PlayerData();
        playerData.user = _menuManager.currentUserData;
        playerData.userId = _menuManager.currentUserData.id;
        playerData.acceptationState = true;

        gameData.players.Add(playerData);


        //Add listener to dropdown
        numberPlayersDropdown.onValueChanged.AddListener(delegate { SetNumberOfPlayers(); });
        SetNumberOfPlayers();
    }

    private void OnDisable() {
        numberPlayersDropdown.onValueChanged.RemoveAllListeners();
    }

    #endregion


    #region Public Methods
    public void CreateNewGame()
    {
        StartCoroutine(ICreateNewGame());
    }

    public IEnumerator ICreateNewGame()
    {
        if (canCalzarToggle.isOn) gameData.canCalzar = true;
        if (obligarToggle.isOn) gameData.canObligar = true;
        gameData.gameName = nameInputField.text;
        gameData.password = passwordInputField.text;
        gameData.maxPlayers = gameData.players.Count;

        StartCoroutine(_onlineManager.NewGame(gameData.Stringify(), result => {
            Debug.Log(result);
            gameData.id = result["id"];
        }));

        yield return new WaitForSeconds(5);

        foreach (var playerData in gameData.players) {
            playerData.gameId = gameData.id;
            Debug.Log(playerData.Stringify());
            StartCoroutine(_onlineManager.NewPlayer(playerData.Stringify()));
        }
    }

    public void AddPlayer(UserData userData) {
        PlayerData playerData = new PlayerData();
        playerData.user = userData;
        playerData.userId = userData.id;
        gameData.players.Add(playerData);



        addFriendsScroll.SetActive(false);
    }

    #endregion


    #region Private Methods
    private void SetNumberOfPlayers() {
        //Set total players on gameData
        gameData.maxPlayers = Int32.Parse(numberPlayersDropdown.captionText.text);

        //clear playersPanel
        foreach (Transform child in playersPanel.transform) {
            Destroy(child.gameObject);
        }
        //Add missingPlayerPanels to playersPanel
        for (int i = 0; i < gameData.maxPlayers - 1; i++) {
            GameObject missingPlayerPanel = Instantiate(missingPlayerPanelPrefab, playersPanel.transform);
            missingPlayerPanel.transform.Find("Add Player Button").GetComponent<Button>().onClick.AddListener(OpenFriendsPanel);
        }
    }

    private void OpenFriendsPanel() {
        addFriendsScroll.SetActive(true);
        foreach (Transform child in addFriendsScroll.transform.Find("Panel").transform.Find("Friends Panel").transform) {
            child.GetComponent<AddFriendPanel>().myDelegate -= AddPlayer;
            Destroy(child.gameObject);
        }
        foreach (UserData friend in _menuManager.currentUserData.friends) {
            GameObject friendPanel = Instantiate(addFriendPrefab, addFriendsScroll.transform.Find("Panel").transform.Find("Friends Panel").transform);
            friendPanel.GetComponent<AddFriendPanel>().userData = friend;
            friendPanel.GetComponent<AddFriendPanel>().myDelegate += AddPlayer;
        }
    }

    #endregion
}
