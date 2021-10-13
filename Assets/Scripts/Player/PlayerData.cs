using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    #region Public Attributes

    public string username;

    #endregion


    #region JSON

    public string Stringify() {
        return JsonUtility.ToJson(this);
    }

    public static PlayerData Parse(string json) {
        return JsonUtility.FromJson<PlayerData>(json);
    }

    #endregion

}
