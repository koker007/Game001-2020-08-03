using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
/// <summary>
/// управляет звуками и музыкой
/// </summary>
public class SoundManager : MonoBehaviour
{
    private AudioSource Sound;
    private AudioSource Music;

    public AudioClip PressButton;
    public AudioClip MoveCrystal;
    public AudioClip DesctoyCrystal;

    public static SoundManager main;

    private void Awake()
    {
        main = this;

        Sound = GameObject.Find("Sound").GetComponent<AudioSource>();
        Music = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    /// <summary>
    /// проигрывание звукового клипа
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        Sound.PlayOneShot(clip);
    }
}
