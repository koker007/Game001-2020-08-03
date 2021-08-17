using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�����
/// <summary>
/// ������������ ���������, ��������, � ��������� ������� ��� ������
/// </summary>
public class MessageCTRL : MonoBehaviour
{
    [SerializeField]
    public bool NeedClose = false;
    [SerializeField]
    public bool WorlUIClose;
    [SerializeField]
    float sinTime = 2; //��� �������� �� ������

    [SerializeField]
    Text title;
    [SerializeField]
    Text message;
    [SerializeField]
    Text button;

    private bool WorlUIWasClose;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        startPosition();
    }

    // Update is called once per frame
    void Update()
    {
        moving();
    }
    
    void startPosition()
    {
        rectTransform.pivot = new Vector2(0.5f, 5);
    }
    void moving() {

        //���������
        if (!NeedClose)
        {
            sinTime -= Time.unscaledDeltaTime;
            if (sinTime < 0)
                sinTime = 0;

            float sinPlus = Mathf.Sin(sinTime * 20);

            float y = rectTransform.pivot.y;
            y += (0.5f - y) * Time.unscaledDeltaTime * 5;

            //y += sinPlus * sinTime * 0.05f;

            if (Mathf.Abs(y) > 3 || sinTime > 2) { 
                y = -3;
                sinTime = 1;
            }
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, y);

            //����������� worldUI ��� ��������� ���������
            if(WorlUIClose)
            {
                MainComponents.Vertical.SetActive(false);
                if (MainComponents.PanelMap.activeSelf)
                {
                    WorlUIWasClose = true;
                    MainComponents.PanelMap.SetActive(false);
                }
            }
        }
        //���������
        else {
            sinTime += Time.unscaledDeltaTime;

            float sinPlus = Mathf.Sin(sinTime * 20);
            float y = rectTransform.pivot.y;

            float minusTime = sinTime - 0.25f;
            if (minusTime < 0) minusTime = 0;
            y += minusTime;

            float sinTimeCut = 0.5f - sinTime;
            if (sinTimeCut < 0) sinTimeCut = 0;
            y += sinPlus * sinTimeCut * 0.2f;

            rectTransform.pivot = new Vector2(rectTransform.pivot.x, y);

            //��������� worldUI
            if (WorlUIClose)
            {
                MainComponents.Vertical.SetActive(true);
                if (WorlUIWasClose)
                {
                    MainComponents.PanelMap.SetActive(true);
                    WorlUIClose = false;
                }
            }

            //�������� �������
            if (Mathf.Abs(y) > 10) {
                Destroy(gameObject);
            }
        }


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
        NeedClose = true;
        GlobalMessage.Close();
    }
}
