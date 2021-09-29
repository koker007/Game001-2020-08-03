using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//семен
/// <summary>
/// Представляет из себя данные о текущей локации
/// </summary>
public class WorldLocation : MonoBehaviour
{
    /// <summary>
    /// Размер локации в градусах
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

    //Инициализировать локацию
    public void Inicialize(float myAngleNew, int myLVLnumStartNew) {
        myAngle = myAngleNew;

        //Вращяем так как того требует наш новый градус
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
