using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLChain : MonoBehaviour
{

    static List<LVLChain> allChains = new List<LVLChain>();

    [SerializeField]
    LevelButton myButton;

    public LVLChain back;
    public LVLChain next;

    //Вектор показывающий направление взгляда вперед
    public Vector3 forvard;

    [SerializeField]
    RopeChain myRope;

    public void iniChain() {

        //Удаляем пустоты в списке
        ClearVoidInList();

        //Добавляем себя в список цепочек уровней
        AddMeInList();

        //Если цепочки сзади нету
        if (back == null) {
            //Ищем уровень
            back = getChainFromLvlNum(myButton.NumLevel - 1);

            //Если уровень сзади нашли добавляем себя туда
            if (back) {
                back.next = this;
            }
        }

        //Если цепочки спереди нету
        if (next == null) {
            next = getChainFromLvlNum(myButton.NumLevel + 1);
            
            //Если уровень спереди нашли, добавляем себя туда
            if (next) {
                next.back = this;
            }
        }

        //ищем вектор наравления маршрута
        Vector3 old = new Vector3(0, 0, 0);
        Vector3 future = new Vector3(0, 0, 0);
        forvard = new Vector3(0, 0, 0);


        //Если есть предыдущая позиция
        if (back)
        {
            old = gameObject.transform.position - back.gameObject.transform.position;
            forvard = old;
        }
        //Если есть следующая точка
        if (next)
        {
            future = next.gameObject.transform.position - gameObject.transform.position;
            forvard = future;
        }

        //Если есть точка сзади и спереди
        if (back != null && next != null)
        {
            //находим вектор захода и выхода из точки, это средний вектор между прошлым движением и будущим
            forvard = (old + future)/2;
        }


        //Звенья цепи настроенны есть направляющий вектор
        //Теперь создаем саму визуализацию звена от певого к следующему

        //Создаем звено от текущего к следующему


        if(back != null)
            back.ReCalcVisual();

        ReCalcVisual();

        if(next != null)
            next.ReCalcVisual();



        ///////////////////////////////////////////////////////////////////////////////////

        //Удалить пустоты в списке
        void ClearVoidInList() {
            List<LVLChain> allChainsNew = new List<LVLChain>();

            foreach (LVLChain chain in allChains) {
                if (chain == null) continue;

                allChainsNew.Add(chain);
            }

            allChains = allChainsNew;
        }

        //проверить есть ли я в списке и добавить если нету
        void AddMeInList() {
            bool found = false;

            foreach (LVLChain lVlChain in allChains) {
                if (lVlChain == this) {
                    found = true;
                    break;
                }
            }

            //Добавляем себя в список уровней
            if (!found)
                allChains.Add(this);
        }

        LVLChain getChainFromLvlNum(int lvl) {
            foreach (LVLChain chain in allChains) {
                if (chain.myButton.NumLevel == lvl) {
                    return chain;
                }
            }

            return null;
        }

    }

    void ReCalcVisual() {
        TestCreateRope();

        //Пересчитываем позицию моего и соседних звеньев с учетом новых данных
        if (back != null && back.myRope != null)
            back.myRope.ReCalcPositions3(back);

        myRope.ReCalcPositions3(this);

        if (next != null && next.myRope != null)
            next.myRope.ReCalcPositions3(next);

        void TestCreateRope()
        {
            if (myRope == null)
            {
                GameObject ropeObj = Instantiate(WorldRopeChainCTRL.main.PrefabRopeChain.gameObject);
                ropeObj.transform.parent = myButton.transform.parent.parent;
                myRope = ropeObj.GetComponent<RopeChain>();
            }
            else
            {
                bool test = false;
            }
        }
    }

    //Получить позицию, передать прогресс движения от 0 до 1
    public Vector3 GetPosition(float progress, float strange)
    {
        Vector3 result = gameObject.transform.position;

        //Если нулевой прогресс или следующей позиции нет
        if (progress <= 0 || next == null)
        {
            return result;
        }
        //Если прогресс больше 1 то равно окончанию
        else if (progress >= 1)
        {
            return next.gameObject.transform.position;
        }

        //Находим промежуточное значение по прямой между двумя точками
        Vector3 posInLine = Vector3.Lerp(transform.position, next.transform.position, progress);

        //Находим промежуточное смещение по направляющим векторам
        Vector3 offsetVector = vectorLerp(forvard.normalized, -next.forvard.normalized, 0.5f);
        float magnitude = Vector3.Distance(forvard.normalized, next.forvard.normalized);
        offsetVector = new Vector3((1 - offsetVector.normalized.x) * -1, 0, 0);

        //Вектор в направлении к цели 
        Vector3 nextVector = next.transform.position - transform.position;

        //Меняем местами x.y и отражаем -y в месте для x
        nextVector = new Vector3(-nextVector.y, nextVector.x, nextVector.z);
        //Меняем местами y.z и отражаем -z в месте для y
        nextVector = new Vector3(nextVector.x, -nextVector.z, nextVector.y);
        //Меняем местами z.x 
        //nextVector = new Vector3(nextVector.z, nextVector.y, -nextVector.x);
        nextVector = new Vector3(nextVector.x, 0, 0);

        //Меняем местами x.z и отражаем z в месте x
        //nextVector = new Vector3(-nextVector.z, nextVector.y, nextVector.x);
        //Меняем местами z.y и отражаем y в месте z
        //nextVector = new Vector3(nextVector.x, nextVector.z, -nextVector.y);

        //Меняем местами x.y
        //nextVector = new Vector3(-nextVector.y, nextVector.x, nextVector.z);
        //Меняем местами y.z
        //nextVector = new Vector3(nextVector.x, -nextVector.z, nextVector.y);

        //Vector3 offsetVector = nextVector;

        float offsetProgress = progress;
        //Если прогресс больше 0.5 то реверс
        if (offsetProgress > 0.5f) {
            offsetProgress = 0.5f - (progress - 0.5f);
        }
        //else if (offsetProgress >= 0.75f && offsetProgress <= 1) {
        //    offsetProgress = 0.25f - (progress - 0.75f);
        //    offsetProgress *= -1;
        //}

        //нашли промежуточную точку и смещение. вычисляем результат
        result = posInLine + offsetVector * offsetProgress * strange;

        return result;

        Vector3 vectorLerp(Vector3 a, Vector3 b, float interpolate) {
            float intA = 1 - interpolate;
            a = a * intA;

            b = b * interpolate;

            return a + b;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        iniChain();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
