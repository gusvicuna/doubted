using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddFriendPanel : MonoBehaviour
{
    public UserData userData;

    //Delegate
    [HideInInspector]
    public delegate void OnAddFriendCLickedDelegate(UserData userData);
    [HideInInspector]
    public OnAddFriendCLickedDelegate myDelegate;

    private Text _friendUsernameText;

    void Start()
    {
        _friendUsernameText.text = userData.username;
    }

    public void AddFriendToGame() {
        myDelegate(userData);
    }
}
