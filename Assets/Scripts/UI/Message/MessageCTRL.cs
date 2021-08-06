using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Семен
/// <summary>
/// Контролирует сообщения, движение, и несколько функций для кнопок
/// </summary>
public class MessageCTRL : MonoBehaviour
{
    [SerializeField]
    public bool NeedClose = false;
    [SerializeField]
    float sinTime = 2; //Для дрожания по синусу

    [SerializeField]
    Text title;
    [SerializeField]
    Text message;
    [SerializeField]
    Text button;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        moving();
    }

    void moving() {

        //Появление
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
        }
        //Исчезание
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

            
        }


    }

    public void setMessage(string titleFunc, string messageFunc, string buttonFunc) {
        title.text = titleFunc;
        message.text = messageFunc;
        button.text = buttonFunc;
    }

}
