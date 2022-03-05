using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderIceObj : MonoBehaviour
{
    [SerializeField]
    RectTransform myRectPos; //Смещение и позиция объекта игровом поле
    [SerializeField]
    RectTransform myRectSize; //Размер объекта
    [SerializeField]
    GameFieldCTRL myField; //Игровое поле объекта 

    [SerializeField]
    Image Image;

    [SerializeField]
    public TexturesAndSize[] texturesAndSizes;

    [System.Serializable]
    public class TexturesAndSize {
        public Sprite texture;
        public Vector2Int size;
    }
    TexturesAndSize texturesAndSize;
    public Vector2Int pos = new Vector2Int();

    //Установить поле родителя и добавить себя в этом поле если меня нет
    void SetField(GameFieldCTRL myFieldFunc) {
        myField = myFieldFunc;

        bool found = false;
        foreach (UnderIceObj obj in myField.listUnderIceObj) {
            if (obj == this) {
                found = true;
                break;
            }
        }

        //Добавить меня в список если не нашли
        if (found) return;

        //Добавить список
        myField.listUnderIceObj.Add(this);
        //Установить родителя
        transform.parent = myField.parentOfUnderIceObj;
        
    }

    
    public bool IniObjectSizeAndPos(GameFieldCTRL myFieldFunc, Vector2Int pos, TexturesAndSize texturesAndSizeFunc) {
        bool iniOk = true;

        //Установить поле которому принадлежит данный объект
        myField = myFieldFunc;
        texturesAndSize = texturesAndSizeFunc;
        this.pos = pos;

        //Занести объект в список объектов игрового поля
        SetField(myField);

        //Установить размер и текстуру
        myRectSize.sizeDelta = new Vector2(texturesAndSize.size.x * 100, texturesAndSize.size.y * 100);
        myRectPos.pivot = new Vector2(-1 * pos.x, -1 * pos.y);

        Image.sprite = texturesAndSize.texture;
        Image.type = Image.Type.Sliced;
        

        //проверяем все места на свободность
        for (int x = 0; x < texturesAndSize.size.x; x++) {
            for (int y = 0; y < texturesAndSize.size.y; y++) {
                //Если ячейки нет или место уже занято то ошибка
                if (myField.cellCTRLs[pos.x + x, pos.y + y] == null ||
                    myField.cellCTRLs[pos.x + x, pos.y + y].underIceObj != null) {
                    iniOk = false;
                    break;
                }
            }

            if (!iniOk) break;
        }

        //Если ошибка - выходим
        if (!iniOk) return iniOk;

        //Заполняем ячейки
        for (int x = 0; x < texturesAndSize.size.x; x++)
        {
            for (int y = 0; y < texturesAndSize.size.y; y++)
            {
                myField.cellCTRLs[pos.x + x, pos.y + y].underIceObj = this;
            }
        }

        return iniOk;
    }


    public void IniRandom() {
    
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("InvokeTestLive", Random.Range(0, 1f));
        Invoke("InvokeTestList", Random.Range(0, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Переодическая проверка на жизнь
    void InvokeTestLive() {
        //проверяем позицию подледного объекта на наличие льда
        bool isEnd = true;

        for (int x = 0; x < texturesAndSize.size.x; x++) {
            for (int y = 0; y < texturesAndSize.size.y; y++) {
                //Если ничего не препятствует вытаскиванию то вытаскиваем объект
                if (myField.cellCTRLs[pos.x + x, pos.y + y].ice > 0 ||
                    myField.cellCTRLs[pos.x + x, pos.y + y].Box > 0 ||
                    myField.cellCTRLs[pos.x + x, pos.y + y].rock > 0) {
                    isEnd = false;
                    break;
                }
            }

            if (!isEnd) break;
        }

        //Жизнь еще не закончилась
        if (!isEnd)
        {
            //Проверяем через некоторое время снова
            Invoke("InvokeTestLive", Random.Range(0.2f, 0.8f));
            return;
        }

        //Создаем летаюшую частицу
        GameObject flyingParticleObj = Instantiate(myField.prefabFlyingParticleObj, MenuGameplay.main.transform);
        FlyingParticleObj flyingParticle = flyingParticleObj.GetComponent<FlyingParticleObj>();
        //Инициализируем
        flyingParticle.IniFlyingParticleData(
            MenuGameplay.main.transform.gameObject, null, texturesAndSize.texture, texturesAndSize.size, myRectSize, MenuGameplay.main.Goal[0].GetComponent<RectTransform>(), new Vector2(0,0));

        Destroy(gameObject);

    }
}
