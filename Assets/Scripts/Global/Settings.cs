using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//�����
/// <summary>
/// ������ � ����������� ����
/// </summary>
public class Settings : MonoBehaviour
{
    //��� �������� ������ ����
    static public Settings main;

    public AudioMixer audioMixer;

    [SerializeField]
    float volumeAll = 0;
    [SerializeField]
    float volumeSound = 0;
    [SerializeField]
    float volumeMusic = 0;

    /// <summary>
    /// ����� ��������� �� 0 �� 1
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
            audioMixer.SetFloat("Master", volumeAll);
        }
    }
    /// <summary>
    ///��������� ������ �� 0 �� 1
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
            audioMixer.SetFloat("Sound", volumeSound);
        }
    }
    /// <summary>
    ///��������� ������ �� 0 �� 1
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
            audioMixer.SetFloat("Music", volumeMusic);
        }
    }

    bool screenIsVertical = true;

    /// <summary>
    /// ������������ �� ��������� ������
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
