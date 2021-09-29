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

    //���������������� �������
    public void Inicialize(float myAngleNew, int myLVLnumStartNew) {
        myAngle = myAngleNew;

        //������� ��� ��� ���� ������� ��� ����� ������
        Quaternion rot = transform.localRotation;
        rot.eulerAngles = new Vector3(Mathf.Abs(myAngle), rot.eulerAngles.y, rot.eulerAngles.z);
        transform.localRotation = rot;

        myLvlNumStart = myLVLnumStartNew;
    }
    public void TestDelete() {
        rotateNow = WorldGenerateScene.main.rotationNow;

        if (Mathf.Abs(WorldGenerateScene.main.rotationNow - myAngle) > 180) {
            Destroy(gameObject);
        }
    }

}
