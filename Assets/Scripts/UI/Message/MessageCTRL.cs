using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������
//�����
/// <summary>
/// ������������ ���������, ��������, � ��������� ������� ��� ������
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

    //������������ ��������� �� �� ����
    static public void TestBufferMessages() {
        foreach (MessageCTRL message in BufferMessages) {

        }
    }

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
    /// ������� ���������
    /// </summary>
    public void ClickButtonClose() {
        if(myAnimator)
            myAnimator.SetBool("Close", true);

    }

    public void TryDestroy()
    {
        //���� ������ ��������� � ������
        foreach (MessageCTRL message in BufferMessages) {
            if (message == this) {
                return;
            }
        }

        //������� ������ �������
        Destroy();
    }

    public void Destroy()
    {
        DeleteInBuffer();
        Destroy(gameObject);
    }

    public void DeleteInBuffer() {
        //����� ������ ��������� � ������
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

        //���� ���� ������� ��������� �� �������� �� ����� �� �����
        if (selected != null) return;

        //��������� ��� ��� ��������� ���� � ������ ��������� � ��� ��� ���������
        bool meLast = false;
        bool mefound = false;
        for (int num = 0; num < BufferMessages.Count; num++) {
            if (BufferMessages[num] == this)
            {
                mefound = true;
                if (num == BufferMessages.Count - 1) meLast = true;
            }
        }

        //���� ���� �� ����� ������� ��� ���������
        if (!mefound) {
            //������� ������ �������
            Destroy(gameObject);
        }

        //������� ���� � �� ��������� ���
        if (!meLast) return;

        //���� ����������� � ��������� �� ������� ��� ���������
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
