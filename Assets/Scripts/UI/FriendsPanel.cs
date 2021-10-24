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

    public GameObject friendPanelPrefab;
    public GameObject friendsList;

    private MenuManager menuManager;
    private OnlineManager onlineManager;
    #endregion

    private void Start() {
        menuManager = MenuManager.singleton;
        onlineManager = OnlineManager.singleton;

        feedbackText.text = "";
        friendSearchText.text = "";
        addFriendButton.gameObject.SetActive(false);
    }

    public void ShowFriend(string friendUsername) {
        GameObject friendPanel = Instantiate(friendPanelPrefab, friendsList.transform);
        friendPanel.GetComponentInChildren<Text>().text = friendUsername; 
    }

    public void AddFriend() {
        menuManager.RequestFriend(usernameInputField.text);
    }

    public void SearchFriend() {
        StartCoroutine(onlineManager.GetPlayerId(usernameInputField.text, result => {
            if (result != null) {
                feedbackText.text = "";
                friendSearchText.text = usernameInputField.text;
                addFriendButton.gameObject.SetActive(true);
            }
            else {
                friendSearchText.text = "";
                feedbackText.text = "User not found.";
                addFriendButton.gameObject.SetActive(false);
            }
        }));
    }
}
