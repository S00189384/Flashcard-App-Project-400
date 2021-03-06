using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Note: didn't add any json data to user list since I wont be doing that until I'm gonna set up logging out etc.

public class UserDataHolder : MonoBehaviour
{
    public static UserDataHolder Instance;

    [SerializeField] private User currentUser;
    public User CurrentUser { get => currentUser; set { currentUser = value; } }

    //[SerializeField] private List<User> userList;

    [Header("Storing JSON Location - Unity Editor & PC")]
    [SerializeField] private string userListJSONPathPC;
    [SerializeField] private string currentUserJSONPathPC;
    [Header("Storing JSON Location - Mobile")]
    [SerializeField] private string userListJSONPathMobile;
    [SerializeField] private string currentUserJSONPathMobile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        BtnLogout.UserLoggedOutEvent += OnUserLoggedOut;
    }

    private void OnUserLoggedOut() => currentUser = null;
    public void SetCurrentUser(User user) => currentUser = user;

    //Json.
    private void SaveCurrentUserToJson()
    {
        string userAsJSON = JsonUtility.ToJson(currentUser);

        string filePath = Application.dataPath + currentUserJSONPathPC;

        File.WriteAllText(filePath, userAsJSON);
    }
    private void SetCurrentUserFromJSON()
    {
        string json;

#if UNITY_EDITOR
        json = File.ReadAllText(Application.dataPath + currentUserJSONPathPC);

#elif UNITY_ANDROID
        json = File.ReadAllText(Application.persistentDataPath + currentUserJSONPathMobile);

#endif
        currentUser = JsonUtility.FromJson<User>(json);
    }

    private void OnDestroy()
    {

        BtnLogout.UserLoggedOutEvent -= OnUserLoggedOut;
    }
}
