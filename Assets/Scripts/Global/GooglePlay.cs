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
    public const string KeyFileProfile = "Profile.v02";
    public const string KeyFileLVLs = "LVLs.v02";

    bool firstGetProfile = false;
    bool firstGetLVLs = false;

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
    List<SaveOrLoadData> BufferWaitingFiles = new List<SaveOrLoadData>();

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

        //Убеждаемся что подобного запроса нет в списке
        foreach (SaveOrLoadData bufferWaitingFile in BufferWaitingFiles) {
            if (bufferWaitingFile.keyFile == keyFileF //Если ключ исполняемого файла тот же
                ) {

                //Если требутся сохраниться
                if (bufferWaitingFile.isSave)
                {
                    //и этот запрос не обрабатывается
                    if (!bufferWaitingFile.inProcess) {
                        //Заменяем данные в этом запросе новыми значениями
                        bufferWaitingFile.dataSTR = dataSTRF;
                        //Не добавляем запрос потому что нашли аналогичный и заменили в нем старые значения новыми
                        return;
                    }
                }

                else {
                    //Не добавляем запрос потому что это загрузка а подобный запрос уже есть в списке
                    return;
                }

            }
        }

        //Создаем запрос
        BufferWaitingFiles.Add(new SaveOrLoadData(keyFileF, needSaveF, dataSTRF));
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
            BufferWaitingFiles.Count <= 0 || // буффер выполнения пуст
            BufferWaitingFiles[0].inProcess || //Первый в очереди уже в процессе выполнения
            Time.unscaledTime - lastTimeSaveLoad < 1 //время для проверки еще не пришло
            ) {
            return; 
        }

        //Выполняем шаг первый в очереди
        OpenSavedGame();

        lastTimeSaveLoad = Time.unscaledTime;
    }

    //Проверка первой загрузки
    //чтобы все файлы были загруженны
    void TestFirstReload() {
        if (BufferWaitingFiles.Count > 0 || !isAutorized) return;

        //Если нет в буффере очереди
        ///Проверяем была ли выполнена первая загрузка
        if (!firstGetProfile) {
            AddBufferWaitingFile(KeyFileProfile);
        }
        else if (!firstGetLVLs) {
            AddBufferWaitingFile(KeyFileLVLs);
        }
    }

    //Удалить первого в списке на сохранение или загрузку
    void DelFirstInBufferSaveOrLoad() {

        List<SaveOrLoadData> BufferWaitingFileNew = new List<SaveOrLoadData>();
        //Перебираем список начиная со второго чтобы не добавить первого
        for (int num = 0; num < BufferWaitingFiles.Count; num++) {
            if (num != 0) {
                BufferWaitingFileNew.Add(BufferWaitingFiles[num]);
            }
        }

        //Заменяем
        BufferWaitingFiles = BufferWaitingFileNew;
        //Время последнего сохранения
        lastTimeSaveLoad = Time.unscaledTime;
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
                AddBufferWaitingFile(KeyFileLVLs);
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

        TestFirstReload();

        //Debug.Log("google play autorized: " + isAutorized);
        if (Settings.main != null && Settings.main.DeveloperTesting)
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
        if (BufferWaitingFiles == null || 
            BufferWaitingFiles.Count == 0 || 
            BufferWaitingFiles[0].inProcess) {
            return;
        }

        //Говорим что в процессе выполнения
        BufferWaitingFiles[0].SetProcessTrue();

        LastMessage = BufferWaitingFiles[0].keyFile;

        LastMessage += "OpenSavedGame";
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(BufferWaitingFiles[0].keyFile, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        LastMessage = BufferWaitingFiles[0].keyFile + " ";
        if (status == SavedGameRequestStatus.Success)
        {
            if (BufferWaitingFiles[0].isSave)
            {

                //Перезаписать данные можно только если был ранее получен ответ
                bool canWrite = false;
                if (BufferWaitingFiles[0].keyFile == KeyFileProfile && firstGetProfile ||
                    BufferWaitingFiles[0].keyFile == KeyFileLVLs && firstGetLVLs)
                {
                    canWrite = true;
                }


                if (canWrite)
                {
                    byte[] dataSave = System.Text.Encoding.UTF8.GetBytes(BufferWaitingFiles[0].dataSTR);
                    SaveGame(game, dataSave);
                    LastMessage += "IsSaving";
                }
                else
                {
                    LastMessage += "not can save";
                    DelFirstInBufferSaveOrLoad();
                }
            }
            else
            {
                LoadGameData(game);
                LastMessage += " IsLoading";
            }
        }
        else
        {
            // handle error
            LastMessage = BufferWaitingFiles[0].keyFile + " error OnSavedGameOpened ";
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
                //Обрабатываем файл в зависимости от имени
                CalcLoadedData(BufferWaitingFiles[0].keyFile, dataStr);
            }
            else
            {

                LastMessage = "loading data is null";
            }
            //Говорим что ответ был получен
            //Если данные профиля
            if (BufferWaitingFiles[0].keyFile == KeyFileProfile) {
                firstGetProfile = true;
            }
            //Если данные звезд уровней
            else if (BufferWaitingFiles[0].keyFile == KeyFileLVLs) {
                firstGetLVLs = true;
            }
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

    //Обработать загруженные данные
    void CalcLoadedData(string fileName, string data) {
        if (fileName == KeyFileProfile) {
            //Отправляем на расшифровку загруженный текст
            PlayerProfile.main.LoadFromGoogle(data);
        }
        else if (fileName == KeyFileLVLs) {
            PlayerProfile.main.LoadFromGoogleLVL(data);
        }
    }
}
