using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsPanel : MonoBehaviour
{
    #region Public Attributes

    public InputField usernameInputField;
    public Text feedbackText;
    public Text friendSearchText;
    public Button addFriendButton;

    private UserData userSearched;

    public GameObject friendPanelPrefab;
    public GameObject friendsList;

    private MenuManager _menuManager;
    private OnlineManager onlineManager;
    #endregion


    #region MonoBehaviour Callbacks

    private void OnEnable() {
        _menuManager = MenuManager.singleton;
        onlineManager = OnlineManager.singleton;

        feedbackText.text = "";
        friendSearchText.text = "";
        addFriendButton.gameObject.SetActive(false);

        UpdateFriends();
    }

    #endregion


    #region Public Methods

    public void UpdateFriends() {
        Debug.Log("Updating friends");
        foreach (Transform child in friendsList.transform) {
            Destroy(child.gameObject);
        }
        foreach (UserData friend in _menuManager.currentUserData.friends) {
            GameObject friendPanel = Instantiate(friendPanelPrefab, friendsList.transform);
            friendPanel.GetComponentInChildren<Text>().text = friend.username;
            friendPanel.GetComponent<Friend>().friendData = friend;
            friendPanel.GetComponent<Friend>().myDelegate += RemoveFriend;
        }
    }

    public void AddFriend() {
        _menuManager.RequestFriend(userSearched);
    }

    public void RemoveFriend(UserData FriendData)
    {
        _menuManager.currentUserData.friends.Remove(FriendData);
        _menuManager.RemoveFriendship(FriendData);
    }

    public void SearchFriend() {
        StartCoroutine(onlineManager.GetUser(usernameInputField.text, result => {
            if (result != null)
            {
                Debug.Log(usernameInputField.text);
                if(usernameInputField.text == _menuManager.currentUserData.username) {
                    feedbackText.text = "You are that user.";
                }
                else {
                    foreach (var userDataFriend in _menuManager.currentUserData.friends)
                    {
                        if (usernameInputField.text == userDataFriend.username)
                        {
                            feedbackText.text = "You already have that friend.";
                            return;
                        }
                    }
                    userSearched = new UserData();
                    feedbackText.text = "";
                    userSearched.id = result["id"];
                    userSearched.username = usernameInputField.text;
                    friendSearchText.text = userSearched.username;
                    addFriendButton.gameObject.SetActive(true);
                }
            }
            else {
                friendSearchText.text = "";
                feedbackText.text = "User not found.";
                addFriendButton.gameObject.SetActive(false);
            }
        }));
    }

    #endregion
}
