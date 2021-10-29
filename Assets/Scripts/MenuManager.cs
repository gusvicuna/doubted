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
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> entry in result) {
                if (entry.Key != "current") {
                    UserData friendData = new UserData();
                    friendData.username = entry.Value;
                    userData.friends.Add(friendData);
                }
            }
        }));
    }
    public void UpdateFriendNotifications() {
        StartCoroutine(onlineManager.GetFriendNotifications(userData.id.ToString(), result => {
            userData.friendsInvited = new List<UserData>();
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> entry in result) {
                if (entry.Key != "current") {
                    UserData friendData = new UserData();
                    friendData.username = entry.Value;
                    userData.friendsInvited.Add(friendData);
                }
            }
        }));
    }

    //Friend Post and Update
    public void RequestFriend(UserData friend) {
        Debug.Log("Adding friend: " + friend.username);
        StartCoroutine(onlineManager.RequestFriend(userData.id.ToString(), friend.id.ToString(), result => {

        }));
    }
    public void AcceptFriendRequest(string username) {
        StartCoroutine(onlineManager.AcceptFriend(userData.id.ToString(), username, result => {

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
