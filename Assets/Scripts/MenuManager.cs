using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public UserData userData;
    public static MenuManager singleton;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null) singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
