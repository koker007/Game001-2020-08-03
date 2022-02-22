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
    static public MessageCTRL selected;


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
        foreach (MessageCTRL message in BufferMessages) {
            if (message != this && message != null)
            {
                BufferMessagesPlus.Add(message);
            }
        }

        //Новый список сообщений
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

    


    ///конец новых сообщений буфера
    /// ////////////////////////////////////////////////////////////////////////////////

    public void SetSelected() {
        if (selected != null && selected != this) {
            selected.ClickButtonClose();
        }

        selected = this;
    }

    public void setMessage(string titleFunc, string messageFunc, string buttonFunc) {
        title.text = titleFunc;
        message.text = messageFunc;
        button.text = buttonFunc;
    }

    /// <summary>
    /// Закрыть сообщение
    /// </summary>
    public void ClickButtonClose() {
        if(myAnimator)
            myAnimator.SetBool("Close", true);

    }

    public void TryDestroy()
    {
        //Ищем данное сообщение в списке
        foreach (MessageCTRL message in BufferMessages) {
            if (message == this) {
                return;
            }
        }

        //Ненашли значит удаляем
        Destroy();
    }

    public void Destroy()
    {
        DeleteInBuffer();
        Destroy(gameObject);
    }

    public void DeleteInBuffer() {
        //Новый список сообщений в буфере
        List<MessageCTRL> bufferMessagesNew = new List<MessageCTRL>();
        foreach (MessageCTRL messageCTRL in BufferMessages)
        {
            if (messageCTRL == this)
            {
                continue;
            }

            bufferMessagesNew.Add(messageCTRL);
        }

        BufferMessages = bufferMessagesNew;
    }
    public void AddInBuffer() {

        foreach (MessageCTRL messageCTRL in BufferMessages)
        {
            if (messageCTRL == this)
            {
                return;
            }
        }

        BufferMessages.Add(this);
    }

    public void TestOpenFromHide() {
        myAnimator.SetBool("Close", false);

        //Если есть текущее сообщение то проверка на вывод не нужна
        if (selected != null) return;

        //Проверяем что это сообщение есть в списке сообщений и оно там последнее
        bool meLast = false;
        bool mefound = false;
        for (int num = 0; num < BufferMessages.Count; num++) {
            if (BufferMessages[num] == this)
            {
                mefound = true;
                if (num == BufferMessages.Count - 1) meLast = true;
            }
        }

        //Если себя не нашли удаляем это сообщение
        if (!mefound) {
            //Ненашли значит удаляем
            Destroy(gameObject);
        }

        //Выходим если я не последний или
        if (!meLast) return;

        //Если оказывается я последний то выводим это сообщение
        myAnimator.SetBool("Open", true);
    }

    public void SetOpenFalse()
    {
        myAnimator.SetBool("Open", false);
    }
    public void SetOpenTrue() {
        myAnimator.SetBool("Open", true);
        if (selected != null && selected != this)
            selected.SetOpenFalse();

        selected = this;
    }


    static public  void NewMessage(MessageCTRL message) {
        if (selected != null)
        {
            selected.AddInBuffer();
        }
        selected = message;
    }
}
