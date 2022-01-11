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
    

    bool dataIsLoadedSuccess = false; //Была ли уже попытка считать данные и ответ был получен
    float lastTimeSaveLoad = 0;
    private DateTime startDateTime;
    string LastMessage = "None";

    //Имена файлов которые участвуют в загрузке и сохранении на гуглплей
    public const string KeyFileProfile = "Profile";
    public const string KeyFileLVLStars = "LVLStars";

    class SaveOrLoadData {
        public string keyFile; //имя файла который будет загружаться
        public bool isSave; //Этот файл нужно сохранить? если нет то загрузка из гугла
        public string dataSTR; //Данные файла

        public bool inProcess; //Находится ли этот шаг в процессе выполнения

        public SaveOrLoadData(string keyFileF, bool isSavingF, string dataSTRF) {
            keyFile = keyFileF;
            isSave = isSavingF;
            dataSTR = dataSTRF;

            inProcess = false;
        }

        public void SetProcessTrue() {
            inProcess = true;
        }
    }

    //Список запросов на запись или чтение
    List<SaveOrLoadData> BufferWaitingFile = new List<SaveOrLoadData>();

    /// <summary>
    /// Добавить файл в очередь на загрузку или сохранение
    /// </summary>
    /// <param name="keyFileF"></param>
    /// <param name="needSaveF"></param>
    /// <param name="dataSTRF"></param>
    void AddBufferWaitingFile(string keyFileF, bool needSaveF, string dataSTRF) {
        //выходим, если нет информации или нет имени файла 
        if (keyFileF.Length == 0 || keyFileF == "" || 
            (needSaveF && (dataSTRF == "" || dataSTRF.Length == 0))) {
            return;
        }

        //Создаем запрос
        BufferWaitingFile.Add(new SaveOrLoadData(keyFileF, needSaveF, dataSTRF));
    }

    /// <summary>
    /// Добавить в очерерь на загрузку из гугл плей
    /// </summary>
    /// <param name="KeyFile"></param>
    public void AddBufferWaitingFile(string keyFileF) {
        AddBufferWaitingFile(keyFileF, false, "");
    }

    /// <summary>
    /// Сохранить информацию в файл
    /// </summary>
    /// <param name="keyFileF"></param>
    /// <param name="dataSTRF"></param>
    public void AddBufferWaitingFile(string keyFileF, string dataSTRF) {
        AddBufferWaitingFile(keyFileF, true, dataSTRF);
    }

    //Выполнить шаг загрузки или сохранения файла из очереди
    void StepBufferSaveOrLoad() {
        //Выходим если
        if (!isAutorized || //Не выполнена авторизация в гугл плее
            BufferWaitingFile.Count <= 0 || // буффер выполнения пуст
            BufferWaitingFile[0].inProcess || //Первый в очереди уже в процессе выполнения
            Time.unscaledTime - lastTimeSaveLoad < 5 //время для проверки еще не пришло
            ) {
            return; 
        }

        //Выполняем шаг первый в очереди
        OpenSavedGame();

        lastTimeSaveLoad = Time.unscaledTime;
    }

    //Удалить первого в списке на сохранение или загрузку
    void DelFirstInBufferSaveOrLoad() {

        List<SaveOrLoadData> BufferWaitingFileNew = new List<SaveOrLoadData>();
        //Перебираем список начиная со второго чтобы не добавить первого
        for (int num = 0; num < BufferWaitingFile.Count; num++) {
            if (num > 0) {
                BufferWaitingFileNew.Add(BufferWaitingFile[num]);
            }
        }

        //Заменяем
        BufferWaitingFile = BufferWaitingFileNew;
    }


    private void Awake()
    {
        main = this;
        iniServices();
    }

    void iniServices() {
        LastMessage = "1";
        //Создаем конфигурацию с указанием какие ништяки Google play нужно использовать
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                // enables saving game progress.
            .EnableSavedGames()
                // requests the email address of the player be available.
                // Will bring up a prompt for consent.
            //.RequestEmail()
                // requests a server auth code be generated so it can be passed to an
                //  associated back end server application and exchanged for an OAuth token.
            //.RequestServerAuthCode(false)
                // requests an ID token be generated.  This OAuth token can be used to
                //  identify the player to other services such as Firebase.
            //.RequestIdToken()
            .Build();

        LastMessage = "2";
        //Применяем конфигурацию
        PlayGamesPlatform.InitializeInstance(config);
        LastMessage = "3";

        PlayGamesPlatform.DebugLogEnabled = true;
        LastMessage = "4";
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

        LastMessage = "AutorizationSocial";
        Social.localUser.Authenticate(success => {
            Debug.Log("===============================================");
            if (success)
            {
                Debug.Log("Google Play Services: Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);

                LastMessage = "Google Play Services: Authentication ok";

                //Время входа
                startDateTime = DateTime.Now;

                //Добавляем на загрузку данные
                AddBufferWaitingFile(KeyFileProfile);
            }
            else {
                Debug.Log("Google Play Services: Authentication failed");
                LastMessage = "Google Play Services: Authentication failed";
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

        StepBufferSaveOrLoad();

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

        TestText.text = LastMessage;
    }

    //Exit Google Play Store
    private void OnDestroy()
    {
        PlayGamesPlatform.Instance.SignOut();
    }


    //////////////////////////////////////
    //чтение игры из гугл плей //По файлу первому в очереди
    void OpenSavedGame()
    {
        //Если первый файл уже в процессе выполнения
        if (BufferWaitingFile[0].inProcess) {
            return;
        }

        //Говорим что в процессе выполнения
        BufferWaitingFile[0].SetProcessTrue();

        LastMessage = "OpenSavedGame";
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        LastMessage = "savedGameClient";
        savedGameClient.OpenWithAutomaticConflictResolution(BufferWaitingFile[0].keyFile, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (BufferWaitingFile[0].isSave)
            {

                //Перезаписать данные можно только если был ранее получен ответ
                if (dataIsLoadedSuccess)
                {
                    byte[] dataSave = System.Text.Encoding.UTF8.GetBytes(BufferWaitingFile[0].dataSTR);
                    SaveGame(game, dataSave);
                    LastMessage = "OnSavedGameOpened IsSaving";
                }
                else
                {
                    LastMessage = "OnSavedGameOpened IsSaving not Success";
                    DelFirstInBufferSaveOrLoad();
                }
            }
            else
            {
                LoadGameData(game);
                LastMessage = "OnSavedGameOpened IsLoading";
            }
        }
        else
        {
            // handle error
            LastMessage = "error OnSavedGameOpened ";
            if (status == SavedGameRequestStatus.AuthenticationError)
            {
                LastMessage += "AuthenticationError";
            }
            else if(status == SavedGameRequestStatus.BadInputError) {
                LastMessage += "BadInputError";
            }
            else if (status == SavedGameRequestStatus.InternalError)
            {
                LastMessage += "InternalError";
            }
            else if (status == SavedGameRequestStatus.TimeoutError)
            {
                LastMessage += "TimeoutError";
            }

            //Какая-то ошибка удаляем из очереди
            DelFirstInBufferSaveOrLoad();
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
            LastMessage ="Успешно сохранено в google play";

        }
        else
        {
            // handle error
            LastMessage = "OnSavedGameWritten bad ";
            if (status == SavedGameRequestStatus.AuthenticationError)
            {
                LastMessage += "AuthenticationError";
            }
            else if (status == SavedGameRequestStatus.BadInputError)
            {
                LastMessage += "BadInputError";
            }
            else if (status == SavedGameRequestStatus.InternalError)
            {
                LastMessage += "InternalError";
            }
            else if (status == SavedGameRequestStatus.TimeoutError)
            {
                LastMessage += "TimeoutError";
            }

        }

        //Процесс сохранения выполнен удаляем из очереди
        DelFirstInBufferSaveOrLoad();
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
            else
            {
                
                LastMessage = "loading data is null";
            }
            //Говорим что ответ был получен
            dataIsLoadedSuccess = true;
        }
        else
        {
            // handle error
            LastMessage = "OnSavedGameDataRead ";
            if (status == SavedGameRequestStatus.AuthenticationError)
            {
                LastMessage += "AuthenticationError";
            }
            else if (status == SavedGameRequestStatus.BadInputError)
            {
                LastMessage += "BadInputError";
            }
            else if (status == SavedGameRequestStatus.InternalError)
            {
                LastMessage += "InternalError";
            }
            else if (status == SavedGameRequestStatus.TimeoutError)
            {
                LastMessage += "TimeoutError";
            }
        }

        //Процесс загрузки был выполнен удаляем из очереди
        DelFirstInBufferSaveOrLoad();
    }
}
