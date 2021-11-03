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
    public UserData userData;

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

        userData = new UserData();
        
        onlineManager = OnlineManager.singleton;
    }

    #endregion


    #region Public Methods

    public void LogOut() {
        PlayerPrefs.SetInt("logedIn", 0);
        userData = new UserData();
        updating = false;
    }

    public void LogIn() {
        StartCoroutine(onlineManager.GetPlayer(PlayerPrefs.GetString("username"), result => {
            if (result == null) return;

            userData.id = result["id"];
            userData.username = result["username"];
            userData.GenerateLists();

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
            yield return new WaitForSeconds(5);
        }
    }

    public void UpdateActiveGames() {
        StartCoroutine(onlineManager.GetPlayerGames(userData.id.ToString(), result => {
            userData.games = new List<GameData>();
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> entry in result) {
                GameData gameData = new GameData();
                gameData.sala = entry.Value[0];
                gameData.totalPlayers = entry.Value[1];
                userData.games.Add(gameData);
            }
        }));
    }
    public void UpdateFriends() {
        StartCoroutine(onlineManager.GetPlayerFriends(userData.id.ToString(), result => {
            userData.friends = new List<UserData>();
            Debug.Log(result);
            foreach (SimpleJSON.JSONNode friendship in result)
            {
                UserData friendData = new UserData();
                if (friendship["invitatorUser"]["username"] == userData.username)
                {
                    friendData.username = friendship["invitatedUser"]["username"];
                    friendData.id = friendship["invitatedUser"]["id"];
                }
                else
                {
                    friendData.username = friendship["invitatorUser"]["username"];
                    friendData.id = friendship["invitatorUser"]["id"];
                }
                userData.friends.Add(friendData);
            }
        }));
    }
    public void UpdateFriendNotifications() {
        StartCoroutine(onlineManager.GetFriendNotifications(userData.id.ToString(), result => {
            userData.friendsInvited = new List<UserData>();
            foreach (SimpleJSON.JSONNode friendship in result)
            {
                UserData friendData = new UserData();
                friendData.username = friendship["invitatorUser"]["username"];
                friendData.id = friendship["invitatorUser"]["id"];
                userData.friendsInvited.Add(friendData);
            }
        }));
    }

    //Friend Post and Update
    public void RequestFriend(UserData friend) {
        StartCoroutine(onlineManager.RequestFriend(userData.id.ToString(), friend.Stringify(), result => {

        }));
    }
    public void AcceptFriendRequest(UserData friend) {
        StartCoroutine(onlineManager.AcceptFriend(userData.id.ToString(), friend.id.ToString(), result => {

        }));
    }
    public void DeclineFriendRequest(string username) {
        //TODO: Uncomment when database ready
        /*
        StartCoroutine(onlineManager.DeclineFriend(userData.id.ToString(), username, result => {

        }));
        */
    }

    //Game Post and Update
    public void AcceptGameRequest(string gameId) {
        //TODO: Uncomment when database ready
        /*
        StartCoroutine(onlineManager.AcceptGame(userData.id.ToString(), gameId, result => {

        }));
        */
    }
    public void DeclineGameRequest(string gameId) {
        //TODO: Uncomment when database ready
        /*
        StartCoroutine(onlineManager.DeclineGame(userData.id.ToString(), gameId, result => {

        }));
        */
    }



    #endregion
}
