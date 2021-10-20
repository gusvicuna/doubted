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
    public void DoSomething() {
        StartCoroutine(UpdateData());
    }

    public IEnumerator UpdateData() {
        while (updating) {
            UpdateActiveGames();
            // TODO: UpdateFriends()
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
                    friendsPanel.AddFriend(entry.Value);
                }
            }
        }));
    }

    #endregion
}
