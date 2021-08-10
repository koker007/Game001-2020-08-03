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
        if (Settings.main.VolumeMusic == -80)
        {
            Settings.main.VolumeMusic = 0;
        }
        else {
            Settings.main.VolumeMusic = -80;
        }
    }
    public void ClickButtonSound() {
        if (Settings.main.VolumeSound == -80)
        {
            Settings.main.VolumeSound = 0;
        }
        else
        {
            Settings.main.VolumeSound = -80;
        }
    }
    public void ClickButtonSave() {

    }

    //��������� ����� ����������� ��������� �� ������
    void imagesUpdate() {
        if (Settings.main.VolumeMusic == -80)
        {
            ImageMusic.texture = MusicOff;
        }
        else {
            ImageMusic.texture = MusicOn;
        }

        if (Settings.main.VolumeSound == -80)
        {
            ImageSound.texture = SoundOff;
        }
        else
        {
            ImageSound.texture = SoundOn;
        }
    }
}
