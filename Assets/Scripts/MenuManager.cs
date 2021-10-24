using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region Public Attributes

    //Singleton
    public static MenuManager singleton;

    public UserData userData;

    public MainMenu mainMenu;
    public FriendsPanel friendsPanel;
    public Notifications notifications;
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

    //Update data
    public void DoSomething() {
        StartCoroutine(UpdateData());
    }

    public IEnumerator UpdateData() {
        while (updating) {
            UpdateActiveGames();
            // TODO: UpdateFriends();
            // TODO: UpdateFriendNotifications();
            yield return new WaitForSeconds(5);
        }
    }

    public void UpdateActiveGames() {
        StartCoroutine(onlineManager.GetPlayerGames(userData.id.ToString(), result => {
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> entry in result) {
                GameData gameData = new GameData();
                gameData.sala = entry.Value[0];
                gameData.players = entry.Value[1];
                if (!userData.games.Exists(x => x.sala == gameData.sala)) {
                    userData.games.Add(gameData);
                    mainMenu.AddGame(gameData);
                }
            }
        }));
    }
    public void UpdateFriends() {
        StartCoroutine(onlineManager.GetPlayerFriends(userData.id.ToString(), result => {
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> entry in result) {
                if (!userData.friends.Exists(x => x == entry.Value)) {
                    userData.friends.Add(entry.Value);
                    friendsPanel.ShowFriend(entry.Value);
                }
            }
        }));
    }
    public void UpdateFriendNotifications() {
        StartCoroutine(onlineManager.GetFriendNotifications(userData.id.ToString(), result => {
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> entry in result) {
                if (!userData.friendsInvited.Exists(x => x == entry.Value)) {
                    userData.friendsInvited.Add(entry.Value);
                }
            }
        }));
    }

    //Friend Post and Update
    public void RequestFriend(string username) {
        Debug.Log("Adding friend: " + username);
        //TODO: Uncomment when database ready
        /*
        StartCoroutine(onlineManager.AddFriend(userData.id.ToString(), username, result => {
            foreach (KeyValuePair<string, SimpleJSON.JSONNode> entry in result) {
                if (!userData.friends.Exists(x => x == entry.Value)) {
                    userData.friends.Add(entry.Value);
                    friendsPanel.ShowFriend(entry.Value);
                }
            }
        }));
        */
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
