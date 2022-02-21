using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//Семен
/// <summary>
/// Скрипт с настройками игры
/// </summary>
public class Settings : MonoBehaviour
{
    //для хранения самого себя
    static public Settings main;

    public AudioMixer audioMixer;

    [SerializeField]
    public bool DeveloperTesting = false;

    [SerializeField]
    float volumeAll = 0;
    [SerializeField]
    float volumeSound = 0;
    [SerializeField]
    float volumeMusic = 0;
    [SerializeField]
    public string launguage;

    public bool isLandscapeRotation = true;
    [SerializeField] private List<GameObject> VerticalUI;
    [SerializeField] private List<GameObject> HorizontalUI;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        SetSettings();
    }

    private void Update()
    {
        CheckScreenRotation();
    }

    private void CheckScreenRotation()
    {
        switch (Input.deviceOrientation)
        {
            case DeviceOrientation.Portrait:
                if (isLandscapeRotation)
                {
                    ScreenRotationPortrait();
                }
                break;
            case DeviceOrientation.PortraitUpsideDown:
                if (isLandscapeRotation)
                {
                    ScreenRotationPortrait();
                }
                break;
            case DeviceOrientation.LandscapeLeft:
                if (!isLandscapeRotation)
                {
                    ScreenRotationLandscape();
                }
                break;
            case DeviceOrientation.LandscapeRight:
                if (!isLandscapeRotation)
                {
                    ScreenRotationLandscape();
                }
                break;
        }
    }

    private void ScreenRotationPortrait()
    {
        isLandscapeRotation = false;
        foreach (GameObject UI in VerticalUI)
        {
            UI.SetActive(true);
        }
        foreach (GameObject UI in HorizontalUI)
        {
            UI.SetActive(false);
        }
    }
    private void ScreenRotationLandscape()
    {
        isLandscapeRotation = true;
        foreach (GameObject UI in VerticalUI)
        {
            UI.SetActive(false);
        }
        foreach (GameObject UI in HorizontalUI)
        {
            UI.SetActive(true);
        }

    }

    /// <summary>
    /// Общая громкость от 0 до 1
    /// </summary>
    public float VolumeAll
    {
        get
        {
            return main.volumeAll;
        }
        set
        {
            if (value < -80)
            {
                main.volumeAll = -80;
            }
            else if (value > 0)
            {
                main.volumeAll = 0;
            }
            else
            {
                main.volumeAll = value;
            }
            PlayerPrefs.SetFloat("volumeAll", volumeAll);
            audioMixer.SetFloat("Master", volumeAll);
        }
    }
    public float VolumeAllFrom0To1 {

        get
        {
            float volume = (80 + volumeAll) / 80;
            return volume;
        }
    }
    /// <summary>
    ///Громкость звуков от 0 до 1
    /// </summary>
    public float VolumeSound {
        get {
            return volumeSound;
        }
        set {
            if (value < -80)
            {
                main.volumeSound = -80;
            }
            else if (value > 0)
            {
                main.volumeSound = 0;
            }
            else {
                main.volumeSound = value;
            }
            PlayerPrefs.SetFloat("volumeSound", volumeSound);
            audioMixer.SetFloat("Sound", volumeSound);
        }
    }
    public float VolumeSoundFrom0To1 {
        get
        {
            float volume = (80 + volumeSound) / 80;
            return volume;
        }
    }
    /// <summary>
    ///Громкость музыки от 0 до 1
    /// </summary>
    public float VolumeMusic
    {
        get
        {
            return main.volumeMusic;
        }
        set
        {
            if (value < -80)
            {
                main.volumeMusic = -80;
            }
            else if (value > 0)
            {
                main.volumeMusic = 0;
            }
            else
            {
                main.volumeMusic = value;
            }
            PlayerPrefs.SetFloat("volumeMusic", volumeMusic);
            audioMixer.SetFloat("Music", volumeMusic);
        }
    }

    public float VolumeMusicFrom0To1
    {

        get
        {
            float volume = (80 + volumeMusic) / 80;
            return volume;
        }
    }

    public void SetLaunguage(string laung)
    {
        if(launguage != laung)
        {
            launguage = laung;
            PlayerPrefs.SetString("Launguage", launguage);
            SceneManager.LoadScene(0);
        }
    }

    bool screenIsVertical = true;

    /// <summary>
    /// Вертикальное ли положение экрана
    /// </summary>
    public bool ScreenIsVertical {
        get {
            return screenIsVertical;
        }
    }


    //загрузка всех параметров
    private void SetSettings()
    {
        volumeAll = PlayerPrefs.GetFloat("volumeAll", 0);
        VolumeAll = volumeAll;
        volumeSound = PlayerPrefs.GetFloat("volumeSound", 0);
        VolumeSound = volumeSound;
        volumeMusic = PlayerPrefs.GetFloat("volumeMusic", 0);
        VolumeMusic = volumeMusic;
        launguage = PlayerPrefs.GetString("Launguage", "English");
        TranslateManager.main.LoadFile(launguage);
    }
}
