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
    public List<int> dados;
    public int dado1;
    public int dado2;
    public int dado3;
    public int dado4;
    public int dado5;
    public bool haObligado;
    public PredictionData lastPrediction;

    public PlayerData() {
        dados = new List<int>();
        lastPrediction = new PredictionData();
    }

    public string Stringify() {
        return JsonUtility.ToJson(this);
    }

    public static PlayerData Parse(string json) {
        return JsonUtility.FromJson<PlayerData>(json);
    }
}
