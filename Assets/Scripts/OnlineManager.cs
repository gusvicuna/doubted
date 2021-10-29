using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class OnlineManager : MonoBehaviour
{
    #region Public Attributes

    public static OnlineManager singleton;

    #endregion


    #region Routes

    private const string rootPath = "https://gentle-brook-46195.herokuapp.com/";

    #endregion


    #region MonoBehaviour Callbacks

    void Start()
    {
        if (singleton == null) singleton = this;
    }

    #endregion


    #region Public Methods

    //GetPlayer(username)
    public IEnumerator GetPlayerId(string username, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/login" + "?username=" + username)) {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
            }
        }
    }

    //PostPlayer(username)
    public IEnumerator Signup(string username, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/new_user" + "?username=" + username)) {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
            }
        }
    }


    //GetPlayerGames(id)
    public IEnumerator GetPlayerGames(string id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/games" + "?my_id=" + id)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }

    //GetPlayerFriends(username) = List<username>
    public IEnumerator GetPlayerFriends(string id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/get_friends" + "?current_id=" + id)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }


    //PostCreateGame(id,players,sala)
    public IEnumerator NewGame(string id, string players, string sala, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/new_game" + "?current_id=" + id + "&players=" + players + "&sala=" + sala)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }

    //PostJoinGame()
    public IEnumerator JoinGame(string id, string sala, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/join" + "?sala=" + sala + "&current_id=" + id)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }


    //GetGameInvitations(id) => List<game_id>
    public IEnumerator GetGameNotifications(string id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/get_game_invitations" + "?current_id=" + id)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }

    //GetPlayerFriendInvitations(id)  => List<username>
    public IEnumerator GetFriendNotifications(string id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/get_friends_not_accepted" + "?current_id=" + id)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }


    //InviteToGame(sala, target_id)

    //AcceptInviteToGame(id, sala)
    public IEnumerator AcceptGame(string id, string sala, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/accept_game" + "?current_id=" + id + "?sala=" + sala)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }

    //DeleteInviteToGame(id, sala)
    public IEnumerator DeclineGame(string id, string sala, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/decline_game" + "?current_id=" + id + "?sala=" + sala)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }

    //PostAddFriend(id, target_id)
    public IEnumerator RequestFriend(string id, string target_id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/add_friend" + "?current_id=" + id + "?target_id=" + target_id)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.LogError("error");
                }
            }
        }
    }

    //UpdateAddFriend(id, target_id, status)
    public IEnumerator AcceptFriend(string id, string target_id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/accept_friend_request" + "?current_id=" + id + "?target_id=" + target_id)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }

    //DeleteAddFriend(id, target_id)
    public IEnumerator DeclineFriend(string id, string target_id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/decline_friend_request" + "?current_id=" + id + "?target_id=" + target_id)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
                if (callback != null) {
                    callback.Invoke(null);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
                else {
                    Debug.Log("error");
                }
            }
        }
    }

    #endregion
}
