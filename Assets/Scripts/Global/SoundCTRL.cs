using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//alexandr
//семен
/// <summary>
/// контроллер звуков и музыки
/// </summary>
public class SoundCTRL : MonoBehaviour
{
    private AudioSource Sound;
    private AudioSource Music;


    private class AudioSourceParametrs {
        public AudioSource Source;
        public float TimeStart;
    }
    private AudioSourceParametrs[] Arrays;

    [Header("Parents")]
    [SerializeField]
    public GameObject ParentOfSounds;
    [SerializeField]
    public GameObject ParentOfMusic;


    [Header("Clip")]
    [Header("Music")]
    public AudioClip clipEventOk;
    public AudioClip clipEventBad;
    public AudioClip clipLVLComplite;
    public AudioClip clipLVLFailed;
    public AudioClip clipLVL;

    [Header("Clip")]
    [Header("Sounds")]
    [SerializeField]
    public AudioClip clipPressButton;

    public AudioClip clipCellSelect;
    public AudioClip clipMoveCrystal;
    public AudioClip clipMoveReturn;
    public AudioClip clipDesctoyCrystal;

    public AudioClip clipExploseBomb;
    public AudioClip clipExploseRocket;
    public AudioClip clipExploseColor5;
    public AudioClip clipFlyStart;
    public AudioClip clipFlyProcess;
    public AudioClip clipFlyEnd;

    public AudioClip clipDamageRock;
    public AudioClip clipDamageMold;
    public AudioClip clipDamageAcne;
    public AudioClip clipAddPanel;
    public AudioClip clipAddMold;

    public static SoundCTRL main;

    private void Awake()
    {
        main = this;

        inicialize();

        Sound = GameObject.Find("Sound").GetComponent<AudioSource>();
        Music = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    void inicialize() {

        iniSourceSound();

        ////////////////////////////////////////////////////////////////////
        void iniSourceSound() {
            //получаем список всех источников 
            AudioSource[] audioSources = ParentOfSounds.GetComponentsInChildren<AudioSource>();

            //—оздаем такой же массив источников
            Arrays = new AudioSourceParametrs[audioSources.Length];

            for (int num = 0; num < audioSources.Length; num++)
            {
                //создаем экземпл€р массива если нету
                if (Arrays[num] == null)
                {
                    Arrays[num] = new AudioSourceParametrs();
                }

                //≈сли источник не обнаружен, выходим
                if (audioSources[num] == null)
                {
                    continue;
                }

                //ѕринимаем источник звука
                Arrays[num].Source = audioSources[num];
            }
        }
    }

    /// <summary>
    /// проигрывание звукового клипа
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        Sound.PlayOneShot(clip);
    }


    //¬оспроизвести короткий звук по умному приоритету
    public void SmartPlaySound(AudioClip clip, float volume, float pitch) {
        //»щем свободный источник

        //¬ыбираем наиболее старый источник звука
        AudioSourceParametrs sourcePrioritet = null;
        float prioritet = 0;

        //¬ыбираем наименее приоритетный источник
        foreach (AudioSourceParametrs array in Arrays) {
            //≈сли в массиве нет источника
            if (array == null ||
                array.Source == null) {
                continue;
            }

            //≈сли сурса в приритете нет, то дабавим текущий
            if (sourcePrioritet == null) {
                sourcePrioritet = array;
                sourcePrioritet.Source.clip = clip;
                sourcePrioritet.TimeStart = 0;
                prioritet = calcPrioritet(sourcePrioritet.Source, sourcePrioritet.TimeStart);
            }


            //”знаем приоритет текущий
            float prioritetArray = calcPrioritet(array.Source, array.TimeStart);

            //¬ыбираем сурс с наименьшим приоритетом
            if (prioritet > prioritetArray) {
                sourcePrioritet = array;
                prioritet = prioritetArray;
            }
        }

        //»спользуем приоритетный сурс дл€ воспроизведени€
        if (sourcePrioritet != null) {
            sourcePrioritet.Source.volume = Settings.main.VolumeAllFrom0To1 * Settings.main.VolumeSoundFrom0To1 * volume;
            sourcePrioritet.Source.pitch = pitch;
            sourcePrioritet.TimeStart = Time.unscaledTime;

            sourcePrioritet.Source.clip = clip;
            sourcePrioritet.Source.PlayOneShot(clip);
        }
        else {
            Debug.Log("Sound Not play");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //–асчитать приоритет звука в зависимости от его громкости и давности
        float calcPrioritet(AudioSource source, float timeStartPlay) {
            float timePlaying = Time.unscaledTime - timeStartPlay;

            //≈сли питч около нулевой
            if (source.pitch < 0.01f && source.pitch > -0.01f)
                source.pitch = 0.01f;


            float timePlayNeed = 0;
            
            if(source.clip != null)
                timePlayNeed = source.clip.length / Mathf.Abs(source.pitch);

            //если врем€ воспроизведени€ около нулевое
            if (timePlayNeed <= 0.001f) 
                timePlayNeed = 0.001f;

            float result = (1-(timePlaying / timePlayNeed))*source.volume;
            return result;
        }
    }

    //воспроизветсти клип, громкость 1, питч 1
    public void SmartPlaySound(AudioClip clip) {
        SmartPlaySound(clip, 1, 1);
    }


    const float playDestroyZaderzhka = 0.05f;
    void PlaySoundDestroy()
    {
        float time = Time.unscaledTime - CellInternalObject.timeLastPlayDestroy;
        float pith = 1f + (time / playDestroyZaderzhka) * 0.05f;
        if (pith < 0.25)
            pith = 0.25f;

        SmartPlaySound(clipDesctoyCrystal, 1, pith);
    }

    public void PlaySoundDestroyInvoke()
    {
        //≈сли звук не был очень давно
        if (CellInternalObject.timeLastPlayDestroy < Time.unscaledTime)
        {
            CellInternalObject.timeLastPlayDestroy = Time.unscaledTime;
        }

        //прибавл€ем врем€ запуска
        CellInternalObject.timeLastPlayDestroy += playDestroyZaderzhka;

        //“еперь врем€ запуска больше текущего момента

        Invoke("PlaySoundDestroy", CellInternalObject.timeLastPlayDestroy - Time.unscaledTime);

    }
}
