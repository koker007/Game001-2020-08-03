using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// �������� �� ��������� �������� � ���������� ����
/// </summary>
public class MessageSettings : MonoBehaviour
{
    [Header("Images")]
    [SerializeField]
    RawImage ImageMusic;
    [SerializeField]
    RawImage ImageSound;
    [SerializeField]
    RawImage ImageSave;

    [Header("Textures")]
    [SerializeField]
    Texture MusicOff;
    [SerializeField]
    Texture MusicOn;
    [SerializeField]
    Texture SoundOff;
    [SerializeField]
    Texture SoundOn;

    private void Update()
    {
        imagesUpdate();
    }

    public void ClickButtonMusic() {
        if (Settings.main.VolumeMusic > 0.5f)
        {
            Settings.main.VolumeMusic = 1;
        }
        else {
            Settings.main.VolumeMusic = 0;
        }
    }
    public void ClickButtonSound() {
        if (Settings.main.VolumeSound > 0.5f)
        {
            Settings.main.VolumeSound = 1;
        }
        else
        {
            Settings.main.VolumeSound = 0;
        }
    }
    public void ClickButtonSave() {

    }

    //��������� ����� ����������� ��������� �� ������
    void imagesUpdate() {
        if (Settings.main.VolumeMusic == 0)
        {
            ImageMusic.texture = MusicOff;
        }
        else {
            ImageMusic.texture = MusicOn;
        }

        if (Settings.main.VolumeSound == 0)
        {
            ImageMusic.texture = SoundOff;
        }
        else
        {
            ImageMusic.texture = SoundOn;
        }
    }
}
