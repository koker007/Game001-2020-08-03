using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeChain : MonoBehaviour
{
    [SerializeField]
    LVLChain myChain;

    [SerializeField]
    Transform positionBack;
    [SerializeField]
    Transform positionMe;
    [SerializeField]
    Transform positionNext;
    [SerializeField]
    Transform positionNext2;

    [SerializeField]
    SkinnedMeshRenderer mesh;

    //Пересчитать положение костей для модели
    public void ReCalcPositions(LVLChain myChain) {

        //Сперва поворачиваем первую вершину в сторону меня
        if (myChain.back != null)
        {
            //Ставим объект на начало
            SetPosBone(myChain.back.gameObject.transform.position, positionBack);
            //Поворачиваем объект чтобы смотреть на вторую вершину
            SetRotBone(myChain.back.transform.position, myChain.transform.position, positionBack);
        }
        //Если заднего уровня нет то вычисляем точку на дистанции 0.1
        else {
            Vector3 backPos = myChain.transform.position - myChain.transform.forward;
            SetPosBone(backPos, positionBack);
            SetRotBone(backPos, myChain.transform.position, positionBack);
        }

        //Настраиваем вторую вершину
        if (myChain != null)
        {
            SetPosBone(myChain.transform.position, positionMe);
            //Вращяем
            if (myChain.next != null)
                SetRotBone(myChain.transform.position, myChain.next.transform.position, positionMe);
        }

        //Настраиваем третью вершину
        if (myChain.next != null)
        {
            SetPosBone(myChain.next.transform.position, positionNext);
            //Вращяем
            if (myChain.next.next != null)
                SetRotBone(myChain.next.transform.position, myChain.next.next.transform.position, positionNext);

        }
        else
        {

        }

        //Настраиваем последнюю вершину
        if (myChain.next != null && myChain.next.next != null) {
            SetPosBone(myChain.next.next.transform.position, positionNext2);
        }


        void SetPosBone(Vector3 GlobalPosNeed, Transform TransfomObj) {
            TransfomObj.position = GlobalPosNeed;
        }

        void SetRotBone(Vector3 from , Vector3 to, Transform TransfomObj) {
            TransfomObj.rotation.SetFromToRotation(from, to);
        }
    }

    public void ReCalcPositions2(LVLChain myChain)
    {
        float scale = 0.01f;

        Vector3 up = myChain.transform.position - WorldGenerateScene.main.transform.position;

        //Сперва поворачиваем первую кость
        if (myChain.back != null)
        {
            //Ставим объект на начало
            gameObject.transform.position = myChain.back.gameObject.transform.position;

            //Вектор разности растояния
            Vector3 vector = myChain.transform.position - myChain.back.transform.position;


            //разворачиваем обьект чтобы смотреть на следуюшую точку
            Quaternion rot = Quaternion.LookRotation(vector, up);
            rot.eulerAngles = new Vector3(rot.eulerAngles.x+90, rot.eulerAngles.y, rot.eulerAngles.z);
            gameObject.transform.rotation = rot;

            positionMe.transform.localPosition = new Vector3(0, vector.magnitude * scale, 0);
        }
        //Если заднего уровня нет то вычисляем точку на дистанции 0.1
        else
        {
            Vector3 backPos = myChain.transform.position - myChain.transform.forward;
            gameObject.transform.position = backPos;

            //Вектор разности растояния
            Vector3 vector = myChain.transform.position - backPos;
            //Vector3 up = backPos - WorldGenerateScene.main.transform.position;

            //разворачиваем обьект чтобы смотреть на следуюшую точку
            Quaternion rot = Quaternion.LookRotation(vector, up);
            rot.eulerAngles = new Vector3(rot.eulerAngles.x+90, rot.eulerAngles.y, rot.eulerAngles.z);
            gameObject.transform.rotation = rot;

            positionMe.transform.localPosition = new Vector3(0, vector.magnitude * scale, 0);
        }

        //Основная кость
        if (myChain != null && myChain.next != null)
        {
            //Ставим объект на начало
            positionMe.transform.position = myChain.gameObject.transform.position;

            //Вектор разности растояния
            Vector3 vector = myChain.next.transform.position - myChain.transform.position;
            //Vector3 up = myChain.transform.position - WorldGenerateScene.main.transform.position;

            //разворачиваем обьект чтобы смотреть на следуюшую точку
            Quaternion rot = Quaternion.LookRotation(vector);
            rot.eulerAngles = new Vector3(rot.eulerAngles.x+90, rot.eulerAngles.y, rot.eulerAngles.z);
            positionMe.transform.rotation = rot;

            positionNext.transform.localPosition = new Vector3(0, vector.magnitude * scale, 0);
        }

        //последняя кость
        if (myChain.next != null && myChain.next.next != null)
        {
            //Ставим объект на начало
            positionNext.transform.position = myChain.next.gameObject.transform.position;

            //Вектор разности растояния
            Vector3 vector = myChain.next.next.transform.position - myChain.next.transform.position;
            //Vector3 up = myChain.next.transform.position - WorldGenerateScene.main.transform.position;

            //разворачиваем обьект чтобы смотреть на следуюшую точку
            Quaternion rot = Quaternion.LookRotation(vector);
            rot.eulerAngles = new Vector3(rot.eulerAngles.x+90, rot.eulerAngles.y, rot.eulerAngles.z);
            positionNext.transform.rotation = rot;

            positionNext2.transform.localPosition = new Vector3(0, vector.magnitude * scale, 0);
        }

    }


    public void ReCalcTransform(LVLChain myChainFunc)
    {
        myChain = myChainFunc;

        float scale = 10;

        //Узнаем вектор вниз
        Vector3 up = myChain.transform.position - WorldGenerateScene.main.transform.position.normalized;

        if (myChainFunc.next != null)
        {
            //Ставим объект на начало
            gameObject.transform.position = myChainFunc.gameObject.transform.position;

            //Вектор разности растояния
            Vector3 vector = myChainFunc.next.transform.position - myChainFunc.transform.position;


            //разворачиваем обьект чтобы смотреть на следуюшую точку
            Quaternion rot = Quaternion.LookRotation(vector);
            rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
            gameObject.transform.rotation = rot;

            gameObject.transform.localScale = new Vector3(scale, scale, vector.magnitude / 2 * scale);

        }
        else {
            //Ставим объект на начало
            gameObject.transform.position = myChainFunc.gameObject.transform.position;
        }
    }
}
