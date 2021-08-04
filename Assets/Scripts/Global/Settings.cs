using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен
/// <summary>
/// Скрипт с настройками игры
/// </summary>
public class Settings : MonoBehaviour
{
    //для хранения самого себя
    static Settings main;

    [SerializeField]
    float volumeAll = 1;
    [SerializeField]
    float volumeSound = 1;
    [SerializeField]
    float volumeMusic = 1;

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
            if (value < 0)
            {
                main.volumeAll = 0;
            }
            else if (value > 1)
            {
                main.volumeAll = 1;
            }
            else
            {
                main.volumeAll = value;
            }
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
            if (value < 0)
            {
                main.volumeSound = 0;
            }
            else if (value > 1)
            {
                main.volumeSound = 1;
            }
            else {
                main.volumeSound = value;
            }
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
            if (value < 0)
            {
                main.volumeMusic = 0;
            }
            else if (value > 1)
            {
                main.volumeMusic = 1;
            }
            else
            {
                main.volumeMusic = value;
            }
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

    // Start is called before the first frame update
    void Start()
    {
        main = this;
    }

    

    // Update is called once per frame
    void Update()
    {

    }
}
