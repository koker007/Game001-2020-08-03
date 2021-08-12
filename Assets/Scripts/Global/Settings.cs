using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    float volumeAll = 0;
    [SerializeField]
    float volumeSound = 0;
    [SerializeField]
    float volumeMusic = 0;


    // Start is called before the first frame update
    void Start()
    {
        main = this;
        SetSettings();
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
        volumeSound = PlayerPrefs.GetFloat("volumeSound", 0);
        volumeMusic = PlayerPrefs.GetFloat("volumeMusic", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
