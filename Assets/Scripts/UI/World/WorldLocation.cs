using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�����
/// <summary>
/// ������������ �� ���� ������ � ������� �������
/// </summary>
public class WorldLocation : MonoBehaviour
{
    /// <summary>
    /// ������ ������� � ��������
    /// </summary>
    [SerializeField]
    public float lenghtAngle = 90;
    [SerializeField]
    float rotateNow = 0;
    [SerializeField]
    public float myAngle = 0;

    [SerializeField]
    public int myLvlNumStart = 0;
    [SerializeField]
    public Vector3[] LVLButtons;

    public GameObject[] Graunds;

    //���������������� �������
    public void Inicialize(float myAngleNew, int myLVLnumStartNew) {
        myAngle = myAngleNew;

        //������� ��� ��� ���� ������� ��� ����� ������
        Quaternion rot = transform.localRotation;
        rot.eulerAngles = new Vector3(Mathf.Abs(myAngle), rot.eulerAngles.y, rot.eulerAngles.z);
        transform.localRotation = rot;

        myLvlNumStart = myLVLnumStartNew;

        CreateGraunds();

        void CreateGraunds() {

            if (Graunds == null)
            {
                Graunds = new GameObject[(int)lenghtAngle / 10];
            }

            //�������� ���� �������
            int plus = 0;
            if ((int)lenghtAngle % 10 > 0.01f)
            {
                plus++;
            }

            //���� ������� ������ ��� ������ ����
            if (Graunds.Length < lenghtAngle / 10)
            {
                //��������� ������
                GameObject[] GraundsNew = new GameObject[((int)lenghtAngle / 10) + plus];
                for (int num = 0; num < Graunds.Length; num++) {
                    GraundsNew[num] = Graunds[num];
                }

                Graunds = GraundsNew;
            }

            //��������� �� �������
            for (int num = 0; num < Graunds.Length; num++) {
                //���� ��� ���� ����� �������
                if (Graunds[num] != null) continue;

                Graunds[num] = Instantiate(WorldGenerateScene.main.PrefGroundLoc10Grad, transform);

                //����� ��������� �� ����������� ������
                Quaternion rotate = Graunds[num].transform.localRotation;
                rotate.eulerAngles = new Vector3(rotate.eulerAngles.x, rotate.eulerAngles.y, rotate.eulerAngles.z + num*10);
                Graunds[num].transform.localRotation = rotate;

                //���� ���� ������������ �� 1
                if (num == Graunds.Length + 1 && plus > 0) {
                    //������ ������ ������� ������
                    Graunds[num].transform.localScale = new Vector3(Graunds[num].transform.localScale.x*0.99f, Graunds[num].transform.localScale.y, Graunds[num].transform.localScale.z);
                }
            }

        }

        void DeleteGraunds() {
            foreach (GameObject gameObject in Graunds) {
                if (gameObject != null) {
                    Destroy(gameObject);
                }
            }
            Graunds = null;
        }
    }
    public void TestDelete() {
        rotateNow = WorldGenerateScene.main.rotationNow;

        if (Mathf.Abs(WorldGenerateScene.main.rotationNow - myAngle) > 180) {
            Destroy(gameObject);
        }
    }

}
