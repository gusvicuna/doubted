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

    private const string rootPath = "https://pdsuandes.herokuapp.com/";

    #endregion


    #region MonoBehaviour Callbacks
    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null) singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        using (UnityWebRequest request = UnityWebRequest.Get(rootPath + "/friends" + "?current_id=" + id)) {
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


    //GetPlayerGameInvitations(id) => List<game_id>

    //GetPlayerFriendInvitations(id)  => List<username>


    //InviteFriendToGame(sala, target_id)

    //AcceptInviteFriendToGame(id, sala)

    //DeleteInviteFriend(id, sala)


    //PostAddFriend(id, target_id)

    //UpdateAddFriend(id, target_id, status)

    //DeleteAddFriend(id, target_id)

    #endregion
}
