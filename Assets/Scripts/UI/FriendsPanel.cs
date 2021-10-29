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
        foreach (UserData friend in _menuManager.userData.friends) {
            GameObject friendPanel = Instantiate(friendPanelPrefab, friendsList.transform);
            friendPanel.GetComponentInChildren<Text>().text = friend.username;
        }
    }

    public void AddFriend() {
        _menuManager.RequestFriend(userSearched);
    }

    public void SearchFriend() {
        StartCoroutine(onlineManager.GetPlayerId(usernameInputField.text, result => {
            if (result != null) {
                if(friendSearchText.text == _menuManager.userData.username) {
                    feedbackText.text = "You are that user.";
                }
                else {
                    userSearched = new UserData();
                    feedbackText.text = "";
                    userSearched.id = result["user_id"];
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
