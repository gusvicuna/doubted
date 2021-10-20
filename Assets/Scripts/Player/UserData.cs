using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    #region Public Attributes

    public int id;
    public string username;
    public List<string> friends;
    public List<GameData> games = new List<GameData>();
    public string[] gamesInvited;
    public string[] friendsInvited;

    #endregion


    #region JSON

    public string Stringify() {
        return JsonUtility.ToJson(this);
    }

    public static UserData Parse(string json) {
        return JsonUtility.FromJson<UserData>(json);
    }

    #endregion

}