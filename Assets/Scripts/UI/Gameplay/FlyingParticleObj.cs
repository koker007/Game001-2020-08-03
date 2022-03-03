using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingParticleObj : MonoBehaviour
{

    [SerializeField]
    RectTransform rectSpawn;
    [SerializeField]
    RectTransform rectCenter;
    [SerializeField]
    GameObject sizeObj;

    [SerializeField]
    Vector2 PositionNeed;
    [SerializeField]
    Image sprite;
    [SerializeField]
    RawImage image;


    Vector2 vectorMove = new Vector2(0,0); //Вектор движения объекта
    Vector2 sizeStart = new Vector2(100,100);
    float speedNow = 0; //Текущая скорость
    float distStart = 0; //Стартовое растояние до цели, чтобы понимать нужно ли ускоряться или замедляться
    float speedRotateNow = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateTransform();
    }



     public void IniFlyingParticleData(GameObject ParentObj, Texture image, Sprite sprite, Vector2 sizeimage, RectTransform rectStart, RectTransform rectTarget) {
        //Родитель внутри которого перемещаемся
        if (gameObject.transform.parent != ParentObj.transform)
            gameObject.transform.parent = ParentObj.transform;


        //Устанавливаем изображения //Спрайт в приоритере
        if (sprite != null)
        {
            this.sprite.sprite = sprite;
            this.sprite.type = Image.Type.Sliced;

            this.sprite.gameObject.SetActive(true);
            this.image.gameObject.SetActive(false);
        }
        else if (image != null)
        {
            this.image.texture = image;

            this.image.gameObject.SetActive(true);
            this.sprite.gameObject.SetActive(false);
        }

        //Устанавливаем начальную позицию и размер
        Vector2 startPos = new Vector2(rectStart.position.x + sizeimage.x, rectStart.position.y + sizeimage.y/2);
        rectSpawn.position = startPos;
        sizeStart = sizeimage * 100;
        rectSpawn.sizeDelta = sizeStart;

        //говорим куда надо двигаться
        PositionNeed = rectTarget.position;

        //узнаем растояние между текущим положением и целевым
        distStart = Vector2.Distance(
            new Vector2(rectCenter.position.x, rectCenter.position.y), 
            new Vector2(PositionNeed.x, PositionNeed.y));

        //Устанавливаем рандомный вектор
        vectorMove = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        vectorMove.Normalize();

        //Рандомизируем стартовое вращение
        speedRotateNow = Random.Range(1f, 3f);
        if(Random.Range(0,100) < 50)
            speedRotateNow *= -1;

    }

    void updateTransform() {

        //Узнаем текущее растояние до цели
        float distNow = Vector2.Distance(
            new Vector2(rectCenter.position.x, rectCenter.position.y),
            new Vector2(PositionNeed.x, PositionNeed.y));

        //Если дистанция больше половины
        if (distNow > distStart/2) {
            //Прибавляем скорость
            speedNow += Time.deltaTime * 10;
        }
        else {
            //Уменьшаем скорость, так чтобы она никогда не стала отрицательной
            speedNow -= speedNow * Time.deltaTime;
            //Если скорость стала меньше времени то плохо
            if (speedNow < Time.deltaTime) speedNow = Time.deltaTime;
        }

        //Узнаем вектор до цели
        Vector2 vectorTarget = new Vector2(
            PositionNeed.x - rectCenter.position.x, 
            PositionNeed.y - rectCenter.position.y);

        vectorTarget.Normalize();

        //вычисляем новый вектор полета
        float lerpCoof = Time.deltaTime * speedNow;
        if (lerpCoof > 1) lerpCoof = 1;
        vectorMove = Vector2.Lerp(vectorMove, vectorTarget, lerpCoof);
        

        //считаем новые кординаты
        float xNew = rectCenter.position.x + vectorMove.x * speedNow; //поx
        float yNew = rectCenter.position.y + vectorMove.y * speedNow; //поy

        //Перемещаем объект от текущего положения
        rectCenter.position = new Vector3(xNew, yNew);

        distNow = Vector2.Distance(
            new Vector2(rectCenter.position.x, rectCenter.position.y),
            new Vector2(PositionNeed.x, PositionNeed.y));


        ////////////////////////////////////////////////////////
        //Вращение с разгоном
        speedRotateNow += speedRotateNow * Time.deltaTime * 0.5f;
        rectCenter.Rotate(0,0, speedRotateNow);

        //уменьшаем объект
        float scale = 1;
        //Если текущая дистанция меньше четверти
        if (distNow < distStart/4) {
            scale = distNow / (distStart / 4);
        }
        //Применяем новый размер только ели он меньше чем тот что шас
        if(sizeObj.transform.localScale.x >= scale)
            sizeObj.transform.localScale = new Vector3(scale, scale, scale); //sizeStart/2 * scale;

        //если дистанция еще большая выыходим
        if (distNow > 0.1f) return;

        //Удаляем
        Destroy(gameObject);
    }
}
