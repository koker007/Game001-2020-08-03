using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Семен
/// <summary>
/// Контролирует основной канвас и все UI
/// </summary>
public class UICTRL : MonoBehaviour
{
    /// <summary>
    /// Основной UICTRL
    /// </summary>
    static public UICTRL main;

    [SerializeField]
    GameObject UILoading; //Меню загрузки игры
    [SerializeField]
    GameObject UIHello; //Меню приветствия
    [SerializeField]
    GameObject UIWorld; //Главное меню
    [SerializeField]
    GameObject UIGameplay; //Игровой процесс

    // Start is called before the first frame update
    void Start()
    {
        main = this;
        OpenLoading();
    }

    

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }



    float loadingPlayTime = 0; //Временная переменная для выключания загрузки.


    /// <summary>
    /// Закрыть все окна
    /// </summary>
    void CloseAll() {
        UILoading.SetActive(false);
        UIHello.SetActive(false);
        UIWorld.SetActive(false);
        UIGameplay.SetActive(false);
    }

    /// <summary>
    /// Открыть окно загрузки
    /// </summary>
    public void OpenLoading() {
        CloseAll();
        UILoading.SetActive(true);
    }

    /// <summary>
    /// Открыть приветственное меню
    /// </summary>
    public void OpenHello() {
        CloseAll();
        UIHello.SetActive(true);
    }
    
    /// <summary>
    /// Открыть основное меню карты
    /// </summary>
    public void OpenWorld()
    {
        Gameplay.main.isGameplay = false;
        Time.timeScale = 1;
        Gameplay.main.timeScale = 1;
        CloseAll();
        UIWorld.SetActive(true);
    }

    /// <summary>
    /// Открыть игровой процесс
    /// </summary>
    public void OpenGameplay() {
        CloseAll();
        Gameplay.main.isGameplay = true;
        UIGameplay.SetActive(true);
    }

    //Главная функция обрабатывающая какое из окон показать
    void UpdateUI() {
        if (UILoading.activeSelf) {
            //loadingPlayTime += Time.deltaTime;
            //if (loadingPlayTime > 0.1f) {
                OpenHello();
            //}
        }
        else if (UIHello.activeSelf) {

        }
        else if (UIWorld.activeSelf) {

        }
        else if (UIGameplay.activeSelf) {
            
        }
    }

    public bool isOpenMap() {
        if (UIWorld.activeSelf && MenuWorld.main.isOpenMap())
            return true;

        return false;
    }

    public void ClickButtonOpttions()
    {
        GlobalMessage.Settings();
    }
}
