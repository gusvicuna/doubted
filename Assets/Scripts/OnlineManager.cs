using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class OnlineManager : MonoBehaviour
{
    #region Public Attributes

    //Singleton
    public static OnlineManager singleton;

    #endregion

    [SerializeField]
    public bool local = true;
    private const string DeployPath = "https://doubtedapi20211029141217.azurewebsites.net/api";
    private const string LocalPath = "http://localhost:38619/api";
    private string _apiPath;


    #region MonoBehaviour Callbacks

    void Start()
    {
        //Singleton
        if (singleton == null) singleton = this;

        _apiPath = local ? LocalPath : DeployPath;
    }

    #endregion


    #region Public Methods

    //GetUser(username)
    public IEnumerator GetUser(string username, System.Action<JSONNode> callback = null)
    {
        using UnityWebRequest request = UnityWebRequest.Get(_apiPath + "/users/" + username);
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
        using (UnityWebRequest request = new UnityWebRequest(_apiPath + "/users", "POST")) {
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
        using (UnityWebRequest request = UnityWebRequest.Get(_apiPath + "/users/" + id + "/players")) {
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
        using UnityWebRequest request = UnityWebRequest.Get(_apiPath + "/users/" + id + "/friends");
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
        using (UnityWebRequest request = new UnityWebRequest(_apiPath + "/Games", "POST")) {
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
        using (UnityWebRequest request = UnityWebRequest.Get(_apiPath + "/join" + "?id=" + sala + "&current_id=" + id)) {
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
        using (UnityWebRequest request = UnityWebRequest.Get(_apiPath + "/users/" + id + "/gameNotifications")) {
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
        using UnityWebRequest request = UnityWebRequest.Get(_apiPath + "/users/" + id + "/friendNotifications");
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

    //DeleteInviteToGame(id, id)
    public IEnumerator DeclineGame(string id, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(_apiPath + "/players/" + id,"DELETE")) {
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
        using UnityWebRequest request = new UnityWebRequest(_apiPath + "/users/" + id + "/friends", "POST");
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
        using UnityWebRequest request = new UnityWebRequest(_apiPath + "/users/" + id + "/friends/" + target_id, "PUT");
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
        using UnityWebRequest request = new UnityWebRequest(_apiPath + "/users/" + id + "/friends/" + target_id,"DELETE");
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
        using (UnityWebRequest request = new UnityWebRequest(_apiPath + "/players", "POST")) {
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
                    callback.Invoke(JSONNode.Parse(request.downloadHandler.text));
                }
            }
        }
    }

    public IEnumerator GetPlayers(string game_id, System.Action<JSONNode> callback = null) {
        using UnityWebRequest request = UnityWebRequest.Get(_apiPath + "/games/" + game_id + "/players");
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
        using UnityWebRequest request = UnityWebRequest.Get(_apiPath + "/games/" + game_id);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log(request.error);
            callback?.Invoke(null);
        }
        else {
            callback?.Invoke(JSON.Parse(request.downloadHandler.text));
        }
    }

    public IEnumerator UploadGame(string id, string profile, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(_apiPath + "/games/" + id, "PUT")) {
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

    public IEnumerator PutUser(string id, string profile, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(_apiPath + "/users/" + id, "PUT")) {
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

    public IEnumerator GetCurrentPlayer(string id, string game_id, System.Action<JSONNode> callback = null) {
        Debug.Log(game_id);
        using UnityWebRequest request = UnityWebRequest.Get(_apiPath + "/users/" + id + "/" + game_id + "/players");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError) {
            Debug.Log(request.error);
            callback?.Invoke(null);
        }
        else {
            callback?.Invoke(JSON.Parse(request.downloadHandler.text));
        }
    }

    public IEnumerator PutGame(string id, string profile, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(_apiPath + "/games/" + id, "PUT")) {
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

    public IEnumerator PutPlayer(string id, string profile, System.Action<JSONNode> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(_apiPath + "/players/" + id, "PUT")) {
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


    #endregion
}
