using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendNotification : MonoBehaviour
{

    public Text friendText;
    public UserData friendData;

    private MenuManager _menuManager;

    void Start() {
        _menuManager = MenuManager.singleton;
    }

    public void AcceptFriend() {
        _menuManager.AcceptFriendRequest(friendData);
        Destroy(this.gameObject);
    }

    public void DeclineFriend() {
        _menuManager.DeclineFriendRequest(friendText.text);
    }
}
