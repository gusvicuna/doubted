using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Public Attributes

    //Singleton
    [HideInInspector]
    public static MenuManager singleton;

    [HideInInspector]
    public UserData currentUserData;

    [Header("Panels:")] 
    public MainMenu mainMenu;
    public Login loginMenu;

    [HideInInspector]
    public bool updating = false;

    #endregion


    #region Private Attributes

    private OnlineManager onlineManager;

    #endregion


    #region MonoBehaviour Callbacks

    void Start()
    {
        //Singleton
        if (singleton == null) singleton = this;

        currentUserData = new UserData();
        
        onlineManager = OnlineManager.singleton;
    }

    #endregion


    #region Public Methods

    public void LogOut() {
        PlayerPrefs.SetInt("logedIn", 0);
        currentUserData = new UserData();
        updating = false;
    }

    public void LogIn() {
        StartCoroutine(onlineManager.GetUser(PlayerPrefs.GetString("username"), result => {
            if (result == null) return;

            currentUserData.id = result["id"];
            currentUserData.username = result["username"];
            currentUserData.GenerateLists();

            updating = true;
            UpdateDatas();
            PlayerPrefs.SetInt("logedIn", 1);

            loginMenu.ChangePanels();
        }));
    }

    //Update data

    public void UpdateDatas() {
        StartCoroutine(UpdateData());
    }
    public IEnumerator UpdateData() {
        while (updating) {
            UpdateActiveGames();
            UpdateFriends();
            UpdateFriendNotifications();
            UpdateGameNotifications();
            yield return new WaitForSeconds(5);
        }
    }

    public void UpdateActiveGames() {
        StartCoroutine(onlineManager.GetUserGames(currentUserData.id.ToString(), result => {
            currentUserData.games = new List<GameData>();
            foreach (SimpleJSON.JSONNode game in result)
            {
                GameData gameData = new GameData();
                gameData.id = game["id"];
                gameData.gameName = game["gameName"];
                gameData.playerTurn = game["playerTurn"];
                gameData.maxPlayers = game["maxPlayers"];
                currentUserData.games.Add(gameData);
            }
            mainMenu.UpdateGames();
        }));
    }
    public void UpdateFriends() {
        StartCoroutine(onlineManager.GetUserFriends(currentUserData.id.ToString(), result => {
            currentUserData.friends = new List<UserData>();
            foreach (SimpleJSON.JSONNode friendship in result)
            {
                UserData friendData = new UserData();
                if (friendship["invitatorUser"]["username"] == currentUserData.username)
                {
                    friendData.username = friendship["invitatedUser"]["username"];
                    friendData.id = friendship["invitatedUser"]["id"];
                }
                else
                {
                    friendData.username = friendship["invitatorUser"]["username"];
                    friendData.id = friendship["invitatorUser"]["id"];
                }
                currentUserData.friends.Add(friendData);
            }
        }));
    }
    public void UpdateFriendNotifications() {
        StartCoroutine(onlineManager.GetFriendNotifications(currentUserData.id.ToString(), result => {
            currentUserData.friendsInvited = new List<UserData>();
            foreach (SimpleJSON.JSONNode friendship in result)
            {
                UserData friendData = new UserData();
                friendData.username = friendship["invitatorUser"]["username"];
                friendData.id = friendship["invitatorUser"]["id"];
                currentUserData.friendsInvited.Add(friendData);
            }
        }));
    }

    public void UpdateGameNotifications() {
        StartCoroutine(onlineManager.GetGameNotifications(currentUserData.id.ToString(), result => {
            currentUserData.gamesInvited = new List<PlayerData>();
            foreach (SimpleJSON.JSONNode player in result) {
                PlayerData playerData = new PlayerData();
                playerData.gameId = player["gameId"];
                playerData.userId = player["userId"];
                playerData.id = player["id"];
                currentUserData.gamesInvited.Add(playerData);
            }
        }));
    }

    //Friend Post and Update
    public void RequestFriend(UserData friend) {
        StartCoroutine(onlineManager.RequestFriend(currentUserData.id.ToString(), friend.Stringify(), result => {

        }));
    }
    public void AcceptFriendRequest(UserData friend) {
        StartCoroutine(onlineManager.AcceptFriend(currentUserData.id.ToString(), friend.id.ToString(), result => {

        }));
    }
    public void RemoveFriendship(UserData friendData) {
        StartCoroutine(onlineManager.DeleteFriendship(currentUserData.id.ToString(), friendData.id.ToString(), result => {

        }));
        
    }

    //Game Post and Update
    public void AcceptGameRequest(PlayerData playerData) {

        StartCoroutine(onlineManager.PutPlayer(playerData.id.ToString(),playerData.Stringify(), result => {

        }));
    }
    public void DeclineGameRequest(PlayerData playerData) {

        StartCoroutine(onlineManager.DeclineGame(playerData.id.ToString(), result => {

        }));
    }



    #endregion
}
