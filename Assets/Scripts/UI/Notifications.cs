using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    #region Public Attributes

    public GameObject friendNotificationsPanel;
    public GameObject gameNotificationsPanel;

    public GameObject friendNotificationPrefab;
    public GameObject gameNotificationPrefab;

    #endregion

    #region Private Attributes

    private MenuManager _menuManager;

    #endregion

    void OnEnable() {
        _menuManager = MenuManager.singleton;

        UpdateFriendNotifications();
        UpdateGameNotifications();
    }

    public void UpdateFriendNotifications() {
        Debug.Log("Updating friend notifications");
        foreach (Transform child in friendNotificationsPanel.transform) {
            Destroy(child.gameObject);
        }
        foreach (UserData friend in _menuManager.userData.friendsInvited) {
            GameObject friendInvitationPanel = Instantiate(friendNotificationPrefab, friendNotificationsPanel.transform);
            friendInvitationPanel.GetComponent<FriendNotification>().friendData = friend;
            friendInvitationPanel.GetComponent<FriendNotification>().friendText.text = friend.username;
        }
    }

    public void UpdateGameNotifications() {
        Debug.Log("Updating game notifications");
        foreach (Transform child in gameNotificationsPanel.transform) {
            Destroy(child.gameObject);
        }
        foreach (GameData game in _menuManager.userData.gamesInvited) {
            GameObject gameInvitation = Instantiate(gameNotificationPrefab, gameNotificationsPanel.transform);
            gameInvitation.GetComponent<GameNotification>().gameIdText.text = game.sala.ToString();
        }
    }
}
