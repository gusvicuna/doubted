using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public long id;

    public string gameName;
    public string password;

    public int maxPlayers;
    public List<PlayerData> players;

    public int round;
    public int playerTurn;

    public bool gameStarted;
    public bool gameFinished;
    public bool newRound;

    public bool obligando;
    public bool canObligar;
    public bool canCalzar;

    public GameData() {
        players = new List<PlayerData>();
        newRound = true;
    }

    public string Stringify() {
        return JsonUtility.ToJson(this);
    }

    public static GameData Parse(string json) {
        return JsonUtility.FromJson<GameData>(json);
    }
}
