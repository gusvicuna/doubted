using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    #region Public Attributes

    public long id;
    public string username;

    public List<UserData> friends;
    public List<UserData> friendsInvited;
    public List<GameData> games;
    public List<PlayerData> gamesInvited;

    public int wins;
    public int loses;
    public int dudasCorrectas;
    public int dudasIncorrectas;
    public int calzadasCorrectas;
    public int calzadasIncorrectas;

    #endregion

    public UserData() {
        GenerateLists();
    }

    public void GenerateLists()
    {
        friends = new List<UserData>();
        friendsInvited = new List<UserData>();
        games = new List<GameData>();
        gamesInvited = new List<PlayerData>();
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
