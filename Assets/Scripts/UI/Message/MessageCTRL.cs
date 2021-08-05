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
    bool NeedClose = false;
    [SerializeField]
    float sinTime = 2; //Для дрожания по косинусу

    [SerializeField]
    Text text;

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
            y += (0.5f - y) * Time.unscaledDeltaTime;
            y += sinPlus * sinTime * 0.05f;
            rectTransform.pivot = new Vector2(rectTransform.pivot.x, y);
        }
        //Исчезание
        else {
            sinTime += Time.unscaledDeltaTime;

            float sinPlus = Mathf.Sin(sinTime);
            float y = rectTransform.pivot.y;

            y += sinTime;
            y += sinPlus * sinTime;

            rectTransform.pivot = new Vector2(rectTransform.pivot.x, y);
        }
    }
}
