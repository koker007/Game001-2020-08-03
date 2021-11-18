using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeChain : MonoBehaviour
{
    [SerializeField]
    LVLChain myChain;

    [SerializeField]
    Transform[] posRigs;
    [SerializeField]
    Transform[] posVector;

    [SerializeField]
    float strange = 4f;

    [SerializeField]
    SkinnedMeshRenderer mesh;


    //�������� ������� � ������ ������������� �������
    public void ReCalcPositions3(LVLChain myChainFunc) {
        myChain = myChainFunc;

        if (posVector.Length < 2) {
            return;
        }

        float one = 1.0f/(posVector.Length-1);

        //���������� ��� ����� �� ������ �� �����
        for (int num = 0; num < posVector.Length; num++) {
            float progress = one * num;

            // ������ ����� �������
            Vector3 position = myChain.GetPosition(progress, 1);

            //���������� ��� ����� �� ��� �������
            //���� ��� ������ �����
            if (num == 0) {
                //������ ���������� ��� ������ �� ��� �������
                gameObject.transform.position = position;
                //� ������������ ��� ����� ��� �������� �� ������������� ������� ���� ������
                Quaternion rot = Quaternion.LookRotation(myChain.forvard);
                rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);

                gameObject.transform.rotation = rot;


            }
            else{
                //������������ �������� � ������� ����� �������
                Vector3 forvard = (position - posVector[num - 1].transform.position).normalized;

                //� ������������ ��� ����� ��� �������� �� ������������� ������� � ��������� �������
                Quaternion rot = Quaternion.LookRotation(forvard);
                
                rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
                posVector[num - 1].transform.rotation = rot;

                //������ ��������� ����� �������
                float distance = Vector3.Distance(position, posVector[num - 1].transform.position)*0.01f;
                //������������ ����� �� ��������� ���������
                posVector[num].localPosition = new Vector3(0, 0, distance);

                //������������ ������� ���
                Quaternion rotMesh = posRigs[num].rotation;
                rotMesh.eulerAngles = new Vector3(posVector[num-1].rotation.eulerAngles.x+90, posVector[num-1].rotation.eulerAngles.y, posVector[num-1].rotation.eulerAngles.z);
                posRigs[num].rotation = rotMesh;

                //���������� ������� ���
                posRigs[num].position = posVector[num].position;
            }
        }

        /*
        //������������ �������� � ������� ����� �������
        Vector3 forvard2 = (myChain.next.transform.position - posVector[posVector.Length - 1].transform.position).normalized;

        //� ������������ ��� ����� ��� �������� �� ������������� ������� � ��������� �������
        Quaternion rot2 = Quaternion.LookRotation(forvard2);

        rot2.eulerAngles = new Vector3(rot2.eulerAngles.x, rot2.eulerAngles.y, rot2.eulerAngles.z);
        posVector[posVector.Length - 1].transform.rotation = rot2;

        //������ ��������� ����� �������
        float distance2 = Vector3.Distance(myChain.next.transform.position, posVector[posVector.Length - 1].transform.position) * 0.01f;
        //������������ ����� �� ��������� ���������
        posVector[num].localPosition = new Vector3(0, 0, distance2);

        //������������ ������� ���
        Quaternion rotMesh2 = posRigs[num].rotation;
        rotMesh2.eulerAngles = new Vector3(posVector[num - 1].rotation.eulerAngles.x + 90, posVector[num - 1].rotation.eulerAngles.y, posVector[num - 1].rotation.eulerAngles.z);
        posRigs[num].rotation = rotMesh2;

        //���������� ������� ���
        posRigs[num].position = posVector[num].position;
        */
    }

    private void Start()
    {
        Invoke("ReCalc", 0.5f);
    }

    private void Update()
    {
        ReCalc();
    }

   [SerializeField]
    bool needRecalc = false;

    void ReCalc()
    {
        if (!needRecalc)
            return;

        myChain.iniChain();
        needRecalc = false;
    }
}