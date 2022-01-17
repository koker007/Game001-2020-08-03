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
    

    bool dataIsLoadedSuccess = false; //���� �� ��� ������� ������� ������ � ����� ��� �������
    float lastTimeSaveLoad = 0;
    private DateTime startDateTime;
    string LastMessage = "None";

    //����� ������ ������� ��������� � �������� � ���������� �� ��������
    public const string KeyFileProfile = "Profile.v02";
    public const string KeyFileLVLs = "LVLs.v02";

    bool firstGetProfile = false;
    bool firstGetLVLs = false;

    class SaveOrLoadData {
        public string keyFile; //��� ����� ������� ����� �����������
        public bool isSave; //���� ���� ����� ���������? ���� ��� �� �������� �� �����
        public string dataSTR; //������ �����

        public bool inProcess; //��������� �� ���� ��� � �������� ����������

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

    //������ �������� �� ������ ��� ������
    List<SaveOrLoadData> BufferWaitingFiles = new List<SaveOrLoadData>();

    /// <summary>
    /// �������� ���� � ������� �� �������� ��� ����������
    /// </summary>
    /// <param name="keyFileF"></param>
    /// <param name="needSaveF"></param>
    /// <param name="dataSTRF"></param>
    void AddBufferWaitingFile(string keyFileF, bool needSaveF, string dataSTRF) {
        //�������, ���� ��� ���������� ��� ��� ����� ����� 
        if (keyFileF.Length == 0 || keyFileF == "" ||
            (needSaveF && (dataSTRF == "" || dataSTRF.Length == 0))) {
            return;
        }

        //���������� ��� ��������� ������� ��� � ������
        foreach (SaveOrLoadData bufferWaitingFile in BufferWaitingFiles) {
            if (bufferWaitingFile.keyFile == keyFileF //���� ���� ������������ ����� ��� ��
                ) {

                //���� �������� �����������
                if (bufferWaitingFile.isSave)
                {
                    //� ���� ������ �� ��������������
                    if (!bufferWaitingFile.inProcess) {
                        //�������� ������ � ���� ������� ������ ����������
                        bufferWaitingFile.dataSTR = dataSTRF;
                        //�� ��������� ������ ������ ��� ����� ����������� � �������� � ��� ������ �������� ������
                        return;
                    }
                }

                else {
                    //�� ��������� ������ ������ ��� ��� �������� � �������� ������ ��� ���� � ������
                    return;
                }

            }
        }

        //������� ������
        BufferWaitingFiles.Add(new SaveOrLoadData(keyFileF, needSaveF, dataSTRF));
    }

    /// <summary>
    /// �������� � ������� �� �������� �� ���� ����
    /// </summary>
    /// <param name="KeyFile"></param>
    public void AddBufferWaitingFile(string keyFileF) {
        AddBufferWaitingFile(keyFileF, false, "");
    }

    /// <summary>
    /// ��������� ���������� � ����
    /// </summary>
    /// <param name="keyFileF"></param>
    /// <param name="dataSTRF"></param>
    public void AddBufferWaitingFile(string keyFileF, string dataSTRF) {
        AddBufferWaitingFile(keyFileF, true, dataSTRF);
    }

    //��������� ��� �������� ��� ���������� ����� �� �������
    void StepBufferSaveOrLoad() {
        //������� ����
        if (!isAutorized || //�� ��������� ����������� � ���� ����
            BufferWaitingFiles.Count <= 0 || // ������ ���������� ����
            BufferWaitingFiles[0].inProcess || //������ � ������� ��� � �������� ����������
            Time.unscaledTime - lastTimeSaveLoad < 1 //����� ��� �������� ��� �� ������
            ) {
            return; 
        }

        //��������� ��� ������ � �������
        OpenSavedGame();

        lastTimeSaveLoad = Time.unscaledTime;
    }

    //�������� ������ ��������
    //����� ��� ����� ���� ����������
    void TestFirstReload() {
        if (BufferWaitingFiles.Count > 0 || !isAutorized) return;

        //���� ��� � ������� �������
        ///��������� ���� �� ��������� ������ ��������
        if (!firstGetProfile) {
            AddBufferWaitingFile(KeyFileProfile);
        }
        else if (!firstGetLVLs) {
            AddBufferWaitingFile(KeyFileLVLs);
        }
    }

    //������� ������� � ������ �� ���������� ��� ��������
    void DelFirstInBufferSaveOrLoad() {

        List<SaveOrLoadData> BufferWaitingFileNew = new List<SaveOrLoadData>();
        //���������� ������ ������� �� ������� ����� �� �������� �������
        for (int num = 0; num < BufferWaitingFiles.Count; num++) {
            if (num != 0) {
                BufferWaitingFileNew.Add(BufferWaitingFiles[num]);
            }
        }

        //��������
        BufferWaitingFiles = BufferWaitingFileNew;
        //����� ���������� ����������
        lastTimeSaveLoad = Time.unscaledTime;
    }


    private void Awake()
    {
        main = this;
        iniServices();
    }

    void iniServices() {
        LastMessage = "1";
        //������� ������������ � ��������� ����� ������� Google play ����� ������������
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
        //��������� ������������
        PlayGamesPlatform.InitializeInstance(config);
        LastMessage = "3";

        PlayGamesPlatform.DebugLogEnabled = true;
        LastMessage = "4";
        PlayGamesPlatform.Activate();
        //AutorizationPlayGamesPatform();
        AutorizationSocial();

        //���� ����������� �� ������ �������, ��������� ����� ������� �����
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

                //����� �����
                startDateTime = DateTime.Now;

                //��������� �� �������� ������
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
    //������ ���� �� ���� ���� //�� ����� ������� � �������
    void OpenSavedGame()
    {
        //���� ������ ���� ��� � �������� ����������
        if (BufferWaitingFiles == null || 
            BufferWaitingFiles.Count == 0 || 
            BufferWaitingFiles[0].inProcess) {
            return;
        }

        //������� ��� � �������� ����������
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

                //������������ ������ ����� ������ ���� ��� ����� ������� �����
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

            //�����-�� ������ ������� �� �������
            DelFirstInBufferSaveOrLoad();
        }
    }


    //���������� ���� � ���� ����
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
            LastMessage ="������� ��������� � google play";

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

        //������� ���������� �������� ������� �� �������
        DelFirstInBufferSaveOrLoad();
    }

    //�������� ����
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

                //������� ����������
                //������������ ���� � ����������� �� �����
                CalcLoadedData(BufferWaitingFiles[0].keyFile, dataStr);
            }
            else
            {

                LastMessage = "loading data is null";
            }
            //������� ��� ����� ��� �������
            //���� ������ �������
            if (BufferWaitingFiles[0].keyFile == KeyFileProfile) {
                firstGetProfile = true;
            }
            //���� ������ ����� �������
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

        //������� �������� ��� �������� ������� �� �������
        DelFirstInBufferSaveOrLoad();
    }

    //���������� ����������� ������
    void CalcLoadedData(string fileName, string data) {
        if (fileName == KeyFileProfile) {
            //���������� �� ����������� ����������� �����
            PlayerProfile.main.LoadFromGoogle(data);
        }
        else if (fileName == KeyFileLVLs) {
            PlayerProfile.main.LoadFromGoogleLVL(data);
        }
    }
}
