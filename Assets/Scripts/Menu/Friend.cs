using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    public UserData friendData;

    //Delegate
    [HideInInspector]
    public delegate void OnFriendRemovedDelegate(UserData userData);
    [HideInInspector]
    public OnFriendRemovedDelegate myDelegate;

    public void RemoveFriend()
    {
        myDelegate(friendData);
        Destroy(gameObject);
    }
}
