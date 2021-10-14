using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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
    public IEnumerator GetPlayer(string username, System.Action<UserData> callback = null) {
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
                    callback.Invoke(UserData.Parse(request.downloadHandler.text));
                }
            }
        }
    }

    //PostPlayer(username)
    public IEnumerator PostPlayer(string profile, System.Action<bool> callback = null) {
        using (UnityWebRequest request = new UnityWebRequest(rootPath, UnityWebRequest.kHttpVerbPOST)) {
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.Log(request.error);
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

    //GetPlayerGameInvitations(username)

    //GetPlayerFriendInvitations(username)

    //GetPlayerFriends(username)


    //PostCreateGame(id,players,sala)


    //PostInviteFriend(sala,target_id)

    //UpdateInviteFriend(sala,id)

    //DeleteInviteFriend(sala,id)


    //PostAddFriend(id,target_id)

    //UpdateAddFriend(id,target_id,status)

    //DeleteAddFriend(id,target_id)

    #endregion
}
