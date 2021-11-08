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

        //Звенья цепи настроенны
        //Теперь создаем саму визуализацию звена
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
            back.myRope.ReCalcTransform(back);

        myRope.ReCalcTransform(this);

        if (next != null && next.myRope != null)
            next.myRope.ReCalcTransform(next);

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
