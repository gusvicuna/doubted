using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public string sala;
    public string password;

    public int totalPlayers;
    public List<PlayerData> players;

    public bool state;
    public int round;
    public PlayerData playerTurn;

    public GameData() {
        players = new List<PlayerData>();
    }
}
