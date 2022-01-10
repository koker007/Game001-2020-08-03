using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using System;

public class GooglePlay : MonoBehaviour
{
    static public GooglePlay main;

    [SerializeField]
    Text TestText;
    [SerializeField]
    Text TestFps;

    public bool isAutorized = false;

    bool isSaving = false;
    public string BufferSaveSTR = "";
    private DateTime startDateTime;

    private void Awake()
    {
        main = this;
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

                //Время входа
                startDateTime = DateTime.Now;
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
        }
        
    }

    //Exit Google Play Store
    private void OnDestroy()
    {
        PlayGamesPlatform.Instance.SignOut();
    }


    public void SaveOrLoad(bool isSavingFunc) {
        isSaving = isSavingFunc;
        OpenSavedGame("Profile");
    }

    //////////////////////////////////////
    //чтение игры из гугл плей
    void OpenSavedGame(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (isSaving) {
                byte[] dataSave = System.Text.Encoding.UTF8.GetBytes(BufferSaveSTR);
                SaveGame(game, dataSave);
            }
            else {
                LoadGameData(game);
            }
        }
        else
        {
            // handle error
        }
    }


    //Сохранение игры в гугл плей
    void SaveGame(ISavedGameMetadata game, byte[] savedData)
    {
        TimeSpan CurrentSpan = DateTime.Now - startDateTime;
        TimeSpan TotalPlaySpan = game.TotalTimePlayed + CurrentSpan;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(TotalPlaySpan)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);

        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            Debug.Log("Успешно сохранено в google play");

        }
        else
        {
            // handle error
        }
    }

    //Загрузка игры
    void LoadGameData(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }

    public void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data
            if (data.Length > 0)
            {
                string dataStr = System.Text.Encoding.ASCII.GetString(data);
                //Успешно загруженно

                //Отправляем на расшифровку загруженный текст
                PlayerProfile.main.LoadFromGoogle(dataStr);
            }
            else Debug.LogError("loading data is null");
        }
        else
        {
            // handle error
        }
    }
}
