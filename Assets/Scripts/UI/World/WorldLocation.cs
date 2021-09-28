using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //���������������� �������
    public void Inicialize(float myAngleNew) {
        myAngle = myAngleNew;

        //������� ��� ��� ���� ������� ��� ����� ������
        Quaternion rot = transform.localRotation;
        rot.eulerAngles = new Vector3(Mathf.Abs(myAngle), rot.eulerAngles.y, rot.eulerAngles.z);
        transform.localRotation = rot;

    }
    public void TestDelete() {
        rotateNow = WorldGenerateScene.main.rotationNow;

        if (Mathf.Abs(WorldGenerateScene.main.rotationNow - myAngle) > 180) {
            Destroy(gameObject);
        }
    }

}
