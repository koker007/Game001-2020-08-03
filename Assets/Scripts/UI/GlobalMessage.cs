using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Отвечает за вывод глобальных сообщений на экран
/// </summary>
public class GlobalMessage : MonoBehaviour
{
    static public GlobalMessage main;

    /// <summary>
    /// Конструктор
    /// </summary>
    public GlobalMessage() {
        main = this;
    }

    [SerializeField]
    Image Fon;
    [SerializeField]
    GameObject PrefabMessanger;
    [SerializeField]
    GameObject SelectMessanger;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Закрыть всплывающее окно
    /// </summary>
    static public void Close() {
    
    }

    /// <summary>
    /// Отправить сообщение в сплывающем окне
    /// </summary>
    /// <param name="text"></param>
    static public void Message(string text){

    }
    /// <summary>
    /// Всплывающее окно здоровье
    /// </summary>
    static public void Health() {

    }
    /// <summary>
    /// Всплывающее окно билеты
    /// </summary>
    static public void Tickets() {
        
    }
    static public void Events() {

    }


}
