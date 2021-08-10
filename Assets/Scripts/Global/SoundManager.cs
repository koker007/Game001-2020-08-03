using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������� ������� � �������
/// </summary>
public class SoundManager : MonoBehaviour
{
    private AudioSource Sound;
    private AudioSource Music;

    public AudioClip PressButton;

    private void Awake()
    {
        Sound = GameObject.Find("Sound").GetComponent<AudioSource>();
        Music = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    /// <summary>
    /// ������������ ��������� �����
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        Sound.PlayOneShot(clip);
    }
}
