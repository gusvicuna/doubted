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

    //GetUser(username)
    public IEnumerator GetUser(string username, System.Action<JSONNode> callback = null)
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


    //GetUserGames(id)
    public IEnumerator GetUserGames(string id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(LocalHostPath + "/users/" + id + "/players")) {
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

    //GetUserFriends(username) = List<username>
    public IEnumerator GetUserFriends(string id, System.Action<JSONNode> callback = null)
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


    //PostCreateGame(id,players,id)
    public IEnumerator NewGame(string profile, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(LocalHostPath + "/Games", "POST")) {
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
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
            }
        }
    }

    //PostJoinGame()
    public IEnumerator JoinGame(string id, string sala, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = UnityWebRequest.Get(RootPath + "/join" + "?id=" + sala + "&current_id=" + id)) {
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
        using (UnityWebRequest request = UnityWebRequest.Get(LocalHostPath + "/users/" + id + "/gameNotifications")) {
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


    //InviteToGame(id, target_id)

    //AcceptInviteToGame(id, id)
    public IEnumerator AcceptGame(string id,string profile, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(LocalHostPath + "/players/" + id, "PUT")) {
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
                    callback.Invoke(JSON.Parse(request.downloadHandler.text));
                }
            }
        }
    }

    //DeleteInviteToGame(id, id)
    public IEnumerator DeclineGame(string id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(LocalHostPath + "/players/" + id,"DELETE")) {
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

    public IEnumerator NewPlayer(string profile, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(LocalHostPath + "/players", "POST")) {
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

    public IEnumerator GetPlayers(string game_id, System.Action<JSONNode> callback = null) {
        using UnityWebRequest request = UnityWebRequest.Get(LocalHostPath + "/games/" + game_id + "/players");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log(request.error);
            callback?.Invoke(null);
        }
        else {
            callback?.Invoke(JSON.Parse(request.downloadHandler.text));
        }
    }

    public IEnumerator GetGame(string game_id, System.Action<JSONNode> callback = null) {
        Debug.Log(game_id);
        using UnityWebRequest request = UnityWebRequest.Get(LocalHostPath + "/games/" + game_id);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log(request.error);
            callback?.Invoke(null);
        }
        else {
            callback?.Invoke(JSON.Parse(request.downloadHandler.text));
        }
    }

    #endregion
}
