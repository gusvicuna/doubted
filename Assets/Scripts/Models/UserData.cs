using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    #region Public Attributes

    public int id;
    public string username;

    public List<UserData> friends;
    public List<UserData> friendsInvited;
    public List<GameData> games;
    public List<GameData> gamesInvited;

    #endregion

    public UserData() {
        friends = new List<UserData>();
        friendsInvited = new List<UserData>();
        games = new List<GameData>();
        gamesInvited = new List<GameData>();
    }

    #region JSON

    public string Stringify() {
        return JsonUtility.ToJson(this);
    }

    public static UserData Parse(string json) {
        return JsonUtility.FromJson<UserData>(json);
    }

    #endregion

}
