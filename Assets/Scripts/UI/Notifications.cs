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

    void Start() {
        _menuManager = MenuManager.singleton;
    }

    public void UpdateFriendNotifications() {
        foreach (Transform child in this.transform) {
            Destroy(child.gameObject);
        }
        foreach (string username in _menuManager.userData.friendsInvited) {
            GameObject friendInvitationPanel = Instantiate(friendNotificationPrefab, friendNotificationsPanel.transform);
            friendInvitationPanel.GetComponent<FriendNotification>().friendText.text = username;
        }
    }

    public void UpdateGameNotifications() {
        foreach (Transform child in this.transform) {
            Destroy(child.gameObject);
        }
        foreach (GameData game in _menuManager.userData.gamesInvited) {
            GameObject gameInvitation = Instantiate(gameNotificationPrefab, gameNotificationsPanel.transform);
            gameInvitation.GetComponent<GameNotification>().gameIdText.text = game.sala.ToString();
        }
    }
}
