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

    //������ ������������ ����������� ������� ������
    public Vector3 forvard;

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

        //���� ������ ���������� ��������
        Vector3 old = new Vector3(0, 0, 0);
        Vector3 future = new Vector3(0, 0, 0);
        forvard = new Vector3(0, 0, 0);


        //���� ���� ���������� �������
        if (back)
        {
            old = gameObject.transform.position - back.gameObject.transform.position;
            forvard = old;
        }
        //���� ���� ��������� �����
        if (next)
        {
            future = next.gameObject.transform.position - gameObject.transform.position;
            forvard = future;
        }

        //���� ���� ����� ����� � �������
        if (back != null && next != null)
        {
            //������� ������ ������ � ������ �� �����, ��� ������� ������ ����� ������� ��������� � �������
            forvard = (old.normalized + future.normalized).normalized;
        }


        //������ ���� ���������� ���� ������������ ������
        //������ ������� ���� ������������ ����� �� ������ � ����������

        //������� ����� �� �������� � ����������


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

    //�������� �������, �������� �������� �������� �� 0 �� 1
    public Vector3 GetPosition(float progress)
    {
        Vector3 result = gameObject.transform.position;

        //���� ������� �������� ��� ��������� ������� ���
        if (progress <= 0 || next == null)
        {
            return result;
        }
        //���� �������� ������ 1 �� ����� ���������
        else if (progress >= 1)
        {
            return next.gameObject.transform.position;
        }

        //������� ������������� �������� �� ������ ����� ����� �������
        Vector3 posInLine = Vector3.Lerp(transform.position, next.transform.position, progress);

        //������� ������������� �������� �� ������������ ��������
        Vector3 offset = Vector3.LerpUnclamped(forvard, next.forvard * -1, progress);

        float offsetSize = progress;
        if (offsetSize > 0.5f) {
            offsetSize =  0.5f-(progress - 0.5f);
        }

        //����� ������������� ����� � ��������. ��������� ���������
        result = posInLine + offset* offsetSize;

        return result;
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
