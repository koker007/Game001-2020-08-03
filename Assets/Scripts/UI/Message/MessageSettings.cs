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
    public Dropdown launguageDropdown;

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
        DropdownUpdate();
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
        //��������� ��������
        PlayerProfile.main.Save();
    }

    public void ClickDropdownLaunguage()
    {
        switch (launguageDropdown.value)
        {
            case 0:
                Settings.main.SetLaunguage("English");
                break;
            case 1:
                Settings.main.SetLaunguage("Russian");
                break;
        }
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

    private void DropdownUpdate()
    {
        switch (Settings.main.launguage)
        {
            case "English":
                launguageDropdown.value = 0;
                break;
            case "Russian":
                launguageDropdown.value = 1;
                break;
        }
    }
}
