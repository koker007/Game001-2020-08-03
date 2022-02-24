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
    public Text title;
    [SerializeField]
    Text message;
    [SerializeField]
    Text button;

    [SerializeField] private bool levelInfo;

    /////////////////////////////////////////////////////////////////////////////////////
    /////// ����� �������� ���������� ������� ���������
    
    //������� ������� ��������� ������ � ������
    public void OpenMessageBuffer() {
        //������ ���� ��� ��������� � �������

        //������� ����� ����� ��� ����� ���������
        List<MessageCTRL> BufferMessagesPlus = new List<MessageCTRL>();
        //��������� ������ ��������� ��� �����
        foreach (MessageCTRL message in BufferMessages) {
            if (message != this && message != null)
            {
                BufferMessagesPlus.Add(message);
            }
        }

        //�������� ����� ������ ���������
        List<MessageCTRL> BufferMessageNew = new List<MessageCTRL>();

        //���������� ��� ��������� �� ������ �����
        BufferMessageNew.Add(this);
        //���������� ��� ���������� ��������� � ������
        foreach (MessageCTRL message in BufferMessagesPlus) {
            BufferMessageNew.Add(message);
        }

        //�������� ����� � ������� ���������� ������ � ������
        BufferMessages = BufferMessageNew;
    }

    //������� ��������� ������ �� ������
    public void DeleteMessageBuffer() {
        //������� ����� ������ ��� ����� ���������
        List<MessageCTRL> BufferMessagesNew = new List<MessageCTRL>();
        
        //���������� ��������� �������� ������ � �������
        foreach (MessageCTRL message in BufferMessages) {
            if (message == this || message == null)
                continue;

            BufferMessagesNew.Add(message);
        }

        BufferMessages = BufferMessagesNew;
    }


    //���������� ������ ��������� � ������ ��������� ������ ���� ���� � ������ ���������
    int oldNum = -1;
    bool destroyNeed = false;
    void TestAnimation() {
        for (int num = 0; num < BufferMessages.Count; num++) {

            //���� �� ����� ��� ��������� � ������
            if (BufferMessages[num] == this) {

                //���� ��� ����� ���������� � ������
                if (oldNum == num) return;

                //����������
                if (oldNum != 0 && num == 0) {
                    myAnimator.SetBool("Opening", true);
                }
                //������
                else if (oldNum == 0 && num != 0){
                    myAnimator.SetBool("Opening", false);
                }

                //���������� ���������
                oldNum = num;

                //������� �.�. ������ �������� ���������
                return;
            }
        }

        //���� ������ ���������� �� ��������� ��� � �� �������.. ������ ��� ��� � ������, � ����� ������ ���������.
        myAnimator.SetBool("Opening", false);
        //������� ��� ���������� �������
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


    ///����� ����� ��������� ������
    /// ////////////////////////////////////////////////////////////////////////////////

    public void setMessage(string titleFunc, string messageFunc, string buttonFunc) {
        title.text = titleFunc;
        message.text = messageFunc;
        button.text = buttonFunc;
    }

    /// <summary>
    /// ������� ���������
    /// </summary>
    public void ClickButtonClose() {
        /*
        if(myAnimator)
            myAnimator.SetBool("Close", true);
        */

        DeleteMessageBuffer();
    }
}
