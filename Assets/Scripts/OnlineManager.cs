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

    private const string RootPath = "https://gentle-brook-46195.herokuapp.com/";
    private const string LocalHostPath = "http://localhost:38619/api";

    #endregion


    #region MonoBehaviour Callbacks

    void Start()
    {
        if (singleton == null) singleton = this;
    }

    #endregion


    #region Public Methods

    //GetPlayer(username)
    public IEnumerator GetPlayer(string username, System.Action<JSONNode> callback = null)
    {
        using UnityWebRequest request = UnityWebRequest.Get(LocalHostPath + "/users" + "/" + username);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log(request.error);
            callback?.Invoke(null);
        }
        else
        {
            callback?.Invoke(JSON.Parse(request.downloadHandler.text));
        }
    }

    //PostPlayer(username)
    public IEnumerator SignUp(string profile, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(LocalHostPath + "/users", "POST")) {
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError(request.error);
                if (callback != null) {
                    callback.Invoke(false);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(request.downloadHandler.text != "{}");
                }
            }
        }
    }


    //GetPlayerGames(id)
    public IEnumerator GetPlayerGames(string id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(RootPath + "/games" + "?my_id=" + id)) {
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
    public IEnumerator GetPlayerFriends(string id, System.Action<JSONNode> callback = null)
    {
        using UnityWebRequest request = UnityWebRequest.Get(LocalHostPath + "/users/" + id + "/friends");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log(request.error);
            callback?.Invoke(null);
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


    //PostCreateGame(id,players,sala)
    public IEnumerator NewGame(string id, string players, string sala, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(RootPath + "/new_game" + "?current_id=" + id + "&players=" + players + "&sala=" + sala)) {
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
        using (UnityWebRequest request = UnityWebRequest.Get(RootPath + "/join" + "?sala=" + sala + "&current_id=" + id)) {
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
        using (UnityWebRequest request = UnityWebRequest.Get(RootPath + "/get_game_invitations" + "?current_id=" + id)) {
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

    //GetPlayerFriendInvitations(id)
    public IEnumerator GetFriendNotifications(string id, System.Action<JSONNode> callback = null)
    {
        using UnityWebRequest request = UnityWebRequest.Get(LocalHostPath + "/users/" + id + "/friendNotifications");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log(request.error);
            callback?.Invoke(null);
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


    //InviteToGame(sala, target_id)

    //AcceptInviteToGame(id, sala)
    public IEnumerator AcceptGame(string id, string sala, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(RootPath + "/accept_game" + "?current_id=" + id + "?sala=" + sala)) {
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
        using (UnityWebRequest request = UnityWebRequest.Get(RootPath + "/decline_game" + "?current_id=" + id + "?sala=" + sala)) {
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
    public IEnumerator RequestFriend(string id, string aTargetUser, System.Action<JSONNode> callback = null)
    {
        Debug.Log("Adding friend: " + aTargetUser);
        using UnityWebRequest request = new UnityWebRequest(LocalHostPath + "/users/" + id + "/friends", "POST");
        request.SetRequestHeader("Content-Type", "application/json");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(aTargetUser);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.LogError(request.error);
            callback?.Invoke(false);
        }
        else
        {
            callback?.Invoke(request.downloadHandler.text != "{}");
        }
    }

    //UpdateAddFriend(id, target_id, status)
    public IEnumerator AcceptFriend(string id, string target_id, System.Action<JSONNode> callback = null) {
        Debug.Log(id + "=>" + target_id);
        using UnityWebRequest request = new UnityWebRequest(LocalHostPath + "/users/" + id + "/friends/" + target_id, "PUT");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.LogError(request.error);
            callback?.Invoke(false);
        }
        else {
            callback?.Invoke(request.downloadHandler.text != "{}");
        }
    }

    //DeleteAddFriend(id, target_id)
    public IEnumerator DeleteFriendship(string id, string target_id, System.Action<JSONNode> callback = null)
    {
        using UnityWebRequest request = new UnityWebRequest(LocalHostPath + "/users/" + id + "/friends/" + target_id,"DELETE");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log(request.error);
            callback?.Invoke(null);
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

    #endregion
}
