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
            forvard = (old + future)/2;
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
    public Vector3 GetPosition(float progress, float strange)
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
        Vector3 offsetVector = vectorLerp(forvard.normalized, next.forvard.normalized, progress);

        Vector3 vectorRoute = forvard.normalized - next.forvard.normalized;

        offsetVector = new Vector3((1 - offsetVector.normalized.x) * -1, 0, 0);

        //������ � ����������� � ���� 
        Vector3 nextVector = next.transform.position - transform.position;

        /*
        //������ ������� x.y � �������� -y � ����� ��� x
        nextVector = new Vector3(-nextVector.y, nextVector.x, nextVector.z);
        //������ ������� y.z � �������� -z � ����� ��� y
        nextVector = new Vector3(nextVector.x, -nextVector.z, nextVector.y);
        //������ ������� z.x 
        //nextVector = new Vector3(nextVector.z, nextVector.y, -nextVector.x);
        nextVector = new Vector3(nextVector.x, 0, 0);
        */

        float offsetProgress = progress;
        //���� �������� ������ 0.5 �� ������
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

        //����� ������������� ����� � ��������. ��������� ���������
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
