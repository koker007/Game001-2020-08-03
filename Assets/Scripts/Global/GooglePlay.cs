using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GooglePlay : MonoBehaviour
{
    [SerializeField]
    Text TestText;
    [SerializeField]
    Text TestFps;
    [SerializeField]
    int fps;

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

        //Если авторизация не прошла успешно, принимаем новую попытку позже
        if (!isAutorized)
        {
            Invoke("iniServices", 60f);
        }
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
    void AutorizationPlayGamesPlatform() {
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

    int[] fpsBuffer = new int[10];
    void Update()
    {
        //Debug.Log("google play autorized: " + isAutorized);
        if (Settings.main.DeveloperTesting)
        {
            TestText.text = "GPS:" + isAutorized;
            int fpsNew = (int)(1 / (float)Time.unscaledDeltaTime);

            int fpsSum = 0;
            for (int num = 0; num < fpsBuffer.Length; num++) {
                if (num != fpsBuffer.Length - 1)
                    fpsBuffer[num] = fpsBuffer[num + 1];
                else fpsBuffer[num] = fpsNew;

                fpsSum += fpsBuffer[num];
            }

            fpsNew = fpsSum / fpsBuffer.Length;

            TestFps.text = fpsNew.ToString();

            fps = fpsNew;
        }
        
    }

    //Exit Google Play Store
    private void OnDestroy()
    {
        PlayGamesPlatform.Instance.SignOut();

    }

}
