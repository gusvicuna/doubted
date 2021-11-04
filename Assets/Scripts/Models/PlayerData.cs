using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public long id;

    public UserData user;
    public GameData game;

    public long userId;
    public long gameId;

    public int turnNumber;
    public string name;
    public bool acceptationState;
    public List<pinta> dice;
    public bool haObligado;
    public PredictionData lastPrediction;

    public PlayerData() {
        dice = new List<pinta>();
        lastPrediction = new PredictionData();
    }

    public string Stringify() {
        return JsonUtility.ToJson(this);
    }

    public static PlayerData Parse(string json) {
        return JsonUtility.FromJson<PlayerData>(json);
    }
}
