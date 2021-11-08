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

        //������� ������� � ������
        ClearVoidInList();

        //��������� ���� � ������ ������� �������
        AddMeInList();

        //���� ������� ����� ����
        if (back == null) {
            //���� �������
            back = getChainFromLvlNum(myButton.NumLevel - 1);

            //���� ������� ����� ����� ��������� ���� ����
            if (back) {
                back.next = this;
            }
        }

        //���� ������� ������� ����
        if (next == null) {
            next = getChainFromLvlNum(myButton.NumLevel + 1);
            
            //���� ������� ������� �����, ��������� ���� ����
            if (next) {
                next.back = this;
            }
        }

        //������ ���� ����������
        //������ ������� ���� ������������ �����
        if(back != null)
            back.ReCalcVisual();

        ReCalcVisual();

        if(next != null)
            next.ReCalcVisual();



        ///////////////////////////////////////////////////////////////////////////////////

        //������� ������� � ������
        void ClearVoidInList() {
            List<LVLChain> allChainsNew = new List<LVLChain>();

            foreach (LVLChain chain in allChains) {
                if (chain == null) continue;

                allChainsNew.Add(chain);
            }

            allChains = allChainsNew;
        }

        //��������� ���� �� � � ������ � �������� ���� ����
        void AddMeInList() {
            bool found = false;

            foreach (LVLChain lVlChain in allChains) {
                if (lVlChain == this) {
                    found = true;
                    break;
                }
            }

            //��������� ���� � ������ �������
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

        //������������� ������� ����� � �������� ������� � ������ ����� ������
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
