using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public UserData user;
    public GameData game;

    public bool aceptationState;
    public List<pinta> dados;
    public bool haObligado;
    public string lastAction;

    public PlayerData() {
        dados = new List<pinta>();
    }
}
