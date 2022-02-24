using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Андрей
//Семен
/// <summary>
/// Контролирует сообщения, движение, и несколько функций для кнопок
/// </summary>
public class MessageCTRL : MonoBehaviour
{
    [SerializeField]
    Animator myAnimator;

    public static List<MessageCTRL> BufferMessages = new List<MessageCTRL>();

    [SerializeField]
    public Text title;
    [SerializeField]
    Text message;
    [SerializeField]
    Text button;

    [SerializeField] private bool levelInfo;

    /////////////////////////////////////////////////////////////////////////////////////
    /////// Новые функциии управления буфером сообщений
    
    //Сделать текущее сообщение первым в списке
    public void OpenMessageBuffer() {
        //сперва ищем это сообщение в буффере

        //Создаем новый буфер без этого сообщения
        List<MessageCTRL> BufferMessagesPlus = new List<MessageCTRL>();
        //Добавляем старые сообщения без этого
        foreach (MessageCTRL message in BufferMessages) {
            if (message != this && message != null)
            {
                BufferMessagesPlus.Add(message);
            }
        }

        //Итоговый новый список сообщений
        List<MessageCTRL> BufferMessageNew = new List<MessageCTRL>();

        //запихиваем это сообщение на первое место
        BufferMessageNew.Add(this);
        //Запихиваем все оставшиеся сообщения в список
        foreach (MessageCTRL message in BufferMessagesPlus) {
            BufferMessageNew.Add(message);
        }

        //заменяем буфер с текущим сообщением первым в списке
        BufferMessages = BufferMessageNew;
    }

    //Закрыть сообщение удалив из списка
    public void DeleteMessageBuffer() {
        //Создаем новый список без этого сообщения
        List<MessageCTRL> BufferMessagesNew = new List<MessageCTRL>();
        
        //Перебираем сообщения исключая пустые и текущее
        foreach (MessageCTRL message in BufferMessages) {
            if (message == this || message == null)
                continue;

            BufferMessagesNew.Add(message);
        }

        BufferMessages = BufferMessagesNew;
    }


    //Заставляет первое сообщение в буфере вывестись поверх всех окон и прячет остальные
    int oldNum = -1;
    bool destroyNeed = false;
    void TestAnimation() {
        for (int num = 0; num < BufferMessages.Count; num++) {

            //Если мы нашли это сообщение в списке
            if (BufferMessages[num] == this) {

                //Если его номер отличается в памяти
                if (oldNum == num) return;

                //Показываем
                if (oldNum != 0 && num == 0) {
                    myAnimator.SetBool("Opening", true);
                }
                //Прячем
                else if (oldNum == 0 && num != 0){
                    myAnimator.SetBool("Opening", false);
                }

                //запоминаем измерения
                oldNum = num;

                //Выходим т.к. нужные действия выполнены
                return;
            }
        }

        //Если список закончился но сообщение так и не нашлось.. Значит его нет в списке, в любом случае закрываем.
        myAnimator.SetBool("Opening", false);
        //Говорим что необходимо удалить
        destroyNeed = true;
    }

    public void TestDelete() {
        if (!destroyNeed) return;

        Destroy(gameObject);
    }

    private void Update()
    {
        TestAnimation();
    }


    ///конец новых сообщений буфера
    /// ////////////////////////////////////////////////////////////////////////////////

    public void setMessage(string titleFunc, string messageFunc, string buttonFunc) {
        title.text = titleFunc;
        message.text = messageFunc;
        button.text = buttonFunc;
    }

    /// <summary>
    /// Закрыть сообщение
    /// </summary>
    public void ClickButtonClose() {
        /*
        if(myAnimator)
            myAnimator.SetBool("Close", true);
        */

        DeleteMessageBuffer();
    }
}
