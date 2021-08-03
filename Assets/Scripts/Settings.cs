using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт с настройками игры
/// </summary>
public class Settings : MonoBehaviour
{
    //для хранения самого себя
    static Settings main;

    float volumeAll = 1;
    float volumeSound = 1;
    float volumeMusic = 1;

    /// <summary>
    /// Общая громкость от 0 до 1
    /// </summary>
    float VolumeAll
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
    float VolumeSound {
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
    float VolumeMusic
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
