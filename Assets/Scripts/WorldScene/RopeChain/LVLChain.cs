using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLChain : MonoBehaviour
{

    static List<LVLChain> allChains = new List<LVLChain>();

    [SerializeField]
    LevelButton3D myButton;

    public LVLChain back;
    public LVLChain next;

    //¬ектор показывающий направление взгл€да вперед
    public Vector3 forvard;

    [SerializeField]
    RopeChain myRope;

    public void iniChain() {

        //”дал€ем пустоты в списке
        ClearVoidInList();

        //ƒобавл€ем себ€ в список цепочек уровней
        AddMeInList();

        //≈сли цепочки сзади нету
        if (back == null) {
            //»щем уровень
            back = getChainFromLvlNum(myButton.NumLevel - 1);

            //≈сли уровень сзади нашли добавл€ем себ€ туда
            if (back) {
                back.next = this;
            }
        }

        //≈сли цепочки спереди нету
        if (next == null) {
            next = getChainFromLvlNum(myButton.NumLevel + 1);
            
            //≈сли уровень спереди нашли, добавл€ем себ€ туда
            if (next) {
                next.back = this;
            }
        }

        //ищем вектор наравлени€ маршрута
        Vector3 old = new Vector3(0, 0, 0);
        Vector3 future = new Vector3(0, 0, 0);
        forvard = new Vector3(0, 0, 0);


        //≈сли есть предыдуща€ позици€
        if (back)
        {
            old = gameObject.transform.position - back.gameObject.transform.position;
            forvard = old;
        }
        //≈сли есть следующа€ точка
        if (next)
        {
            future = next.gameObject.transform.position - gameObject.transform.position;
            forvard = future;
        }

        //≈сли есть точка сзади и спереди
        if (back != null && next != null)
        {
            //находим вектор захода и выхода из точки, это средний вектор между прошлым движением и будущим
            forvard = (old + future)/2;
        }


        //«вень€ цепи настроенны есть направл€ющий вектор
        //“еперь создаем саму визуализацию звена от певого к следующему

        //—оздаем звено от текущего к следующему


        if(back != null)
            back.ReCalcVisual();

        ReCalcVisual();

        if(next != null)
            next.ReCalcVisual();



        ///////////////////////////////////////////////////////////////////////////////////

        //”далить пустоты в списке
        void ClearVoidInList() {
            List<LVLChain> allChainsNew = new List<LVLChain>();

            foreach (LVLChain chain in allChains) {
                if (chain == null) continue;

                allChainsNew.Add(chain);
            }

            allChains = allChainsNew;
        }

        //проверить есть ли € в списке и добавить если нету
        void AddMeInList() {
            bool found = false;

            foreach (LVLChain lVlChain in allChains) {
                if (lVlChain == this) {
                    found = true;
                    break;
                }
            }

            //ƒобавл€ем себ€ в список уровней
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

        //ѕересчитываем позицию моего и соседних звеньев с учетом новых данных
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

    //ѕолучить позицию, передать прогресс движени€ от 0 до 1
    public Vector3 GetPosition(float progress, float strange)
    {
        Vector3 result = gameObject.transform.position;

        //≈сли нулевой прогресс или следующей позиции нет
        if (progress <= 0 || next == null)
        {
            return result;
        }
        //≈сли прогресс больше 1 то равно окончанию
        else if (progress >= 1)
        {
            return next.gameObject.transform.position;
        }

        //Ќаходим промежуточное значение по пр€мой между двум€ точками
        Vector3 posInLine = Vector3.Lerp(transform.position, next.transform.position, progress);

        //Ќаходим промежуточное смещение по направл€ющим векторам
        Vector3 offsetVector = vectorLerp(forvard.normalized, next.forvard.normalized, progress);

        Vector3 vectorRoute = forvard.normalized - next.forvard.normalized;

        offsetVector = new Vector3((1 - offsetVector.normalized.x) * -1, 0, 0);

        //¬ектор в направлении к цели 
        Vector3 nextVector = next.transform.position - transform.position;

        /*
        //ћен€ем местами x.y и отражаем -y в месте дл€ x
        nextVector = new Vector3(-nextVector.y, nextVector.x, nextVector.z);
        //ћен€ем местами y.z и отражаем -z в месте дл€ y
        nextVector = new Vector3(nextVector.x, -nextVector.z, nextVector.y);
        //ћен€ем местами z.x 
        //nextVector = new Vector3(nextVector.z, nextVector.y, -nextVector.x);
        nextVector = new Vector3(nextVector.x, 0, 0);
        */

        float offsetProgress = progress;
        //≈сли прогресс больше 0.5 то реверс
        if (offsetProgress < 0.25f) {
            offsetProgress = offsetProgress * 4;
        }
        else if (offsetProgress < 0.75) {
            //offsetProgress = 0.5f - (progress - 0.5f);
            offsetProgress = 1;
        }
        else {
            offsetProgress = 0.25f - (progress - 0.75f);
            offsetProgress *= 4;
        }

        float magnitude = forvard.magnitude;
        if (magnitude > 1) {
            magnitude = 1;
        }

        //нашли промежуточную точку и смещение. вычисл€ем результат
        result = posInLine + offsetVector * offsetProgress * -vectorRoute.x * magnitude * strange;

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
