using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlay : MonoBehaviour
{
    bool isAutorized = false;

    private void Awake()
    {
        iniServices();
    }

    void iniServices() {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        //AutorizationPlayGamesPatform();
        AutorizationSocial();
    }
    void AutorizationSocial() {
        Social.localUser.Authenticate(success => {
            Debug.Log("===============================================");
            if (success)
            {
                Debug.Log("Google Play Services: Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);
            }
            else {
                Debug.Log("Google Play Services: Authentication failed");
            }

            Debug.Log("===============================================");

            isAutorized = success;
        });

        
    }
    void AutorizationPlayGamesPatform() {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) => {
            Debug.Log("===============================================");
            if (result == SignInStatus.Success)
            {
                Debug.Log("Google Play Services: Authentication successful");
            }
            else {
                Debug.Log("Google Play Services: Authentication failed");
            }

            Debug.Log("===============================================");

        });
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("google play autorized: " + isAutorized);
    }

    //Exit Google Play Store
    private void OnDestroy()
    {
        PlayGamesPlatform.Instance.SignOut();
        //PlayGamesPlatform.Instance.
    }

}
